using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] Sensor sensor;
    [SerializeField] float reactionSpeed = 7;            //Velocidad de movimiento de la torreta para apuntar al objetivo
    [SerializeField] float inspectInterval = 5;
    Quaternion inspectRotation;
    float inspectCounter;

    [Header("Turret Parts")]
    [SerializeField] Transform pivot;                    //Soporte vertical de la torreta
    [SerializeField] Transform gun;                      //Seccion de armas de la torreta

    [Header("Shooting")]
    [SerializeField] Projectile projectile;              //Proyectil que disparara
    [SerializeField] Transform[] shootingPoints;         //Puntos desde los cuales se dispararan los proyectiles

    [SerializeField] int roundsPerMinute = 3;

    float shootInterval => 60f / roundsPerMinute;
    int currentCannon = 0;
    float shootingCounter = 0;

    float targetHeightOffset;
    Transform _currentTarget;
    Transform currentTarget
    {
        get => _currentTarget;
        set
        {
            _currentTarget = value;
            if (value)
            {
                Collider targetCollider = value.GetComponent<Collider>();

                switch (targetCollider)
                {
                    case CapsuleCollider capsuleCollider:
                        targetHeightOffset = capsuleCollider.height * 0.75f;
                        break;

                    case BoxCollider boxCollider:
                        targetHeightOffset = boxCollider.size.y * 0.75f;
                        break;

                    case CharacterController characterController:
                        targetHeightOffset = characterController.height * 0.75f;
                        break;

                    default:
                        targetHeightOffset = 0;
                        break;
                }
            }
                
        }
    }

    void Update()
    {
        if (shootingCounter > 0) shootingCounter -= Time.deltaTime;

        if (!currentTarget)
        {
            //La torreta vuelve a su posicion inicial si no hay objetivo
            if (inspectCounter > 0)
                inspectCounter -= Time.deltaTime;
            else
            {
                inspectCounter = inspectInterval;
                Vector2 patrolDirection = Random.insideUnitCircle;
                inspectRotation = Quaternion.LookRotation(new Vector3(patrolDirection.x, 0, patrolDirection.y).normalized);
            }

            pivot.rotation = Quaternion.RotateTowards(pivot.rotation, inspectRotation, reactionSpeed * 0.75f * Time.deltaTime);
            gun.localRotation = Quaternion.RotateTowards(gun.localRotation, Quaternion.identity, reactionSpeed * 0.25f * Time.deltaTime);

            //Detectar posibles objetivos dentro del area
            currentTarget = sensor.GetNearestThreat();
        }
        else
        {
            if (!sensor.InRange(currentTarget.position))
            {
                currentTarget = null;
                return;
            }

            //Calcular la rotacion en horizontal de la torreta
            Vector3 targetHorDirection = (currentTarget.position - transform.position).normalized;
            targetHorDirection.y = 0;
            Quaternion pivotLookRotation = Quaternion.LookRotation(targetHorDirection);
            pivot.rotation = Quaternion.RotateTowards(pivot.rotation, pivotLookRotation, reactionSpeed * Time.deltaTime);

            //Calcular la rotacion vertical del cañon
            Vector3 targetDirection = ((currentTarget.position + Vector3.up * targetHeightOffset) - gun.position).normalized;
            Quaternion gunLookRotation = Quaternion.LookRotation(targetDirection);
            Quaternion rotation = Quaternion.RotateTowards(gun.rotation, gunLookRotation, reactionSpeed * 0.5f * Time.deltaTime);
            Vector3 euler = rotation.eulerAngles;
            euler.y = 0;
            gun.localRotation = Quaternion.Euler(euler);

            //Calcular el angulo del objectivo con respecto al arma (gun)
            float targetAngle = Vector3.Angle(gun.forward, targetDirection);

            if (targetAngle < 5)
            {
                //Start Shooting
                for(int i = 0; i < shootingPoints.Length; i++)
                {
                    if (i == currentCannon && shootingCounter <= 0)
                    {
                        Instantiate(projectile, shootingPoints[i].position, shootingPoints[i].rotation);
                        shootingCounter = shootInterval;

                        if (currentCannon >= shootingPoints.Length - 1)
                            currentCannon = 0;
                        else
                            currentCannon++;
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {

    }
}
