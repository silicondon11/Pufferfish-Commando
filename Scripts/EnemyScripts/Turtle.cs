using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    private Pufferfish pfcScript;
    private TargetControl targetControl;

    private float moveSpeed = 40.0f;
    private float attackDistanceThreshold = 225.0f;
    private float attackSpeed = 2f;
    private float blockSpeed = 1f;

    private UnityEngine.Vector3 target;
    private GameObject pfc;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool isBlocking = false;


    private Rigidbody rb;
    private Animator animator;

    public List<GameObject> subjects;


    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();
        targetControl = gameObject.GetComponent<TargetControl>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("TURTLE could not find Pufferfish");
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
        if (!isAttacking && (UnityEngine.Vector3.Distance(transform.position, target) < attackDistanceThreshold))
        {
            StartCoroutine(Attack());
        }
        else if (!isAttacking && !isBlocking)
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

        moveDirection = (target - transform.position).normalized;

        transform.LookAt(target);
        transform.Rotate(0f, 180f, 0f);

        UnityEngine.Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        transform.position = newPosition;

    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        animator.SetBool("PlayAttack", true);

        bool hf = hypnoFlag;

        yield return new WaitForSeconds(attackSpeed / 2);

        if (hf)
        {
            targetScript = targetGO.GetComponent<TargetControl>();
            if (targetScript != null)
            {
                targetScript.health -= 10;
            }
        }
        else
        {
            pfcScript.health -= 10;
        }

        yield return new WaitForSeconds(attackSpeed / 2);
        isAttacking = false;
    }

    public IEnumerator Block()
    {
        isBlocking = true;

        animator.SetBool("PlayBlock", true);

        yield return new WaitForSeconds(blockSpeed);
        
        targetControl.blockFlag = false;
        isBlocking = false;
    }

}
