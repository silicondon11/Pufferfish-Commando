using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();

        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;
        Color initialColor = material.color;
        float alpha = initialColor.a;

        for (int i = 0; i < 16; i++)
        {
            transform.localScale = transform.localScale + new UnityEngine.Vector3(1666f, 1666f, 1666f);
            sphereCollider.radius = sphereCollider.radius + 0.0000334f;

            if (i > 8)
            {

                alpha = initialColor.a - (initialColor.a * ((float)(i - 8) / 8));
                Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

                material.color = newColor;
            }

            yield return new WaitForSeconds(0.01f);
        }

        Destroy(gameObject);
    }
}
