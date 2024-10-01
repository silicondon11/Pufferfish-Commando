using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypno : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        TargetControl tc = collider.gameObject.GetComponent<TargetControl>();

        if (tc != null)
        {
            StartCoroutine(tc.HypnoHit());
        }
    }
}
