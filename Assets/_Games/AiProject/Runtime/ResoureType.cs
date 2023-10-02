using UnityEngine;

[CreateAssetMenu (menuName = "Ai Project/Resource Type")]
public class ResoureType : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public string AssociatedStatName { get; private set; }
}
