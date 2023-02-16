using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current { get; private set; }

    [SerializeField] GameObject gameManager;
    RoomEntry roomEntry;

    PlayerMovement player;
    CameraController gameCamera;

    private void Awake()
    {
        current = this;
    }

    public void SpawnPlayer()
    {
        roomEntry = FindObjectOfType<RoomEntry>();
        Instantiate(gameManager, roomEntry.playerSpawnPoint.position, Quaternion.identity);
        player = FindObjectOfType<PlayerMovement>();
        gameCamera = FindObjectOfType<CameraController>();
        player.GetComponent<Health>().OnDie += ()=> FindObjectOfType<Dungeon_UI_Manager>().GetComponent<Animator>().SetTrigger("Die");
    }

    public void RespawnPlayer()
    {
        player.transform.position = roomEntry.playerSpawnPoint.position;
        player.transform.rotation = roomEntry.playerSpawnPoint.rotation;

        gameCamera.transform.position = player.transform.position;
    }
}
