using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMW : MonoBehaviour
{
    private float start;
    private float end;

    void Start()
    {
        start = Time.time;
        end = start + 3f;
    }

    void Update()
    {
        if (start < end)
        {
            transform.localScale = transform.localScale + new UnityEngine.Vector3(2f, 2f, 2f);
            start = Time.time;
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }
}
