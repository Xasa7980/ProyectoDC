using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    [SerializeField] Color color = Color.red;
    //[SerializeField] int width = 1;
    //[SerializeField] int height = 1;
    //[SerializeField] float gridSize;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        float left = transform.position.x - (16 / 2f * 5) + 5f/2;
        float bottom = transform.position.z - (16 / 2f * 5) + 5f/2;
        Vector3 bottomLeft = new Vector3 (left, 0, bottom);

        for(int x =0; x < 16; x++)
        {
            for( int y =0; y < 16; y++)
            {
                Vector3 position = bottomLeft + new Vector3(x * 5f, 0, y * 5f);
                Gizmos.DrawWireCube(position, new Vector3(1, 0, 1) * 5f);
            }
        }
    }
}
