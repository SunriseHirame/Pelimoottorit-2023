public class AiCommandedByPlayer : IAiBehaviour
{
    public float Evaluate(AiAgent agent)
    {
        return float.MaxValue;
    }

    public void OnEnter(AiAgent agent)
    {
    }

    public bool Execute (AiAgent agent)
    {
        if (!agent.GetIsAtDestination())
        {
            return false;
        }

        return true;
    }

    public void OnExit(AiAgent agent)
    {
    }
}
