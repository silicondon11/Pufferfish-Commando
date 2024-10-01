using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    private float start = 0f;
    private float end = 0f;
    private bool fixedFlag = false;

    void Start()
    {
        start = Time.time;
        end = start;

        GameObject pfc = GameObject.Find("PFC");

        GameObject pent = GameObject.Find("HarpoonLauncher(Clone)");
        Rigidbody rb = pent.GetComponent<Rigidbody>();

        Transform harpoon = transform.Find("Harpoon");
        Transform lastLink = transform.Find("untitled (99)");

        Rigidbody hrb = harpoon.GetComponent<Rigidbody>();

        UnityEngine.Vector3 offset = (pent.transform.position - pfc.transform.position).normalized;
        //lastLink.position = pent.transform.position + (5f * offset);

        //FixedJoint fj = lastLink.gameObject.AddComponent<FixedJoint>();
        //fj.connectedBody = rb;

        hrb.velocity = offset * 1000f;


    }

    void Update()
    {
        start = Time.time;

        if (start >= end && !fixedFlag)
        {
            
            fixedFlag = true;
        }
    }
}
