using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionScript : MonoBehaviour
{
    int bounceCount;

    Transform objectTransform;

    Vector3 maxSize = new Vector3(40.0f, 40.0f, 40.0f);

    private void Start()
    {
        bounceCount = 0;
        objectTransform = gameObject.GetComponentInParent<Transform>();
        Debug.Log(objectTransform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        MakeBallBigger();
    }

    private void MakeBallBigger()
    {
        bounceCount++;
        Debug.Log($"The ball has bounced {bounceCount} times!");

        //objectTransform.lossyScale.Set(bounceCount, bounceCount, bounceCount);
        objectTransform.localScale += new Vector3(bounceCount, bounceCount, bounceCount);

        Debug.Log(objectTransform.localScale);
    }
}
