using System.Diagnostics;
using System.Numerics;
using System.Collections;
using UnityEngine;
using System;

public class PentTarget : MonoBehaviour
{
    public GameObject projectile;

    public float launchSpeed = 2000f; //change to public when setting all prefabs
    public float reloadTime = 0.25f;
    public float torqueStrength = 0;
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.up;

    public bool harpoonFlag = false;
    public GameObject ropePrefab;

    private bool onTarget = false;
    private bool loaded = true;

    private Animator animator;

    private ScoreManager scoreManager;
    private MusicController musicController;


    private void Start()
    {
        animator = GetComponent<Animator>();

        scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (IsTarget(other.gameObject))
        {
            onTarget = true;
            StartCoroutine(OnTargetLoop(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        onTarget = false;
    }

    private bool IsTarget(GameObject obj)
    {
        return obj.CompareTag("Target");
    }

    private void TargetDetected(GameObject target)
    {
        //Projectile
        if (projectile != null)
        {
            
            UnityEngine.Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(directionToTarget);
            if (projectile.name == "Whirlpool")
            {
                rot = rot * UnityEngine.Quaternion.Euler(0f, 180f, 0f);

                StartCoroutine(musicController.PlaySoundEffect("Turbine"));
            }
            else if (projectile.name == "Tomahawk")
            {
                StartCoroutine(musicController.PlaySoundEffect("Tomahawk"));
            }
            else if (projectile.name == "Bullet")
            {
                StartCoroutine(musicController.PlaySoundEffect("Turret"));
            }
            else if (projectile.name == "Bubble")
            {
                StartCoroutine(musicController.PlaySoundEffect("Bubble"));
            }
            else if (projectile.name == "Net")
            {
                rot = rot * UnityEngine.Quaternion.Euler(-90f, 0f, 0f);

                StartCoroutine(musicController.PlaySoundEffect("Net"));
            }
            else if (projectile.name == "Poison")
            {
                StartCoroutine(musicController.PlaySoundEffect("Poison"));
            }
            else if (projectile.name == "Flare")
            {
                StartCoroutine(musicController.PlaySoundEffect("Flare"));
            }
            else if (projectile.name == "EMW")
            {
                StartCoroutine(musicController.PlaySoundEffect("EMWave"));
            }
            else if (projectile.name == "EMW")
            {
                StartCoroutine(musicController.PlaySoundEffect("EMWave"));
            }
            else if (projectile.name == "Hypno")
            {
                StartCoroutine(musicController.PlaySoundEffect("Hypnotizer"));
            }

            GameObject instantiatedProjectile = Instantiate(projectile, transform.position + (directionToTarget * 100), rot);
            StartCoroutine(StartProjectileTimer(instantiatedProjectile));

            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = instantiatedProjectile.AddComponent<Rigidbody>();
            }


            rb.velocity = directionToTarget * launchSpeed;

            UnityEngine.Vector3 worldRotationAxis = UnityEngine.Vector3.Cross(directionToTarget, axis);
            rb.AddTorque(worldRotationAxis.normalized * torqueStrength, ForceMode.VelocityChange);

            scoreManager.shots += 1;
        }

        //Animation
        
        PlayAnyStateAnimation();

    }

    private IEnumerator OnTargetLoop(GameObject targ)
    {
        while (onTarget && loaded)
        {
            TargetDetected(targ);
            loaded = false;
            yield return new WaitForSeconds(reloadTime);
            loaded = true;
        }
        yield return null;
    }

    private IEnumerator StartProjectileTimer(GameObject proj)
    {
        yield return new WaitForSeconds(9f);
        Destroy(proj);
    }

    public void PlayAnyStateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("PlayEntry", true);
            
        }
    }

}
