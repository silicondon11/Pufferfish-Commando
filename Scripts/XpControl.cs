using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class XpControl : MonoBehaviour
{
    private float spinSpeed = 2880f; // degrees per second
    private int spinTimes = 8;
    private float verticalMoveDistance = 0.1f;
    private float verticalMoveSpeed = 0.5f;
    private float scaleFactor = 0.01f;
    private float scalingTime = 0.25f;

    private Transform xp;
    public int xpBonus;

    private float start = 20f;

    private ScoreManager scoreManager;
    private MusicController musicController;


    // Start is called before the first frame update
    void Start()
    {
        xpBonus = UnityEngine.Random.Range(10, 50);

        scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        start = 20f;
    }

    void Update()
    {
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

            Transform box = transform.Find("Cube.013");
            xp = transform.Find("Cube.026");
            Transform screws = transform.Find("Cylinder.001");

            Destroy(screws.gameObject);
            Destroy(box.gameObject);

            scoreManager.xp += xpBonus;
            scoreManager.hits += 1;

            StartCoroutine(musicController.PlaySoundEffect("XpHit"));
            StartCoroutine(SpinAndMoveEffect());

        }
    }

    private IEnumerator SpinAndMoveEffect()
    {
        float totalRotation = 360 * spinTimes;
        float rotationPerSegment = totalRotation / spinTimes;

        for (int i = 0; i < spinTimes; i++)
        {
            float rotated = 0f;
            while (rotated < rotationPerSegment)
            {
                float rotationThisFrame = spinSpeed * Time.deltaTime;
                xp.Rotate(0, 0, rotationThisFrame);
                rotated += rotationThisFrame;

                float verticalMovement = Mathf.Sin(Time.time * verticalMoveSpeed) * verticalMoveDistance;
                xp.position = new UnityEngine.Vector3(xp.position.x, xp.position.y + verticalMovement, xp.position.z);

                yield return null;
            }
        }

        // Scale down
        UnityEngine.Vector3 initialScale = xp.localScale;
        UnityEngine.Vector3 targetScale = initialScale * scaleFactor;
        float elapsedTime = 0f;
        while (elapsedTime < scalingTime)
        {
            elapsedTime += Time.deltaTime;
            xp.localScale = UnityEngine.Vector3.Lerp(initialScale, targetScale, elapsedTime / scalingTime);
            yield return null;
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
