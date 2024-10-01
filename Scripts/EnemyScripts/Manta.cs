using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Manta : MonoBehaviour
{
    private GameObject pfc;
    private Pufferfish pfcScript;

    private float moveSpeed = 200.0f;
    private float attackDistanceThreshold = 200.0f;
    private float attackSpeed = 2f;

    private UnityEngine.Vector3 target;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private bool isWalking = false;
    private bool isAttacking = false;

    private bool underCommand = false;
    private bool commandGiven = false;
    private int commandIdx;

    //command parameters
    private float currentPathAngle = -60f;
    private float angleIncrement = 30f;
    private float initialRadius = 0f;
    private float radiusIncrement = 0.01f;

    private UnityEngine.Quaternion startRot;

    private Rigidbody rb;
    private Animator animator;

    public List<GameObject> subjects;


    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("MANTA could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }


    void Update()
    {
        //if statements to call Walking(), Jump(), Attack()

        if (!isAttacking && (UnityEngine.Vector3.Distance(transform.position, target) < attackDistanceThreshold))
        {
            StartCoroutine(Attack());
            startRot = UnityEngine.Quaternion.Euler(transform.position - target).normalized;
        }
        else if (!isAttacking)
        {
            Walking();
        }
        else
        {
            UnityEngine.Quaternion rot = startRot;
            transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

        if (hypnoFlag)
        {
            target = targetGO.transform.position;
        }
        else
        {
            target = pfc.transform.position;
        }

        if (subjects.Count > 0 && !commandGiven)
        {
            int h = GetComponent<TargetControl>().health;
            if (h <= 20)
            {
                GiveCommand();
                commandGiven = true;
            }
        }
    }

    private void Walking()
    {
        if (!isWalking)
        {
            this.animator.Rebind();
            this.animator.Update(0f);

            animator.SetBool("PlayWalk", true);
            isWalking = true;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Walk") && stateInfo.normalizedTime >= 1.0f)
        {
            isWalking = false;
        }

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3();
        if (!underCommand)
        {
            moveDirection = (target - transform.position).normalized;
        }
        else
        {
            currentPathAngle = commandIdx * angleIncrement;

            float currentRadius = initialRadius + commandIdx * radiusIncrement;

            float x = Mathf.Cos(currentPathAngle) * currentRadius;
            float z = Mathf.Sin(currentPathAngle) * currentRadius;
            float y = Mathf.Sin(currentPathAngle) * currentRadius;

            moveDirection = new UnityEngine.Vector3(x, y, z).normalized;
        }

        transform.LookAt(target);
        transform.Rotate(0f, 90f, 90f);

        UnityEngine.Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        transform.position = newPosition;

    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        bool hf = hypnoFlag;


        animator.SetBool("PlayAttack", true);

        yield return new WaitForSeconds(attackSpeed / 4f);

        if (hf)
        {
            targetScript = targetGO.GetComponent<TargetControl>();
            if (targetScript != null)
            {
                targetScript.health -= 45;
            }
        }
        else
        {
            pfcScript.health -= 45;
        }

        yield return new WaitForSeconds((3f * attackSpeed) / 4f);
        isAttacking = false;
    }

    private void GiveCommand()
    {
        UnityEngine.Debug.LogError("COMM");
        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i] != null)
            {
                subjects[i].GetComponent<Manta>().ReceiveCommandHolder(i);
            }   
        }
    }

    public void ReceiveCommandHolder(int i)
    {
        StartCoroutine(ReceiveCommand(i));
    }

    private IEnumerator ReceiveCommand(int i)
    {
        underCommand = true;
        commandIdx = i;

        yield return new WaitForSeconds(3f);

        underCommand = false;

        yield return null;

    }


}
