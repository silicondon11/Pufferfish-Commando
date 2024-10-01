using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

public class CoinControl : MonoBehaviour
{
    private ScoreManager scoreManager;
    private MusicController musicController;

    private float start = 20f;

    private void Start()
    {
        scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
    }

    private void Update()
    {
        transform.Rotate(0f, 0.1f * Time.timeScale, 0f);

        start -= Time.deltaTime;

        if (start <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            collision.gameObject.GetComponent<Projectile>().ricochetFlag = true;

            scoreManager.coins += 1;
            scoreManager.hits += 1;

            StartCoroutine(musicController.PlaySoundEffect("CoinHit"));

            Destroy(transform.gameObject);
        }
    }
}
