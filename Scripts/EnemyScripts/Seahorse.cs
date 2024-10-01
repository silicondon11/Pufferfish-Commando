using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Seahorse : MonoBehaviour
{
    private Pufferfish pfcScript;

    private float moveSpeed = 1000.0f;
    private float attackDistanceThreshold = 200.0f;
    private float attackSpeed = 1f;

    private UnityEngine.Vector3 target;
    private GameObject pfc;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private bool isWalking = false;
    private bool isAttacking = false;

    private bool underCommand = false;
    private int commandIdx;

    //command parameters
    private bool directionChanger = false;

    private Rigidbody rb;
    private Animator animator;

    public List<GameObject> subjects;


    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("SEAHORSE could not find Pufferfish");
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
        }
        else if (!isAttacking)
        {
            Walking();
        }


        if (subjects.Count > 0)
        {
            if (UnityEngine.Vector3.Distance(target, transform.position) <= 500f)
            {
                GiveCommand();
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
            this.animator.Rebind();
            this.animator.Update(0f);

            animator.SetBool("PlayWalk", true);
            isWalking = true;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Walk") && (stateInfo.normalizedTime >= 1.0f))
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
            moveDirection = (target - transform.position).normalized;

            float distanceFactor = (UnityEngine.Vector3.Distance(target, transform.position) - 200f) / 100f;
            float newY = directionChanger ? moveDirection.y * distanceFactor : -moveDirection.y * distanceFactor;

            moveDirection = new UnityEngine.Vector3(moveDirection.x, newY, moveDirection.z);
            directionChanger = !directionChanger;
        }

        transform.LookAt(target);
        transform.Rotate(0f, 180f, 0f);

        rb.AddForce(moveDirection * moveSpeed * Time.timeScale, ForceMode.Impulse);

    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        isWalking = false;

        animator.SetBool("PlayAttack", true);

        bool hf = hypnoFlag;

        yield return new WaitForSeconds(attackSpeed);

        if (hf)
        {
            targetScript = targetGO.GetComponent<TargetControl>();
            if (targetScript != null)
            {
                targetScript.health -= 16;
            }
        }
        else
        {
            pfcScript.health -= 16;
        }

        isAttacking = false;
    }

    private void GiveCommand()
    {
        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i] != null)
            {
                subjects[i].GetComponent<Seahorse>().ReceiveCommand(i);
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

        yield return new WaitForSeconds(2f);

        underCommand = false;
    }


}
