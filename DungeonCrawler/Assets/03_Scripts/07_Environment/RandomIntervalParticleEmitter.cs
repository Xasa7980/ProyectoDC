using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIntervalParticleEmitter : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] Light emitLight;

    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    [SerializeField] float minLightInterval;
    [SerializeField] float maxLightInterval;

    private void Start()
    {
        StartCoroutine(Emit());
    }

    IEnumerator Emit()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            particles.Play();
            StartCoroutine(TurnOffLight());
        }
    }

    IEnumerator TurnOffLight()
    {
        emitLight.enabled = true;

        yield return new WaitForSeconds(Random.Range(minLightInterval, maxLightInterval));

        emitLight.enabled = false;
    }
}
