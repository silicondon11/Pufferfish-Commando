using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;

    private bool expFlag = false;
    private float start;
    private float end;

    private MusicController musicController;


    void Start()
    {
        start = Time.time;
        end = Time.time + 0.35f;

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        StartCoroutine(musicController.PlaySoundEffect("Rocket"));
    }

    void Update()
    {
        if ((start >= end) && !expFlag)
        {
            StartCoroutine(Explode());
            expFlag = true;
        }
        start = Time.time;
    }

    private IEnumerator Explode()
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
