using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    [SerializeField] Vector3 strength = Vector3.one;
    [SerializeField] float speed = 1;
    float interval => 1f / speed;
    float counter = 0;
    Vector3 center;
    Vector3 offset;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter <= 0) {
            offset = Random.insideUnitSphere;
            counter = interval;
            target = new Vector3(center.x + offset.x * strength.x, center.y + offset.y * strength.y, center.z + offset.z * strength.z);
        }

        counter -= Time.deltaTime;

        float percent = (interval - counter) / interval;

        transform.position = Vector3.Lerp(transform.position, target, percent * Time.deltaTime);
    }
}
