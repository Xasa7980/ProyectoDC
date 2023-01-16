using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePresetManager : MonoBehaviour
{
    public bool alowdPropsInstancing = true;
    [SerializeField] GameObject[] floorTilesVariants;

    [SerializeField] Vector3 tileSize = Vector3.one;

    [SerializeField, Min(0)] float floorCellSize;
    [SerializeField] GameObject[] props;
    [SerializeField] PlaceHolder[] holders;

    public bool empty { get; private set; }

    [System.Serializable]
    struct PlaceHolder
    {
        [SerializeField] Transform container;
        [SerializeField] GameObject[] objects;

        public void Place()
        {
            Instantiate(objects[Random.Range(0, objects.Length)], container.position, container.rotation, container);
        }
    }

    public void Init()
    {
        GameObject tile = Instantiate(floorTilesVariants[Random.Range(0, floorTilesVariants.Length)], this.transform);
        tile.transform.localPosition = new Vector3(2.5f, 0, -2.5f);
        empty = true;
    }

    public void Populate(float spawnProbability)
    {
        if (!alowdPropsInstancing) return;

        if (floorCellSize > 0 && Random.value <= spawnProbability)
        {
            int cellX = Mathf.FloorToInt(tileSize.x / floorCellSize);
            int cellY = Mathf.FloorToInt(tileSize.z / floorCellSize);

            Vector3 bottomLeftCorner = transform.position - transform.forward * (cellX / 2f * floorCellSize) - transform.right * (cellY / 2f * floorCellSize);
            for (int x = 0; x < cellX; x++)
            {
                for (int y = 0; y < cellY; y++)
                {
                    if (Random.value <= spawnProbability)
                    {
                        Vector3 position = bottomLeftCorner + new Vector3(x * floorCellSize + floorCellSize/2, 0, y * floorCellSize + floorCellSize / 2);
                        GameObject prop = Instantiate(props[Random.Range(0, props.Length)], position, Quaternion.Euler(Vector3.up * Random.Range(0, 3) * 90));
                        prop.transform.parent = this.transform;
                        empty = false;
                    }
                }
            }
        }
        else
        {
            if (holders.Length > 0)
            {
                holders[Random.Range(0, holders.Length)].Place();
                empty = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.up * tileSize.y / 2, tileSize);
        Gizmos.DrawCube(transform.position, new Vector3(tileSize.x, 0, tileSize.z));

        if (floorCellSize > 0)
        {
            int cellX = Mathf.FloorToInt(tileSize.x / floorCellSize);
            int cellY = Mathf.FloorToInt(tileSize.z / floorCellSize);

            Gizmos.color = Color.green;
            Vector3 bottomLeftCorner = new Vector3(-cellX / 2f * floorCellSize + floorCellSize / 2, 0, -cellY / 2f * floorCellSize + floorCellSize / 2);
            for (int x = 0; x < cellX; x++)
            {
                for (int y = 0; y < cellY; y++)
                {
                    Vector3 position = transform.position + bottomLeftCorner + new Vector3(x * floorCellSize, 0, y * floorCellSize);
                    Gizmos.DrawSphere(position, 0.1f);
                }
            }
        }
    }
}
