using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask impactMask;
    [SerializeField] float speed = 30;
    [SerializeField] float lifeTime = 4;
    [SerializeField] GameObject hitEffect;

    public float damage = 12;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, speed * Time.deltaTime, impactMask))
        {
            if (hit.collider.TryGetComponent<iDamageable>(out iDamageable damageable))
            {
                damageable.ApplyDamage(damage);
            }
            Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(this.gameObject);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
}
