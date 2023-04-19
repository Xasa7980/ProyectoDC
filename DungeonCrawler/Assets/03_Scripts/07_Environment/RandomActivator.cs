using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActivator : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float prob = 0.5f;

    private void Start()
    {
        if (Random.value <= prob)
            gameObject.SetActive(false);
    }
}
