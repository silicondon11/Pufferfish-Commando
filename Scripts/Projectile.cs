using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool ricochetFlag = false;
    public bool ptFlag = false;
    public bool pinballFlag = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name == "Poison(Clone)")
        {
            if (collision.gameObject.name == "Whirlpool(Clone)")
            {
                ptFlag = true;
            }
        }
        else if (gameObject.name == "Seamine(Clone)")
        {
            if (collision.gameObject.name == "Pent.001")
            {
                pinballFlag = true;
            }
        }
    }
}
