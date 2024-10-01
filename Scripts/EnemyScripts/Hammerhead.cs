using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Hammerhead : MonoBehaviour
{
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.zero;
    public float launchSpeed = 1500f; //change to public when setting all prefabs
    public float torqueStrength = 0;

    private float attackSpeed = 3f;
    private float attackLerpScale = 0.015f;

    private float orbitRadius = 3500f;   // Desired radius of the circular path
    private float orbitSpeed = 60f; // Speed of orbiting
    private float orbitAngle = 0f;

    private bool isWalking = false;
    private bool isPlayingAttack = false;
    private bool isAttacking = false;
    private int side = -1;
    private float lerpScale = 0.0015f;

    private UnityEngine.Vector3 randomPoint;
    private UnityEngine.Vector3 target;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private Rigidbody rb;
    private Animator animator;
    private Pufferfish pfcScript;
    private GameObject pfc;

    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("HAMMERHEAD could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        side = UnityEngine.Random.Range(0, 2) * 2 - 1;
        //lerpScale = UnityEngine.Random.Range(0.0005f, 0.001f);
    }

    void FixedUpdate()
    {
        if (!isAttacking && (UnityEngine.Vector3.Distance(transform.position, target) <= 700) && (target == pfc.transform.position))
        {
            Attack();
        }
        else if (!isAttacking)
        {
            Walking();
        }

        if (hypnoFlag)
        {
            target = targetGO.transform.position;
        }
    }

    private void Walking()
    {
        if (UnityEngine.Vector3.Distance(transform.position, target) <= 300)
        {
            target = pfc.transform.position;
            isPlayingAttack = false;
        }

        if (!isWalking)
        {
            StartCoroutine(PlayWalkAnimation());
        }

        if (target == pfc.transform.position)
        {
            orbitAngle += side * orbitSpeed * Time.deltaTime;
            orbitAngle %= 360f; // Keep the angle between 0 and 360 degrees

            // Calculate the new position in the orbit
            float x = target.x + orbitRadius * Mathf.Cos(orbitAngle * Mathf.Deg2Rad);
            float z = target.z + orbitRadius * Mathf.Sin(orbitAngle * Mathf.Deg2Rad);

            // Assuming you want to keep the y-position constant
            float y = target.y;
            y = Mathf.Sin(Time.time * 0.75f) * 100.0f; //alter for each enemy

            UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(x, y, z);

            transform.LookAt(target);
            transform.Rotate(0f, (float)side * 90f, 0f);

            //transform.position = newPosition;
            transform.position = UnityEngine.Vector3.Lerp(transform.position, newPosition, Time.timeScale * lerpScale);
        }
        else
        {
            //transform.LookAt(target);
            UnityEngine.Vector3 directionToTarget = target - transform.position;
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(directionToTarget);
            transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, targetRotation, 8f * Time.deltaTime);

            transform.position = UnityEngine.Vector3.Lerp(transform.position, target, Time.timeScale * lerpScale * 2f);
        }
    }

    private IEnumerator PlayWalkAnimation()
    {
        this.animator.Rebind();
        this.animator.Update(0f);

        animator.SetBool("PlayWalk", true);
        isWalking = true;

        yield return new WaitForSeconds(4f);

        isWalking = false;

        yield return null;
    }

    private void Attack()
    {
        if (!isPlayingAttack)
        {
            isPlayingAttack = true;
            StartCoroutine(PlayAttackAnimation());
        }

        if (UnityEngine.Vector3.Distance(transform.position, target) <= 300f)
        {
            bool hf = hypnoFlag;

            if (hf)
            {
                targetScript = targetGO.GetComponent<TargetControl>();
                if (targetScript != null)
                {
                    targetScript.health -= 115;
                }
            }
            else
            {
                pfcScript.health -= 115;
            }

            List<UnityEngine.Vector3> points = GameObject.Find("GameManager").GetComponent<EnemyManager>().spawnPoints;
            target = points[(int)UnityEngine.Random.Range(0f, 8f)];
        }

        UnityEngine.Vector3 directionToTarget = target - transform.position;
        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(directionToTarget);
        transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, targetRotation, 8f * Time.deltaTime);

        //transform.LookAt(target);

        transform.position = UnityEngine.Vector3.Lerp(transform.position, target, Time.timeScale * attackLerpScale);
    }

    private IEnumerator PlayAttackAnimation()
    {
        this.animator.Rebind();
        this.animator.Update(0f);

        animator.SetBool("PlayAttack", true);
        yield return new WaitForSeconds(attackSpeed);

        isWalking = false;

        //isPlayingAttack = false;
    }
}
