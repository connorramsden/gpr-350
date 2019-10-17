using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code taken from https://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055

public class Screenwrapper : MonoBehaviour
{
    private static List<Renderer> renderers;

    private static Vector3 viewportPosition;

    private static Camera cam;

    public static void WrapToScreen(ref Vector2 positionToWrap)
    {
        viewportPosition = cam.WorldToViewportPoint(positionToWrap);

        if (viewportPosition.x > 1 || viewportPosition.x < 0)
        {
            positionToWrap.x = -positionToWrap.x;
        }

        if (viewportPosition.y > 1 || viewportPosition.y < 0)
        {
            positionToWrap.y = -positionToWrap.y;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }
}
