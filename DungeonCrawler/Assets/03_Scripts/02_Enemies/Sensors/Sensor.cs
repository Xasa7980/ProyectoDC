using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract bool ThreatsDetected();

    public abstract Transform GetNearestThreat();

    public abstract bool InRange(Vector3 position);
}
