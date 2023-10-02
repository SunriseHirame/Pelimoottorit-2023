using UnityEngine;

public interface IAiBehaviour
{
    float Evaluate(AiAgent agent);

    void OnEnter (AiAgent agent);
    bool Execute(AiAgent agent);
    void OnExit (AiAgent agent);
}

public abstract class ScriptableAiBehaviour : ScriptableObject, IAiBehaviour
{
    public abstract float Evaluate(AiAgent agent);
    public abstract bool Execute(AiAgent agent);

    public virtual void OnEnter(AiAgent agent)
    {
    }

    public virtual void OnExit(AiAgent agent)
    {
    }
}
