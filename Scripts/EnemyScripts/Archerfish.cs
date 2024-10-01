using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using UnityEngine;

public class Archerfish : MonoBehaviour
{
    public GameObject projectile;
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.zero;
    public float launchSpeed = 1500f; //change to public when setting all prefabs
    public float reloadTime = 0.25f;
    public float torqueStrength = 0;

    private float attackSpeed = 5f;

    private float orbitRadius = 1500f;   // Desired radius of the circular path
    private float orbitSpeed = 25f; // Speed of orbiting
    private float orbitAngle = 0f;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool rangeAttackFlag = false;
    private bool rangeSignal = false;
    private int side = -1;
    private float lerpScale = 0.005f;

    private UnityEngine.Vector3 randomPoint;
    private UnityEngine.Vector3 target;
    private GameObject pfc;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private Rigidbody rb;
    private Animator animator;
    private Pufferfish pfcScript;

    private MusicController musicController;

    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("ARCHERFISH could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        side = UnityEngine.Random.Range(0, 2) * 2 - 1;
        //lerpScale = UnityEngine.Random.Range(0.0005f, 0.001f);

        StartCoroutine(SignalRangeAttack());
    }

    void FixedUpdate()
    {
        if (!isAttacking && rangeAttackFlag)
        {
            rangeAttackFlag = false;
            StartCoroutine(RangeAttack());
        }
        else if (!isAttacking)
        {
            Walking();
        }

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
        if (!isWalking)
        {
            StartCoroutine(PlayWalkAnimation());
        }

        if (rangeSignal)
        {
            rangeSignal = false;
            StartCoroutine(SignalRangeAttack());

            rangeAttackFlag = true;
        }

        orbitAngle += side * orbitSpeed * Time.deltaTime;
        orbitAngle %= 360f; // Keep the angle between 0 and 360 degrees

        // Calculate the new position in the orbit
        float x = target.x + orbitRadius * Mathf.Cos(orbitAngle * Mathf.Deg2Rad);
        float z = target.z + orbitRadius * Mathf.Sin(orbitAngle * Mathf.Deg2Rad);

        // Assuming you want to keep the y-position constant
        float y = target.y;

        UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(x, y, z);

        transform.LookAt(target);
        transform.Rotate(0f, (float)side * 90f, 0f);
        //transform.position = newPosition;
        transform.position = UnityEngine.Vector3.Lerp(transform.position, newPosition, lerpScale * Time.timeScale);
    }

    private IEnumerator PlayWalkAnimation()
    {
        this.animator.Rebind();
        this.animator.Update(0f);

        animator.SetBool("PlayWalk", true);
        isWalking = true;

        yield return new WaitForSeconds(1f);

        animator.SetBool("PlayWalk", false);
        isWalking = false;

        yield return null;
    }

    private IEnumerator RangeAttack()
    {
        
        isWalking = false;
        isAttacking = true;

        StartCoroutine(musicController.PlaySoundEffect("Poison"));

        animator.SetBool("PlayRangeAttack", true);

        if (projectile != null)
        {

            UnityEngine.Vector3 directionToTarget = (target - transform.position).normalized;

            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(directionToTarget);

            GameObject instantiatedProjectile = Instantiate(projectile, transform.position + (directionToTarget * 100), rot);
            instantiatedProjectile.transform.localScale = instantiatedProjectile.transform.localScale * 0.1f;

            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = instantiatedProjectile.AddComponent<Rigidbody>();
            }

            rb.velocity = directionToTarget * launchSpeed;

            UnityEngine.Vector3 worldRotationAxis = UnityEngine.Vector3.Cross(directionToTarget, axis);
            rb.AddTorque(worldRotationAxis.normalized * torqueStrength, ForceMode.VelocityChange);

            isAttacking = false;

            yield return new WaitForSeconds(attackSpeed);
        }

        rangeAttackFlag = true;

        yield return null;
    }

    private IEnumerator SignalRangeAttack()
    {
        yield return new WaitForSeconds(5f);

        rangeSignal = true;
    }
}

