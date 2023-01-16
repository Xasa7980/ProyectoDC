using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeParts_VFX : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject[] parts;
    [SerializeField] float explosionForce = 10;
    [SerializeField] float explosionRadius = 4;

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        foreach(GameObject part in parts)
        {
            if (part.TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = true;
                Destroy(collider, 4.5f);
            }

            part.transform.parent = null;
            Rigidbody rb = part.AddComponent<Rigidbody>();
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Destroy(rb, 4.5f);
        }
    }
}
