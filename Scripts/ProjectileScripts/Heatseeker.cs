using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Heatseeker : MonoBehaviour
{
    public GameObject explosion;

    private bool expFlag = false;
    private float start;
    private float end;

    private GameObject closestTarget;
    private float followSpeed = 1000f;

    private Rigidbody rb;

    private MusicController musicController;


    void Start()
    {
        start = Time.time;
        end = Time.time + 4f;

        rb = transform.GetComponent<Rigidbody>();

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        StartCoroutine(musicController.PlaySoundEffect("Heatseeker"));
    }

    void Update()
    {
        if ((start >= end) && !expFlag)
        {
            expFlag = true;
            StartCoroutine(Explode());
        }
        start = Time.time;

        FindClosestTarget();
        UnityEngine.Vector3 direction = (closestTarget.transform.position - transform.position).normalized;
        UnityEngine.Vector3 force = direction * followSpeed;
        UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(direction);


        transform.LookAt(closestTarget.transform.position);
        rb.AddForce(force);
        
    }

    private void FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        if (targets.Length == 0)
            return;

        closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = UnityEngine.Vector3.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }
    }

    private IEnumerator Explode()
    {
        StartCoroutine(musicController.PlaySoundEffect("Explosion"));

        rb.isKinematic = true;
        rb.velocity = UnityEngine.Vector3.zero;
        transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);

        GameObject exp = Instantiate(explosion, transform.position, UnityEngine.Quaternion.identity);

        Destroy(gameObject);

        yield return null;
    }
}
