using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Lobster : MonoBehaviour
{
    private Pufferfish pfcScript;

    private float moveSpeed = 7.0f;
    private float diveForce = 250f;
    private float jumpForce = 250.0f;
    private float jumpDistanceThreshold = 700.0f;
    private float attackDistanceThreshold = 200.0f;

    private UnityEngine.Vector3 target;
    private GameObject pfc;
    private TargetControl targetScript;
    public GameObject targetGO;

    public bool hypnoFlag = false;

    private bool isWalking = false;
    private bool isJumping = false;
    private bool isAttacking = false;

    private bool underCommand = false;
    private int commandIdx = 0;
    private UnityEngine.Vector3 commanderPos;
    //command parameters

    private Rigidbody rb;
    private Animator animator;

    public List<GameObject> subjects;


    void Start()
    {
        pfc = GameObject.Find("PFC");
        pfcScript = pfc.GetComponent<Pufferfish>();

        if (pfc == null)
        {
            UnityEngine.Debug.LogError("LOBSTER could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }

    
    void FixedUpdate()
    {
        transform.LookAt(target);
        transform.Rotate(0f, 90f, 0f);

        if (!isAttacking && (UnityEngine.Vector3.Distance(transform.position, target) < attackDistanceThreshold))
        {
            StartCoroutine(Attack());
        }
        else if (!isJumping && (UnityEngine.Vector3.Distance(transform.position, target) < jumpDistanceThreshold) && (UnityEngine.Vector3.Distance(transform.position, target) >= attackDistanceThreshold))
        {
            if (transform.position.y > target.y)
            {
                isJumping = true;
                this.animator.Rebind();
                this.animator.Update(0f);

                StartCoroutine(PlayDive());
            }
            else
            {
                isJumping = true;
                this.animator.Rebind();
                this.animator.Update(0f);

                StartCoroutine(PlayJump());
            }
        }
        else if (!isJumping && !isAttacking)
        {
            Walking();
        }


        if (subjects.Count > 0)
        {
            int h = GetComponent<TargetControl>().health;
            //if (h <= 18)
            //{
            //    GiveCommand();
            //}
            if (UnityEngine.Vector3.Distance(target, transform.position) <= 1000f)
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

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Assuming using layer 0.
        if (stateInfo.IsName("Walk") && stateInfo.normalizedTime >= 1.0f)
        {
            // Reset the flag to false to allow the animation to play again.
            isWalking = false;
        }

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3();
        if (!underCommand)
        {
            moveDirection = (target - transform.position).normalized;

            float y = transform.position.y;

            if (transform.position.y >= 1000f)
            {
                y = 1000f - y;
            }
            else if (transform.position.y <= 0f)
            {
                y = 0f - y;
            }
            else
            {
                y = 0f;
            }

            moveDirection.y = y;
        }
        else
        {
            float fanOutDistance = 100.0f;
            float offset = (commandIdx + 1) * fanOutDistance;

            UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(commanderPos.x, commanderPos.y + offset, commanderPos.z);

            moveDirection = (targetPosition - transform.position).normalized;
        }
        //add to all
        
        

        UnityEngine.Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.timeScale;
        transform.position = newPosition;
        //add to all

    }

    private IEnumerator PlayJump()
    {
        isWalking = false;

        animator.SetBool("PlayJump", true);

        for (int i = 0; i < 100; i++)
        {
            if ((i > 20) && (i < 60))
            {
                rb.AddForce((target - transform.position).normalized * jumpForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(0.01f);
        }

        isJumping = false;
    }

    private IEnumerator PlayDive()
    {
        isWalking = false;

        animator.SetBool("PlayDive", true);

        for (int i = 0; i < 100; i++)
        {
            
            rb.AddForce((target - transform.position).normalized * diveForce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.01f);
        }

        isJumping = false;
    }

    private IEnumerator Attack()
    {

        isAttacking = true;

        transform.LookAt(target);

        animator.SetBool("PlayAttack", true);

        bool hf = hypnoFlag;

        yield return new WaitForSeconds(0.5f);

        if (hf)
        {
            targetScript = targetGO.GetComponent<TargetControl>();
            if (targetScript != null)
            {
                targetScript.health -= 50;
            }
        }
        else
        {
            pfcScript.health -= 50;
        }

        yield return new WaitForSeconds(1.5f);

        isAttacking = false;
    }

    private void GiveCommand()
    {
        UnityEngine.Vector3 comPos = transform.position;
        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i] != null)
            {
                subjects[i].GetComponent<Lobster>().ReceiveCommand(i, comPos);
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
        yield return new WaitForSeconds(4f);

        underCommand = false;
    }
}
