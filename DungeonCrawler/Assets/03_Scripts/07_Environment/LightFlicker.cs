using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light lightComponent;

    [Header("Onn / Off Settings")]
    [SerializeField, Range(0, 1)] float offChance = 0.5f;

    [Header("Intensity Settings")]
    [SerializeField] float minIntensity = 0.5f;
    [SerializeField] float maxIntensity = 1;

    [Header("Flicker Settings")]
    [SerializeField, Range(0, 1)] float flickerChance = 0.5f;
    [SerializeField] float maxOffTime = 1;
    [SerializeField] float maxOnnTime = 2;
    [SerializeField] int flickerCounter = 4;

    [Header("Disable Effect")]
    [SerializeField] GameObject disableEffect;
    [SerializeField, Range(0, 1)] float disableEffectChance = 0.5f; 

    float counter;

    // Start is called before the first frame update
    void Start()
    {
        lightComponent = GetComponent<Light>();
        lightComponent.intensity = Random.Range(minIntensity, maxIntensity);

        if (Random.value < offChance)
        {
            //lightComponent.enabled = false;
            if (Random.value < disableEffectChance)
            {
                Instantiate(disableEffect, transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
        else if (Random.value < flickerChance)
        {
            StartCoroutine(OnnOff());
        }
    }

    IEnumerator OnnOff()
    {
        while (true)
        {
            if (lightComponent.enabled == true)
                counter = Random.Range(maxOffTime * 0.5f, maxOffTime * 1.5f);
            else
                counter = Random.Range(maxOnnTime * 0.5f, maxOnnTime * 1.5f);

            yield return Flicker(!lightComponent.enabled);

            yield return new WaitForSeconds(counter);
        }
    }

    IEnumerator Flicker(bool state)
    {
        float flickerCounter = this.counter * 0.1f;
        float counter = 0;

        while(counter < flickerCounter)
        {
            lightComponent.enabled = !lightComponent.enabled;

            float interval = Random.Range(flickerCounter / this.flickerCounter * 0.5f, flickerCounter / this.flickerCounter);
            counter += interval;

            yield return new WaitForSeconds(interval);
        }

        lightComponent.enabled = state;
    }
}
