using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class HelloWorldPlayer : NetworkBehaviour
{
    //public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    [SerializeField] private CinemachineVirtualCameraBase m_playerFollowCamera;
    [SerializeField] private HelloWorldShooter m_shooter;

    [SerializeField] private float m_movementSpeed = 3f;

    private NetworkVariable<int> score = new NetworkVariable<int>();
    public int Score => score.Value;

    private float xInput;
    private float zInput;
    private bool shootInput;
    private float rotationInput;

    private bool isDead;

    private CancellationTokenSource cts = new CancellationTokenSource();

    private void Awake()
    {
        m_playerFollowCamera.gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        cts.Cancel();
        cts.Dispose();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (isDead) return;

        if (other.CompareTag("Bullet"))
        {
            if (m_shooter.TryGetOwnerForBullet(other.attachedRigidbody, out var owner))
            {
                if (owner == gameObject)
                {
                    Debug.Log($"<color=red>{gameObject} Hit self with bullet!</color>");
                    score.Value--;
                }
                else
                {
                    Debug.Log($"{owner} hit other player {gameObject}");
                    owner.GetComponent<HelloWorldPlayer>().score.Value++;
                }
            }

            isDead = true;
            gameObject.SetActive(false);
            SubmitPlayerDiedClientRpc();
            Destroy(other.gameObject);

            RespawnAsync();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
            m_playerFollowCamera.gameObject.SetActive(true);
        }
    }

    private async void RespawnAsync()
    {
        await Awaitable.WaitForSecondsAsync(1f, cts.Token);

        isDead = false;

        Move();
        gameObject.SetActive(true);
        SubmitRespawnPlayerClientRpc();
    } 

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            //Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        transform.position = GetRandomPositionOnPlane();
        //Position.Value = GetRandomPositionOnPlane();
    }

    [ServerRpc]
    private void SubmitPlayerInputServerRpc (float x, float z, bool shoot, float angle, ServerRpcParams rpcParams = default)
    {
        xInput = Mathf.Clamp(x, -1f, 1f);
        zInput = Mathf.Clamp(z, -1f, 1f);
        shootInput = shoot;
        rotationInput = angle;
    }

    [ClientRpc]
    private void SubmitPlayerDiedClientRpc()
    {
        gameObject.SetActive(false);
    }

    [ClientRpc]
    private void SubmitRespawnPlayerClientRpc()
    {
        gameObject.SetActive(true);
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
    }

    void Update()
    {
        //transform.position = Position.Value;

        if (IsOwner)
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            shootInput = Input.GetMouseButtonDown(0);

            var plane = new Plane(Vector3.down, transform.position);
            var screenToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(screenToWorldRay, out var distance))
            {
                var hitPoint = screenToWorldRay.origin + screenToWorldRay.direction * distance;
                var diretionToHitPoint = hitPoint - transform.position;
                var angle = Vector3.SignedAngle(Vector3.forward, diretionToHitPoint, Vector3.up);
                rotationInput = angle;
            }

            if (!IsServer)
            {
                SubmitPlayerInputServerRpc(xInput, zInput, shootInput, rotationInput);
            }
        }

        if (IsServer)
        {
            Vector3 moveVector = new Vector3(xInput, 0, zInput);
            if (moveVector.magnitude > 1f) moveVector.Normalize();

            transform.rotation = Quaternion.Euler(0f, rotationInput, 0f);
            transform.position += m_movementSpeed * Time.deltaTime * moveVector;

            if (shootInput)
            {
                shootInput = false;
                m_shooter.Shoot();
            }
        }
    }
}
