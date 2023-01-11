using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorBehaviour : MonoBehaviour
{
    [SerializeField] Light sightField;
    [SerializeField] Sensor sensor;


    float defaultAngle;
    float defaultInnerAngle;


    private void Start()
    {
        defaultAngle = sightField.spotAngle;
        defaultInnerAngle = sightField.innerSpotAngle;
    }

    void Update()
    {
        if (sensor.ThreatsDetected())
        {
            sightField.spotAngle = Mathf.Lerp(sightField.spotAngle, 15, Time.deltaTime);
            sightField.innerSpotAngle = Mathf.Lerp(sightField.innerSpotAngle, 15, Time.deltaTime);
            sightField.color = Color.red;
        }
        else
        {
            sightField.spotAngle = Mathf.Lerp(sightField.spotAngle, defaultAngle, Time.deltaTime);
            sightField.innerSpotAngle = Mathf.Lerp(sightField.innerSpotAngle, defaultInnerAngle, Time.deltaTime);
            sightField.color = Color.white;
        }
    }
}
