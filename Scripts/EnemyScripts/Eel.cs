using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Eel : MonoBehaviour
{
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.zero;
    public float launchSpeed = 1500f; //change to public when setting all prefabs
    public float torqueStrength = 0;

    private float attackSpeed = 1f;
    private float attackLerpScale = 10f;

    private float specialAttackSpeed = 2f;
    private float specialAttackLerpScale = 5f;


    public float speed = 0.00001f; // The speed of the object
    public float orbitWidth = 80f; // The width of the elliptical orbit
    public float orbitHeight = 30f; // The height of the elliptical orbit
    public float verticalOffset = 0.001f; // The maximum vertical offset
    public float verticalSpeed = 0.05f; // The speed of vertical movement

    private float timeCounter = 0.0f;

    private bool isWalking = false;
    private bool isPlayingAttack = false;
    private bool isAttacking = false;
    private int side = -1;
    private float lerpScale = 0.0015f;

    private bool isPlayingSpecialAttack = false;

    private float randomCounter = 0f;
    private float randomNumber = 0f;
    private int odds = 0;

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
            UnityEngine.Debug.LogError("EEL could not find Pufferfish");
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
        if (!isAttacking && (UnityEngine.Vector3.Distance(transform.position, target) <= 500) && (target == pfc.transform.position))
        {
            timeCounter = 0f;

            //Logic to choose between Attack of SpecialAttack
            randomCounter += Time.deltaTime;

            if (randomCounter > randomNumber)
            {
                odds = UnityEngine.Random.Range(0, 10);
                randomCounter = 0f;

                if (odds > 7)
                {
                    randomNumber = specialAttackSpeed;
                }
                else
                {
                    randomNumber = attackSpeed;
                }
            }

            if (odds > 7)
            {
                SpecialAttack();
            }
            else
            {
                Attack();
            }
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
        if (UnityEngine.Vector3.Distance(transform.position, target) <= 500f)
        {
            UnityEngine.Debug.LogError("SLICK");
            target = pfc.transform.position;
            isPlayingAttack = false;
            timeCounter = 0.0f;
        }

        if (!isWalking)
        {
            StartCoroutine(PlayWalkAnimation());
            
        }

        timeCounter += Time.deltaTime;

        float convergence = Mathf.Max((10f - timeCounter) / 10f, 0f);
        // Calculate the position on the ellipse using parametric equations
        float x = target.x + (Mathf.Cos(timeCounter) * orbitWidth * convergence);
        float y = target.y + (Mathf.Sin(timeCounter) * orbitHeight * convergence);
        float z = transform.position.z + Mathf.Sin(timeCounter);

        

        // Smoothly move towards the calculated position
        UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(x, y, z);

        UnityEngine.Vector3 directionToTarget = targetPosition - transform.position;
        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(directionToTarget) * UnityEngine.Quaternion.Euler(0, 180, 0);
        transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, targetRotation, 8f * Time.deltaTime);

        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, Time.timeScale * speed); ;
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

        if (UnityEngine.Vector3.Distance(transform.position, target) <= 400f)
        {
            bool hf = hypnoFlag;

            if (hf)
            {
                targetScript = targetGO.GetComponent<TargetControl>();
                if (targetScript != null)
                {
                    targetScript.health -= 30;
                }
            }
            else
            {
                UnityEngine.Debug.LogError("HEALTH");

                pfcScript.health -= 30;
            }

            List<UnityEngine.Vector3> points = GameObject.Find("GameManager").GetComponent<EnemyManager>().spawnPoints;
            target = points[(int)UnityEngine.Random.Range(0f, 8f)];
        }

        UnityEngine.Vector3 directionToTarget = target - transform.position;
        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(directionToTarget) * UnityEngine.Quaternion.Euler(0, 180, 0);
        transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, targetRotation, 8f * Time.deltaTime);

        //transform.LookAt(target);

        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, target, Time.timeScale * attackLerpScale);
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

    private void SpecialAttack()
    {
        if (!isPlayingSpecialAttack)
        {
            isPlayingSpecialAttack = true;
            StartCoroutine(PlaySpecialAttackAnimation());
        }

        UnityEngine.Debug.LogError("SpecialAttack");

        if (UnityEngine.Vector3.Distance(transform.position, target) <= 400f)
        {
            bool hf = hypnoFlag;

            if (hf)
            {
                targetScript = targetGO.GetComponent<TargetControl>();
                if (targetScript != null)
                {
                    targetScript.health -= 200;
                }
            }
            else
            {
                pfcScript.health -= 200;
            }

            List<UnityEngine.Vector3> points = GameObject.Find("GameManager").GetComponent<EnemyManager>().spawnPoints;
            target = points[(int)UnityEngine.Random.Range(0f, 8f)];
        }

        UnityEngine.Vector3 directionToTarget = target - transform.position;
        UnityEngine.Vector3 perpVector = new UnityEngine.Vector3(directionToTarget.y, -directionToTarget.x, 0);

        target = target + perpVector;

        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(directionToTarget) * UnityEngine.Quaternion.Euler(0, 180, 0);
        transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, targetRotation, 8f * Time.deltaTime);

        //transform.LookAt(target);

        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, target, Time.timeScale * specialAttackLerpScale);
    }

    private IEnumerator PlaySpecialAttackAnimation()
    {
        this.animator.Rebind();
        this.animator.Update(0f);

        animator.SetBool("PlaySpecialAttack", true);
        yield return new WaitForSeconds(specialAttackSpeed);

        isWalking = false;

        //isPlayingAttack = false;
    }
}

