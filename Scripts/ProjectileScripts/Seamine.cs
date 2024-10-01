using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Seamine : MonoBehaviour
{
    public GameObject explosion;

    private bool expFlag = false;
    private float start;
    private float end;
    private float lastSecond;

    private MusicController musicController;


    void Start()
    {
        start = Time.time;
        end = Time.time + 8f;

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        StartCoroutine(musicController.PlaySoundEffect("SeamineLauncher"));
    }

    void Update()
    {
        if ((start >= end) && !expFlag)
        {
            expFlag = true;
            StartCoroutine(Explode());
        }
        else if (Mathf.Floor(start) != Mathf.Floor(lastSecond))
        {
            StartCoroutine(musicController.PlaySoundEffect("Seamine"));

            lastSecond = start;
        }

        start = Time.time;
    }

    public IEnumerator Explode()
    {
        StartCoroutine(musicController.PlaySoundEffect("Explosion"));

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = UnityEngine.Vector3.zero;
        transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);

        GameObject exp = Instantiate(explosion, transform.position, UnityEngine.Quaternion.identity);

        Destroy(gameObject);

        yield return null;
    }
}
