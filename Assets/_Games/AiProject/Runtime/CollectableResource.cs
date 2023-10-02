using System.Collections.Generic;
using UnityEngine;

public class CollectableResource : MonoBehaviour
{
    private static Dictionary<ResoureType, List<CollectableResource>> _resourceTypeToCollectableMap = new ();

    [SerializeField] private ResoureType m_resourceType;
    [SerializeField] private float m_resourceAmountToAdd = 10f;

    public ResoureType ResourceType => m_resourceType;
    public float ResourceAmountToAdd => m_resourceAmountToAdd;

    public static bool TryGetResourcesOfType (ResoureType resoureType, out List<CollectableResource> resources)
    {
        return _resourceTypeToCollectableMap.TryGetValue (resoureType, out resources) && resources.Count > 0;
    }

    private void OnEnable()
    {
        if (!_resourceTypeToCollectableMap.TryGetValue (m_resourceType, out List<CollectableResource> resourceList))
        {
            resourceList = new List<CollectableResource> ();
            _resourceTypeToCollectableMap.Add (m_resourceType, resourceList);
        }

        resourceList.Add(this);
    }

    private void OnDisable()
    {
        _resourceTypeToCollectableMap[m_resourceType].Remove(this);
    }
}
