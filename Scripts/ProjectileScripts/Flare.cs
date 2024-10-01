using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{

    private float start;
    private float end;

    private GameObject closestProj;
    private float followSpeed = 200f;

    void Start()
    {
        start = Time.time;
        end = Time.time + 1.5f;
    }

    void Update()
    {
        if (start >= end)
        {
            Destroy(transform.gameObject);
        }
        start = Time.time;

        FindProjectiles();
    }

    void FindProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("EProjectile");

        if (projectiles.Length == 0)
            return;

        foreach (GameObject projectile in projectiles)
        {
            UnityEngine.Vector3 direction = (transform.position - projectile.transform.position).normalized;
            UnityEngine.Vector3 force = direction * followSpeed;
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(direction);

            projectile.transform.LookAt(transform.position);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
