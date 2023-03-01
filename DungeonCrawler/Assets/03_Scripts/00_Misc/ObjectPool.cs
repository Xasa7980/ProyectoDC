using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 10;
    public List<GameObject> pooledObjects;
    private void Start()
    {
        AddObjectToPool(poolSize);
    }
    void AddObjectToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (prefab != null)
            {
                GameObject poolObject = Instantiate(prefab);
                poolObject.SetActive(false);
                pooledObjects.Add(poolObject);
                poolObject.transform.SetParent(gameObject.transform);
            }
        }
    }
    public GameObject RequestObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeSelf)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        AddObjectToPool(1);
        pooledObjects[pooledObjects.Count - 1].SetActive(true);
        return pooledObjects[pooledObjects.Count - 1];
    }

}
