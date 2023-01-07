using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] float sightRange = 12;              //Rango de deteccion de objetivos
    [SerializeField] float reactionSpeed = 7;            //Velocidad de movimiento de la torreta para apuntar al objetivo
    [SerializeField] LayerMask targetMask;               //Capas en las que se buscara al objetivo

    [Header("Turret Parts")]
    [SerializeField] Transform pivot;                    //Soporte vertical de la torreta
    [SerializeField] Transform gun;                      //Seccion de armas de la torreta

    [Header("Shooting")]
    [SerializeField] Proyectile proyectile;              //Proyectil que disparara
    [SerializeField] Transform[] shootingPoints;         //Puntos desde los cuales se dispararan los proyectiles
    [SerializeField] Transform[] cannons;                //Cañones

    Vector3[] cannonsRestingPositions;
    [SerializeField] int roundsPerMinute = 3;
    [SerializeField] float shootStrength = 1;
    float shootInterval => 60f / roundsPerMinute;
    int currentCannon = 0;
    float shootingCounter = 0;

    Transform currentTarget;

    private void Start()
    {
        cannonsRestingPositions = new Vector3[cannons.Length];
        for(int i = 0; i < cannons.Length; i++)
        {
            cannonsRestingPositions[i] = cannons[i].localPosition;
        }
    }

    void Update()
    {
        if (shootingCounter > 0) shootingCounter -= Time.deltaTime;

        if (!currentTarget)
        {
            //Detectar posibles objetivos dentro del area
            Collider[] possibleTargets = Physics.OverlapSphere(transform.position, sightRange, targetMask);
            if (possibleTargets.Length > 0)
            {
                float minDst = float.MaxValue;
                Transform nearestTarget = null;

                //Seleccionar el objetivo mas cercano
                foreach (Collider t in possibleTargets)
                {
                    float sqrDst = (t.transform.position - transform.position).sqrMagnitude;
                    if (sqrDst < minDst)
                    {
                        minDst = sqrDst;
                        nearestTarget = t.transform;
                    }
                }

                currentTarget = nearestTarget;
            }

            //La torreta vuelve a su posicion inicial si no hay objetivo
            pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, Quaternion.identity, reactionSpeed * 0.75f * Time.deltaTime);
            gun.localRotation = Quaternion.RotateTowards(gun.localRotation, Quaternion.identity, reactionSpeed * 0.25f * Time.deltaTime);

            //Los cañones vulven a su posicion si no hay objetivo
            for (int i = 0; i < cannons.Length; i++)
            {
                cannons[i].localPosition = Vector3.MoveTowards(cannons[i].localPosition, cannonsRestingPositions[i], shootInterval * Time.deltaTime * 4);
            }
        }
        else
        {
            if ((currentTarget.position - transform.position).sqrMagnitude > sightRange * sightRange)
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
            Vector3 targetDirection = (currentTarget.position - gun.position).normalized;
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
                for(int i = 0; i < cannons.Length; i++)
                {
                    if (i == currentCannon && shootingCounter <= 0)
                    {
                        cannons[i].localPosition = cannonsRestingPositions[i];
                        Instantiate(proyectile, shootingPoints[i].position, shootingPoints[i].rotation);
                        shootingCounter = shootInterval;
                        cannons[i].localPosition -= Vector3.forward * shootStrength;
                        if (currentCannon >= cannons.Length - 1)
                            currentCannon = 0;
                        else
                            currentCannon++;
                    }

                    //Animacion del cañon para volver a su posicion luego del retroceso del disparo
                    cannons[i].localPosition = Vector3.MoveTowards(cannons[i].localPosition, cannonsRestingPositions[i], shootInterval * Time.deltaTime * 4);
                }
            }
            else
            {
                //Los cañones vuelven a su posicion normal si el objetivo esta fuera de angulo
                for (int i = 0; i < cannons.Length; i++)
                {
                    cannons[i].localPosition = Vector3.MoveTowards(cannons[i].localPosition, cannonsRestingPositions[i], shootInterval * Time.deltaTime * 4);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
