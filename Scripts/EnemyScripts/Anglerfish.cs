using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Anglerfish : MonoBehaviour
{
    private float moveSpeed = 50.0f;
    public float maxDistance = 10000f;

    private UnityEngine.Vector3 randomPoint;
    private UnityEngine.Vector3 lastPoint;
    private UnityEngine.Vector3 target;
    private GameObject pfc;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private Rigidbody rb;
    private Pufferfish pfcScript;

    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("ANGLERFISH could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        randomPoint = GenerateRandomPoint();
    }

    void Update()
    {
        Walking();

        if (hypnoFlag)
        {
            target = targetGO.transform.position;
        }
        else
        {
            target = pfc.transform.position;
        }
    }

    private void Walking()
    {
        if (UnityEngine.Vector3.Distance(randomPoint, transform.position) <= 25f)
        {
            lastPoint = randomPoint;
            randomPoint = GenerateRandomPoint();
        }

        else if (UnityEngine.Vector3.Distance(target, transform.position) <= 450f)
        {
            randomPoint = lastPoint;
        }

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3();
        moveDirection = (randomPoint - transform.position).normalized;

        transform.LookAt(target);
        transform.Rotate(-90f, 0f, 0f);

        UnityEngine.Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    UnityEngine.Vector3 GenerateRandomPoint()
    {
        float x = UnityEngine.Random.Range(0f, 1386f);
        float y = UnityEngine.Random.Range(0f, 1046f);
        float z = UnityEngine.Random.Range(1022f, 1422f);


        return new UnityEngine.Vector3(x, y, z);
    }
}
