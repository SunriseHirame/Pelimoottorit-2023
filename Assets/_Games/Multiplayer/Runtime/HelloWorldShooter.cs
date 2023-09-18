using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HelloWorldShooter : MonoBehaviour
{
    private static Dictionary<Rigidbody, GameObject> bulletToOwnerMap = new Dictionary<Rigidbody, GameObject>();

    [SerializeField] private Rigidbody m_projectile;
    [SerializeField] private Transform m_projectileSpawn;

    [SerializeField] private float m_launchSpeed = 10f;

    public bool TryGetOwnerForBullet (Rigidbody bullet, out GameObject owner)
    {
        return bulletToOwnerMap.TryGetValue(bullet, out owner);
    }

    public void Shoot()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var projectile = Instantiate(m_projectile, m_projectileSpawn.position, m_projectileSpawn.rotation);
            projectile.GetComponent<NetworkObject>().Spawn();
            projectile.velocity = projectile.transform.forward * m_launchSpeed;

            bulletToOwnerMap.Add(projectile, gameObject);
        }
    }
}
