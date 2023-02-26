using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current { get; private set; }

    [SerializeField] GameObject gameManager;
    RoomEntry roomEntry;
    GameObject gameManagerInstance;

    PlayerMovement player;
    CameraController gameCamera;

    private void Awake()
    {
        current = this;
    }

    public void SpawnPlayer()
    {
        roomEntry = FindObjectOfType<RoomEntry>();
        gameManagerInstance = Instantiate(gameManager, roomEntry.playerSpawnPoint.position, Quaternion.identity);
        player = gameManagerInstance.GetComponentInChildren<PlayerMovement>();
        gameCamera = gameManagerInstance.GetComponentInChildren<CameraController>();
        player.GetComponent<Health>().OnDie += ()=> FindObjectOfType<Dungeon_UI_Manager>().GetComponent<Animator>().SetTrigger("Die");
    }

    public void RespawnPlayer()
    {
        FindObjectOfType<Player_UI_Manager>().OnRepawn();
        Destroy(gameManagerInstance);
        SpawnPlayer();

        TargetPointer pointer = FindObjectOfType<TargetPointer>(true);
        pointer.Disable();
    }
}
