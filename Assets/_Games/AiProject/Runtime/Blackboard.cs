using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private Dictionary<string, object> _keyToValueMap = new Dictionary<string, object>();

    public object this[string key] => _keyToValueMap[key];

    public bool TryGetGameObject (string key, out GameObject foundGo)
    {
        if (_keyToValueMap.TryGetValue(key, out var temp))
        {
            foundGo = (GameObject)temp;
            return true;
        }
        foundGo = null;
        return false;
    }

    public bool TryGetFloat(string key, out float foundFloat)
    {
        if (_keyToValueMap.TryGetValue(key, out var temp))
        {
            foundFloat = (float)temp;
            return true;
        }
        foundFloat = 0f;
        return false;
    }

    public void WriteValue (string key, object objectToWrite)
    {
        _keyToValueMap[key] = objectToWrite;
    }
}
