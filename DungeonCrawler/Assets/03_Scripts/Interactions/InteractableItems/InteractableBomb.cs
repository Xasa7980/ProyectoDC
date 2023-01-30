using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBomb : MonoBehaviour
{
    #region References
    Interaction interaction;
    iDamageable pHealth;
    #endregion

    [SerializeField] float radius;
    [SerializeField] LayerMask targetMask;
    public float timeForActive;
    [SerializeField] float timeToActive;
    public Collider[] playerCheck;
    void Awake()
    {
        interaction = GetComponent<Interaction>();
    }
    void Update()
    {
        StartCounter();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            interaction.interactable = true;
            pHealth = other.GetComponent<iDamageable>();
        }
    }
    void StartCounter()
    {
        if (interaction.interactable)
        {
            timeForActive += Time.deltaTime;

            if (timeForActive > timeToActive)
            {
                Explode();
                timeForActive = 0;
            }
        }
    }
    void Explode()
    {
        if (interaction.GetInteraction())
        {
            float power = pHealth.currentHealth * 0.15f;
            gameObject.SetActive(false);
            if (DetectPlayer())  pHealth.ApplyDamage(power);
        }
    }
    bool DetectPlayer()
    {
        playerCheck = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (playerCheck.Length > 0)
        {
            return true;
        }
        else return false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
