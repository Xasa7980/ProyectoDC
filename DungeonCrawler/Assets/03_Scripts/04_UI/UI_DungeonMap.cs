using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DungeonMap : MonoBehaviour
{
    public bool isOpen => this.gameObject.activeSelf;

    [SerializeField] Image roomImage;
    [SerializeField] Image hallImage;
    [SerializeField] float scale = 7;

    DungeonController dungeon;

    private void Start()
    {
        dungeon = FindObjectOfType<DungeonController>();
        gameObject.SetActive(false);
    }

    public void GenerateMap(DungeonController dungeon)
    {
        foreach(RoomController room in dungeon.rooms)
        {
            if (!room.cleared) continue;

            Image roomImage = Instantiate(this.roomImage, this.transform);
            roomImage.rectTransform.sizeDelta = new Vector2(room.room.width * scale, room.room.length * scale);
            roomImage.rectTransform.anchoredPosition = room.room.center * scale;

            foreach(RoomConnection connection in room.room.connections)
            {
                if (connection == null) continue;

                Vector2 direction = connection.direction;
                Quaternion rotation = Quaternion.FromToRotation(Vector2.up, direction);
                Image hallImage = Instantiate(this.hallImage, this.transform);

                float realLength = 0;
                Vector2 startPoint = Vector2.zero;

                if (connection.direction.x == 0)
                {
                    realLength = connection.length - connection.startRoom.length / 2f - connection.endRoom.length / 2f;
                    startPoint = new Vector2(connection.startRoom.center.x, connection.startRoom.center.y + connection.startRoom.length / 2 * connection.direction.y);
                }

                if (connection.direction.y == 0)
                {
                    realLength = connection.length - connection.startRoom.width / 2f - connection.endRoom.width / 2f;
                    startPoint = new Vector2(connection.startRoom.center.x + connection.startRoom.width / 2 * connection.direction.x, connection.startRoom.center.y);
                }

                realLength = Mathf.Abs(realLength);

                hallImage.rectTransform.sizeDelta = new Vector2(10,(realLength / 2 + 1) * scale);
                hallImage.rectTransform.anchoredPosition = (startPoint - (Vector2)connection.direction * 0.25f) * scale;
                hallImage.rectTransform.localRotation = rotation;
            }
        }
    }

    void ClearMap()
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            Destroy(image.gameObject);
        }
    }

    public void Open()
    {
        GenerateMap(dungeon);
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        ClearMap();
        this.gameObject.SetActive(false);
    }
}
