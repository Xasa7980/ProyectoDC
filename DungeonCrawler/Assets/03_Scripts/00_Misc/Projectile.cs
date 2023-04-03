using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask impactMask;
    [SerializeField] float speed = 30;
    [SerializeField] float lifeTime = 4;
    [SerializeField] GameObject hitEffect;
    #region BulletFX 
    [SerializeField] FMODUnity.EventReference bulletFX;
    #endregion
    public float damage = 12;
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(bulletFX);
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, speed * Time.deltaTime, impactMask))
        {
            if (!hit.collider.isTrigger)
            {
                if (hit.collider.TryGetComponent<RaycastEventReciever>(out RaycastEventReciever reciever))
                {
                    reciever.TryInvoke(RaycastEventReciever.RaycastEventType.Shoot, ray);
                }

                if (hit.collider.TryGetComponent<iDamageable>(out iDamageable damageable))
                {
                    damageable.ApplyDamage(damage);
                }
                Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                Destroy(this.gameObject);
            }
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    public bool Validate(Vector3 position)
    {
        return !Physics.CheckSphere(position, 0.2f, impactMask);
    }
}
