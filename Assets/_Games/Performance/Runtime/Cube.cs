using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateable
{
    void OnUpdate();
}

public abstract class Updateable : MonoBehaviour
{
    public abstract void OnAbstractUpdate();
}

public class Cube : Updateable, IUpdateable
{
    private void Awake()
    {
        transform.position = Random.onUnitSphere * Random.value * 20f;
        //GetComponent<Renderer>().material.color = Random.ColorHSV();
        //var pb = new MaterialPropertyBlock();
        //pb.SetColor("_BaseColor", Random.ColorHSV());
        //GetComponent<Renderer>().SetPropertyBlock(pb);
    }

    private void OnDestroy()
    {
        //Destroy(GetComponent<Renderer>().material);
    }

    public void DirectUpdate()
    {
    }

    public override void OnAbstractUpdate()
    {
    }

    public void OnUpdate()
    {
    }

    private void Update()
    {
    }
}
