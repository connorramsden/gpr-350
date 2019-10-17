using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    Camera cam;
    public GameObject player;
    
    private void Update()
    {
        var viewPointThing = cam.WorldToViewportPoint(player.transform.position);

        if(viewPointThing.y > 1)
        {
            PlayerStats.didPlayerWin = true;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }
}
