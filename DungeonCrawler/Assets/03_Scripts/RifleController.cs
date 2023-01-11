using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleController : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] float damage = 7;


    [Header("Manual Setup")]
    [SerializeField] int bulletsPerMinute = 100;
    float fireRate => 60f / bulletsPerMinute;
    float currentfireCount = 0;
    [SerializeField] Projectile projectile;

    private void Update()
    {
        if (currentfireCount > 0)
            currentfireCount -= Time.deltaTime;
    }

    /// <summary>
    /// Disparo con un proyectil externo.
    /// </summary>
    /// <param name="projectile"></param>
    //Este se usa para los enemigos para poder configurar un proyectil directamente desde el Shoot Behaviour
    public void Shoot(Projectile projectile)
    {
        Projectile bullet = Instantiate<Projectile>(projectile, shootPoint.position, shootPoint.rotation);
        bullet.damage += damage;
    }

    /// <summary>
    /// Disparo con la configuracion manual del componente
    /// </summary>
    //Mas util para el player que puede cambiar de arma constantemente y cada arma tiene su propio estilo de
    //disparo y cadencia de fuego
    public void TryShoot()
    {
        if (currentfireCount <= 0)
        {
            Projectile bullet = Instantiate<Projectile>(projectile, shootPoint.position, shootPoint.rotation);
            bullet.damage += damage;
            currentfireCount = fireRate;
        }
    }
}
