using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleController : WeaponController
{
    [SerializeField] Transform shootPoint;
    [SerializeField, Range(0, 1)] float accuracy = 0.5f;

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
        if (!projectile.Validate(shootPoint.position)) return;

        Vector3 point = shootPoint.position + shootPoint.forward * 30 + Random.onUnitSphere * (1 - accuracy) * 3;
        Vector3 direction = (point - shootPoint.position).normalized;
        Quaternion shootRotation = Quaternion.LookRotation(direction);

        Projectile bullet = Instantiate<Projectile>(projectile, shootPoint.position, shootRotation);
        bullet.damage += damage;
    }

    /// <summary>
    /// Disparo con la configuracion manual del componente
    /// </summary>
    //Mas util para el player que puede cambiar de arma constantemente y cada arma tiene su propio estilo de
    //disparo y cadencia de fuego
    public void TryShoot()
    {
        if (!projectile.Validate(shootPoint.position)) return;

        if (currentfireCount <= 0)
        {
            Vector3 point = shootPoint.position + shootPoint.forward * 30 + Random.onUnitSphere * (1 - accuracy) * 3;
            Vector3 direction = (point - shootPoint.position).normalized;
            Quaternion shootRotation = Quaternion.LookRotation(direction);

            Projectile bullet;
            int upgradeIndex = CalculateUpgrade();
            if (upgradeIndex >= 0)
            {
                Upgrade u = upgrades[upgradeIndex];
                bullet = u.Use<Projectile>(shootPoint);
            }
            else
                bullet = Instantiate<Projectile>(projectile, shootPoint.position, shootRotation);

            bullet.damage += damage;
            currentfireCount = fireRate;
        }
    }
}
