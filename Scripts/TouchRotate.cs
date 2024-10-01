using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    public GameObject pfc;
    private float rotSpeed = 50;

    private UnityEngine.Vector2 touchStartPos;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Moved:

                    UnityEngine.Vector2 delta = touch.position - touchStartPos;

                    float rotX = delta.x * rotSpeed * Mathf.Deg2Rad;
                    float rotY = delta.y * rotSpeed * Mathf.Deg2Rad;

                    pfc.transform.Rotate(UnityEngine.Vector3.back, rotX);
                    pfc.transform.Rotate(UnityEngine.Vector3.right, rotY);

                    touchStartPos = touch.position;
                    break;
            }
        }
    }
}
