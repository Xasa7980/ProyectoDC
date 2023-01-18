using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlarmLight_VFX : MonoBehaviour
{
    [SerializeField] Transform lightsContainer;
    Material lightMaterial;
    Light[] lights;

    [SerializeField] float rotationSpeed = 5;
    [SerializeField] float flickerSpeed = 5;
    [SerializeField] float intensity = 5;

    public bool isOn { get; private set; }

    private void Start()
    {
        lights = lightsContainer.GetComponentsInChildren<Light>();
        lightMaterial = GetComponent<Renderer>().material;

        isOn = false;
        lightMaterial.SetColor("_EmissionColor", new Color(0, 0, 0, 0));
        foreach (Light light in lights)
        {
            light.intensity = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn) return;

        lightsContainer.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        float lightIntensity = (Mathf.Sin(Time.time * flickerSpeed) + 1) / 2 * intensity;
        foreach (Light light in lights)
        {
            light.intensity = lightIntensity;
        }
    }

    public void TurnOn()
    {
        isOn = true;
        lightMaterial.SetColor("_EmissionColor", new Color(1, 1, 1, 1));
    }

    public void TurnOff()
    {
        isOn = false;
        lightMaterial.SetColor("_EmissionColor", Color.cyan);
        StartCoroutine(TurnOffLights());
    }

    IEnumerator TurnOffLights()
    {
        float percent = 1;
        while (percent > 0)
        {
            foreach (Light light in lights)
            {
                light.intensity = Mathf.Lerp(light.intensity, 0, percent);
                percent -= Time.deltaTime * 5;
            }

            yield return null;
        }
    }
}
