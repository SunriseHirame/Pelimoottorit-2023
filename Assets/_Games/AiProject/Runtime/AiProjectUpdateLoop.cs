using UnityEngine;

public class AiProjectUpdateLoop : MonoBehaviour
{
    private void Update()
    {
        for (int i = AiAgent.All.Count - 1; i >= 0; i--)
        {
            AiAgent agent = AiAgent.All[i];
            agent.OnUpdate();
        }
    }
}
