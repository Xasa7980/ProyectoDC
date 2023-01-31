using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] GameObject targetPointerPrefab;
    [SerializeField] float radius = 120;

    Dictionary<Enemy, GameObject> targetsMap = new Dictionary<Enemy, GameObject>();

    public void Enable(List<Enemy> targets)
    {
        if (gameObject.activeSelf) return;

        gameObject.SetActive(true);

        foreach(Enemy target in targets)
        {
            if (targetsMap.ContainsKey(target)) continue;

            GameObject pointer = Instantiate(targetPointerPrefab, this.transform);
            targetsMap.Add(target, pointer);
        }
    }

    public void Disable()
    {
        if(!gameObject.activeSelf) return;

        gameObject.SetActive(false);
        
        foreach(GameObject pointer in targetsMap.Values)
        {
            Destroy(pointer);
        }

        targetsMap.Clear();
    }

    public void AddTarget(Enemy target)
    {
        if (!targetsMap.ContainsKey(target))
        {
            GameObject pointer = Instantiate(targetPointerPrefab, this.transform);
            targetsMap.Add(target, pointer);
        }
    }

    public void RemoveTarget(Enemy target)
    {
        if (targetsMap.ContainsKey(target))
        {
            Destroy(targetsMap[target]);
            targetsMap.Remove(target);
        }
    }

    private void Update()
    {
        foreach(Enemy target in targetsMap.Keys)
        {
            Vector3 direction = (target.transform.position - CameraController.current.transform.position).normalized;
            direction.y = 0;

            float x = Vector3.Dot(direction, CameraController.current.transform.right);
            float y = Vector3.Dot(direction, CameraController.current.transform.forward);

            Vector2 screenDir = new Vector2(x, y);

            targetsMap[target].transform.localPosition = screenDir * radius;
            targetsMap[target].transform.rotation = Quaternion.FromToRotation(Vector3.up, screenDir);
        }
    }
}
