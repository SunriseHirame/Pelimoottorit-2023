using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class UpdateManager : MonoBehaviour
{

    public Cube[] _cubes;
    public Updateable[] _updateables;
    public IUpdateable[] _iUpdatbles;

    private void Awake()
    {
        _cubes = GameObject.FindObjectsByType<Cube>(FindObjectsSortMode.None);
        print(_cubes.Length);

        _updateables = new Updateable[_cubes.Length];
        _iUpdatbles = new IUpdateable[_cubes.Length];
        for (int i = 0; i < _cubes.Length; i++)
        {
            Cube cube = _cubes[i];
            _updateables[i] = (Updateable)cube;
            _iUpdatbles[i] = (IUpdateable)cube;
        }
    }

    private void Update()
    {
        foreach (var cube in _cubes)
        {
            cube.DirectUpdate();
        }

        Profiler.BeginSample("DirectUpdate");
        foreach (var cube in _cubes)
        {
            cube.DirectUpdate();
        }

        Profiler.EndSample();
        Profiler.BeginSample("OnAbstractUpdate");

        foreach (var cube in _updateables)
        {
            cube.OnAbstractUpdate();
        }

        Profiler.EndSample();
        Profiler.BeginSample("OnUpdate");

        foreach (var cube in _iUpdatbles)
        {
            cube.OnUpdate();
        }
        Profiler.EndSample();
    }
}
