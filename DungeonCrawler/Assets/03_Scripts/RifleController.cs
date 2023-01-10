using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleController : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform _leftHandIKTarget;
    public Transform leftHandIKTarget => _leftHandIKTarget;

    public void Shoot(Projectile proyectile)
    {
        Instantiate(proyectile, shootPoint.position, shootPoint.rotation);
    }
}
