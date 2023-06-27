using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject Followed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Followed.transform.position;
    }
}
