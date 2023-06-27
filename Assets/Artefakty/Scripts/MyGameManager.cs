using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    private List<GoblinController> Goblins;

    void Start()
    {
        Goblins = FindObjectsOfType<GoblinController>().ToList();
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        foreach(var goblin in Goblins)
        {
            goblin.IsSelected = false;
        }

        foreach(var hit in hits)
        {
            var goblin = hit.collider.GetComponentInParent<GoblinController>();
            if(goblin != null)
            {
                goblin.IsSelected = true;
            }
        }
    }
}
