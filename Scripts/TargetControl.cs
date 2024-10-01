using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    public TargetType type;
    public bool deathFlag = false;
    public int health = 100;

    private bool pauseFlag = false;
    private float speed = 1f;
    private bool hypnoFlag = false;
    public bool blockFlag = false;

    private Coroutine hypnoCoroutine;

    private Rigidbody rb;
    private Renderer rend;
    private Color initColor;

    private ScoreManager scoreManager;
    private AchievementManager achievementManager;

    private MusicController musicController;

    private GameObject pfc;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();

        if ((rend.material.color != Color.yellow) && (rend.material.color != Color.magenta))
        {
            initColor = rend.material.color;
        }

        GameObject gm = GameObject.Find("GameManager");
        scoreManager = gm.GetComponent<ScoreManager>();
        achievementManager = gm.GetComponent<AchievementManager>();
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
        pfc = GameObject.Find("PFC");
    }

    void Update()
    {
        EnemyControl(pauseFlag, speed, hypnoFlag);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckBlock();

        GameObject hitBy = collision.gameObject;

        if (!blockFlag)
        {

            if (hitBy.CompareTag("Target"))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddForce((transform.position - hitBy.transform.position).normalized * 50000f);

            }
            else if (hitBy.name == "Tomahawk(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.tomahawk += 1;
                scoreManager.totalCols += 1;

                health -= 20;
                Destroy(hitBy);
            }
            else if (hitBy.name == "Bullet(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.turret += 1;
                scoreManager.totalCols += 1;

                health -= 6;
                Destroy(hitBy);
            }
            else if (hitBy.name == "Whirlpool(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.turbine += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(WhirlpoolHit(hitBy));
            }
            else if (hitBy.name == "Bubble(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.bubble += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(BubbleHit(hitBy));
            }
            else if (hitBy.name == "Net(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.net += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(NetHit(hitBy));
            }
            else if (hitBy.name == "Rocket(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.rocket += 1;
                scoreManager.totalCols += 1;

                health -= 35;
                Destroy(hitBy);
            }
            else if (hitBy.name == "Explosion(Clone)")
            {
                scoreManager.hits += 1;

                health -= 15;
            }
            else if (hitBy.name == "Cylinder.001") //Umbrella
            {
                scoreManager.umbrella += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(musicController.PlaySoundEffect("UmbrellaShield"));

                rb.AddForce(hitBy.transform.rotation * UnityEngine.Vector3.forward * 100000f, ForceMode.Impulse);
            }
            else if (hitBy.name == "Poison(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.poison += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(PoisonHit(hitBy));
            }
            else if (hitBy.name == "Heatseeker(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.heatseeker += 1;
                scoreManager.totalCols += 1;

                health -= 50;
                Destroy(hitBy);//maybe change to explode
            }
            else if (hitBy.name == "Flare(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.flare += 1;
                scoreManager.totalCols += 1;

                health -= 2;
                Destroy(hitBy);
            }
            else if (hitBy.name == "EMW(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.emw += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(EMWHit(hitBy));
            }
            else if (hitBy.name == "Seamine(Clone)")
            {
                scoreManager.hits += 1;
                scoreManager.seamine += 1;
                scoreManager.totalCols += 1;

                health -= 60;
                Seamine sm = hitBy.GetComponent<Seamine>();
                StartCoroutine(sm.Explode());
            }
            else if (hitBy.name == "PFC_hook")
            {
                StartCoroutine(musicController.PlaySoundEffect("SwingingHook"));

                StartCoroutine(AttachHook(hitBy));

            }
            else if (hitBy.name == "Pent.001") //Spring
            {
                
                scoreManager.spring += 1;
                scoreManager.totalCols += 1;

                StartCoroutine(musicController.PlaySoundEffect("SpringBoard"));

                UnityEngine.Quaternion rot = hitBy.transform.rotation;
                UnityEngine.Vector3 forceDirection = rot * UnityEngine.Vector3.forward;
                rb.AddForce(forceDirection * 100000f, ForceMode.Impulse);
            }
            else if (hitBy.name == "Hypno(Clone)")
            {

                scoreManager.hits += 1;
                scoreManager.hypno += 1;
                scoreManager.totalCols += 1;

                if (hypnoCoroutine != null)
                {
                    StopCoroutine(hypnoCoroutine);
                }
                hypnoCoroutine = StartCoroutine(HypnoHit());
            }
            //else if (hitBy.name == "Harpoon")
            //{
            //    scoreManager.hits += 1;
            //    scoreManager.harpoon += 1;
            //    scoreManager.totalCols += 1;

            //    StartCoroutine(HarpoonHit(hitBy));

            //}

        }

        if (health <= 0)
        {
            deathFlag = true;

            if (UnityEngine.Vector3.Distance(transform.position,pfc.transform.position) >= 700f)
            {
                achievementManager.scubahSnipahFlag = true;
            }

            if (hitBy.GetComponent<Projectile>() != null)
            {
                if (hitBy.GetComponent<Projectile>().ricochetFlag)
                {
                    achievementManager.ricochetFlag = true;
                }

                if (hitBy.GetComponent<Projectile>().ptFlag)
                {
                    achievementManager.ptFlag = true;
                }

                if (hitBy.GetComponent<Projectile>().pinballFlag)
                {
                    achievementManager.pinballFlag = true;
                }
            }
        }
    }


    private void EnemyControl(bool flag, float speed, bool hFlag)
    {
        if (!flag)
        {
            rb.velocity = UnityEngine.Vector3.zero;
        }
    }


    private IEnumerator WhirlpoolHit(GameObject whirlpool)
    {
        pauseFlag = true;

        rb.AddForce((transform.position - whirlpool.transform.position).normalized * 100f, ForceMode.Impulse);

        rb.AddTorque(new UnityEngine.Vector3(0f, 30f, 0f), ForceMode.Impulse);

        yield return new WaitForSeconds(3f);

        Destroy(whirlpool);
        pauseFlag = false;

        yield return null;
    }

    private IEnumerator BubbleHit(GameObject bubble)
    {
        pauseFlag = true;

        transform.gameObject.tag = "Untagged";

        bubble.transform.position = transform.position;

        transform.SetParent(bubble.transform);

        rb.isKinematic = true;

        Rigidbody brb = bubble.GetComponent<Rigidbody>();
        brb.AddForce(new UnityEngine.Vector3(0f, 20f, 0f), ForceMode.Impulse);

        yield return new WaitForSeconds(4f);

        rb.isKinematic = false;
        transform.SetParent(null);
        pauseFlag = false;
        Destroy(bubble);
        transform.gameObject.tag = "Target";


        yield return null;
    }

    private IEnumerator NetHit(GameObject net)
    {
        pauseFlag = true;

        transform.gameObject.tag = "Untagged";

        net.transform.position = transform.position;
        net.transform.rotation = UnityEngine.Quaternion.identity;

        rb.velocity = UnityEngine.Vector3.zero;

        transform.SetParent(net.transform);
        
        Rigidbody nrb = net.GetComponent<Rigidbody>();
        nrb.velocity = UnityEngine.Vector3.zero;
        nrb.velocity = new UnityEngine.Vector3(0f, -200f, 0f);

        yield return new WaitForSeconds(7f);

        transform.SetParent(null);
        pauseFlag = false;
        Destroy(net);
        transform.gameObject.tag = "Target";

        yield return null;
    }

    private IEnumerator PoisonHit(GameObject poison)
    {
        speed = 0.1f;

        rend.material.color = Color.magenta;

        for (int i = 0; i < 4; i++)
        {
            health -= 9;
            yield return new WaitForSeconds(2f);
        }
        Destroy(poison);

        speed = 1f;
        rend.material.color = initColor;

        yield return null;
    }

    private IEnumerator HarpoonHit(GameObject harpoon)
    {
        transform.SetParent(harpoon.transform);
        yield return new WaitForSeconds(3f);

        transform.SetParent(null);
    }

    private IEnumerator EMWHit(GameObject emw)
    {
        pauseFlag = true;

        health -= 16;

        rb.AddForce(emw.transform.rotation * UnityEngine.Vector3.forward * 15000f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        pauseFlag = false;

        yield return null;
    }

    public IEnumerator HypnoHit()
    {
        hypnoFlag = true;
        string hypnoFlagVar = "hypnoFlag";
        string targetString = "targetGO";

        System.Type targetType = System.Type.GetType(type.ToString());

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        GameObject targetHolder = FindClosestObject(targets);

        if (targetType != null)
        {
            Component targetComponent = gameObject.GetComponent(targetType);

            if (targetComponent != null)
            {
                FieldInfo hVariableInfo = targetType.GetField(hypnoFlagVar);
                FieldInfo tVariableInfo = targetType.GetField(targetString);

                if (hVariableInfo != null && tVariableInfo != null)
                {
                    tVariableInfo.SetValue(targetComponent, targetHolder);
                    hVariableInfo.SetValue(targetComponent, true);

                    rend.material.color = Color.green;

                    yield return new WaitForSeconds(2f);

                    hypnoFlag = false;
                    hVariableInfo.SetValue(targetComponent, false);
                }
            }
        }

        rend.material.color = initColor;

        yield return null;
    }

    private IEnumerator AttachHook(GameObject hook)
    {
        transform.SetParent(hook.transform);

        health -= 8;

        scoreManager.hook += 1;
        scoreManager.totalCols += 1;

        yield return new WaitForSeconds(5f);

        transform.SetParent(null);
    }

    private void CheckBlock()
    {
        if (type == TargetType.Turtle)
        {
            int rand = UnityEngine.Random.Range(1, 11);
            if (rand <= 7)//change defense level here
            {
                blockFlag = true;
                Turtle script = GetComponent<Turtle>();
                StartCoroutine(script.Block());
            }
        }
        //...
    }

    private GameObject FindClosestObject(GameObject[] objectArray)
    {
        if (objectArray == null || objectArray.Length == 0)
        {
            UnityEngine.Debug.LogError("Object array is null or empty.");
            return null;
        }

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objectArray)
        {
            if (obj != null && obj != gameObject)
            {
                float distance = UnityEngine.Vector3.Distance(gameObject.transform.position, obj.transform.position);

                if (distance < closestDistance)
                {
                    closestObject = obj;
                    closestDistance = distance;
                }
            }
        }

        return closestObject;
    }
}
