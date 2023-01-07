using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class InteractableBomb : MonoBehaviour
{
    #region References
    Interaction interaction;
    PlayerStats pStats;
    #endregion

    public float timeForActive;
    void Awake()
    {
        interaction = GetComponent<Interaction>();
    }
    void Update()
    {
        StartCounter(0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            interaction.interactable = true;
            pStats = other.GetComponent<PlayerStats>();
        }
    }
    void StartCounter(float timeToActive)
    {
        if (interaction.interactable)
        {
            timeForActive += Time.deltaTime;

            if (timeForActive < timeToActive)
            {
                return;
            }
            else
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
            float power = pStats.CurrentHealth * 0.15f;
            gameObject.SetActive(false);
            pStats.ApplyDamage(power);
        }
    }
}
