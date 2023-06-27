using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float TimeToLive;
    void Start()
    {
        Destroy(this.gameObject, TimeToLive);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void SetColor(Color kolor)
    {
        foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            var property = new MaterialPropertyBlock();
            property.SetColor("_Color", kolor);
            meshRenderer.SetPropertyBlock(property);
        }
    }
}
