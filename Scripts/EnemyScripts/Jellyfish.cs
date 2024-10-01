using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    public UnityEngine.Vector3 axis = UnityEngine.Vector3.zero;
    public float launchSpeed = 1500f; //change to public when setting all prefabs
    public float reloadTime = 0.25f;
    public float torqueStrength = 0;
    private float explodeSpeed = 2f;

    public GameObject explosion;
    private Coroutine coroutine;

    private float moveSpeed = 50.0f;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool attackFlag = false;
    public bool explodeFlag = false;

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
            UnityEngine.Debug.LogError("JELLYFISH could not find Pufferfish");
        }
        else
        {
            target = pfc.transform.position;
        }

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
    }

    void Update()
    {
        if (!isAttacking && attackFlag && !explodeFlag)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Explode());
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

        if (UnityEngine.Vector3.Distance(target, transform.position) <= 250f)
        {
            attackFlag = true;
        }

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3();
        moveDirection = (target - transform.position).normalized;

        transform.LookAt(target);
        //transform.Rotate(0f, 180f, 0f);

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

    public IEnumerator Explode()
    {
        isWalking = false;
        isAttacking = true;

        animator.SetBool("PlayExplode", true);

        yield return new WaitForSeconds(explodeSpeed);

        StartCoroutine(musicController.PlaySoundEffect("Explosion"));

        GameObject exp = Instantiate(explosion, transform.position, UnityEngine.Quaternion.identity);

        Destroy(gameObject);

    }
}
