using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    public GameObject projectile;
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.zero;
    public float launchSpeed = 1500f; //change to public when setting all prefabs
    public float reloadTime = 0.25f;
    public float torqueStrength = 0;

    private float moveSpeed = 150.0f;
    private float attackSpeed = 1f;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool rangeAttackFlag = false;

    private UnityEngine.Vector3 randomPoint;
    private UnityEngine.Vector3 lastPoint;
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
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("OCTOPUS could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        randomPoint = GenerateRandomPoint();
    }

    void Update()
    {
        if (!isAttacking && rangeAttackFlag)
        {
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

        if (UnityEngine.Vector3.Distance(randomPoint, transform.position) <= 50f)
        {
            rangeAttackFlag = true;
            lastPoint = randomPoint;
            randomPoint = GenerateRandomPoint();
        }
        else if (UnityEngine.Vector3.Distance(target, transform.position) <= 350f)
        {
            randomPoint = lastPoint;
        }

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3();
        moveDirection = (randomPoint - transform.position).normalized;

        transform.LookAt(target);
        transform.Rotate(0f, 180f, 0f);

        UnityEngine.Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
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

        animator.SetBool("PlayRangeAttack", true);

        if (projectile != null)
        {

            UnityEngine.Vector3 directionToTarget = (target - transform.position).normalized;

            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(directionToTarget);

            for (int i = 0; i < 8; i++)
            {
                StartCoroutine(musicController.PlaySoundEffect("Turret"));

                GameObject instantiatedProjectile = Instantiate(projectile, transform.position + (directionToTarget * 100), rot);

                Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = instantiatedProjectile.AddComponent<Rigidbody>();
                }

                rb.velocity = directionToTarget * launchSpeed;

                UnityEngine.Vector3 worldRotationAxis = UnityEngine.Vector3.Cross(directionToTarget, axis);
                rb.AddTorque(worldRotationAxis.normalized * torqueStrength, ForceMode.VelocityChange);

                yield return new WaitForSeconds(attackSpeed/8f);
            }
        }

        isAttacking = false;
        rangeAttackFlag = false;

        yield return null;
    }

    UnityEngine.Vector3 GenerateRandomPoint()
    {
        float x = UnityEngine.Random.Range(0, 2) == 0
            ? UnityEngine.Random.Range(0f, 293f)
            : UnityEngine.Random.Range(1093f, 1386f);

        float y = UnityEngine.Random.Range(0, 2) == 0
            ? UnityEngine.Random.Range(0f, 323f)
            : UnityEngine.Random.Range(823f, 1046f);

        float z = UnityEngine.Random.Range(0, 2) == 0
            ? UnityEngine.Random.Range(1022f, 1122f)
            : UnityEngine.Random.Range(1322f, 1422f);

        return new UnityEngine.Vector3(x, y, z);
    }
}
