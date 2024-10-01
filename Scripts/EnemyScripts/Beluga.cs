using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using UnityEngine;

public class Beluga : MonoBehaviour
{
    public GameObject projectile;
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.zero;
    public float launchSpeed = 1500f; //change to public when setting all prefabs
    public float reloadTime = 4f;
    public float torqueStrength = 0;

    private float moveSpeed = 250.0f;
    private float attackSpeed = 2f;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool rangeAttackFlag = false;

    private bool underCommand = false;
    private int commandIdx;
    private UnityEngine.Vector3 commanderPos;
    private bool commandGiven = false;

    public List<GameObject> subjects;

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

    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("BELUGA could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        randomPoint = GenerateRandomPoint();
        lastPoint = transform.position;
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

        if (subjects.Count > 0 && !commandGiven)
        {
            int h = GetComponent<TargetControl>().health;
            if (h <= 35)
            {
                GiveCommand();
                commandGiven = true;
            }
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
        else if (UnityEngine.Vector3.Distance(target, transform.position) <= 600f)
        {
            randomPoint = lastPoint;
        }

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3();
        if (!underCommand)
        {
            moveDirection = (randomPoint - transform.position).normalized;
        }
        else
        {
            randomPoint = commanderPos;
        }

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

        yield return new WaitForSeconds(4f);

        animator.SetBool("PlayWalk", false);
        isWalking = false;

        yield return null;
    }

    private IEnumerator RangeAttack()
    {
        isWalking = false;
        isAttacking = true;

        animator.SetBool("PlayRangeAttack", true);

        float attackDelay = 1f;

        yield return new WaitForSeconds(attackDelay);

        if (projectile != null)
        {

            UnityEngine.Vector3 directionToTarget = (target - transform.position).normalized;

            UnityEngine.Quaternion rot = UnityEngine.Quaternion.LookRotation(directionToTarget);

            UnityEngine.Vector3 right = UnityEngine.Vector3.Cross(UnityEngine.Vector3.up, directionToTarget);
            UnityEngine.Vector3 up = UnityEngine.Vector3.Cross(directionToTarget, right);
            GameObject instantiatedProjectile = Instantiate(projectile, transform.position + (directionToTarget * 300f) + (right * 160f) + (up * 50f), rot);

            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = instantiatedProjectile.AddComponent<Rigidbody>();
            }

            rb.velocity = directionToTarget * launchSpeed;

            UnityEngine.Vector3 worldRotationAxis = UnityEngine.Vector3.Cross(directionToTarget, axis);
            rb.AddTorque(worldRotationAxis.normalized * torqueStrength, ForceMode.VelocityChange);
        }

        yield return new WaitForSeconds(attackSpeed - attackDelay);

        isAttacking = false;
        rangeAttackFlag = false;

        

        yield return null;
    }

    private void GiveCommand()
    {
        UnityEngine.Vector3 comPos = transform.position;
        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i] != null)
            {
                subjects[i].GetComponent<Beluga>().ReceiveCommand(i, comPos);
            }
        }
    }

    public void ReceiveCommandHolder(int i, UnityEngine.Vector3 comPos)
    {
        StartCoroutine(ReceiveCommand(i, comPos));
    }

    private IEnumerator ReceiveCommand(int i, UnityEngine.Vector3 comPos)
    {
        underCommand = true;
        commandIdx = i;
        commanderPos = comPos;
        yield return new WaitForSeconds(6f);

        underCommand = false;
    }

    private UnityEngine.Vector3 GenerateRandomPoint()
    {
        float x = UnityEngine.Random.Range(0, 2) == 0
            ? UnityEngine.Random.Range(0f, 193f)
            : UnityEngine.Random.Range(1193f, 1386f);

        float y = UnityEngine.Random.Range(0, 2) == 0
            ? UnityEngine.Random.Range(0f, 323f)
            : UnityEngine.Random.Range(823f, 1046f);

        float z = UnityEngine.Random.Range(0, 2) == 0
            ? UnityEngine.Random.Range(1022f, 1122f)
            : UnityEngine.Random.Range(1322f, 1422f);

        return new UnityEngine.Vector3(x, y, z);
    }
}
