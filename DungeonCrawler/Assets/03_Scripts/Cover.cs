using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] Transform[] coverSpots;

    public Transform[] GetValidCoverSpotsForPosition(Vector3 targetPosition)
    {
        List<Transform> validSpots = new List<Transform>();

        //Determinar que punto esta detras de la cobertura y de frente al objectivo
        foreach (Transform t in coverSpots)
        {
            Vector3 tDirection = (targetPosition - t.position).normalized;
            if (Vector3.Dot(t.forward, tDirection) > 0)
                validSpots.Add(t);
        }

        return validSpots.ToArray();
    }

    void OnDrawGizmos()
    {
        foreach(Transform t in coverSpots)
        {
            if (t == null) continue;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(t.position, 0.2f);
        }
    }
}
