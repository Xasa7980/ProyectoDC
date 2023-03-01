using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum InitializeInstruction { NewLevel, LoadLevel }

    public static InitializeInstruction initializeInstruction = InitializeInstruction.NewLevel;

    public static LevelManager current { get; private set; }

    List<GameObject> destroyOnRespawn = new List<GameObject>();

    [SerializeField] GameObject gameManager;
    RoomEntry roomEntry;
    GameObject gameManagerInstance;

    PlayerMovement player;
    CameraController gameCamera;

    DungeonController dungeon;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        dungeon = FindObjectOfType<DungeonController>();

        if (initializeInstruction == InitializeInstruction.NewLevel)
            dungeon.Init();
        else
            dungeon.Load();
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
        foreach(GameObject go in destroyOnRespawn)
        {
            Destroy(go);
        }
        destroyOnRespawn.Clear();

        FindObjectOfType<Player_UI_Manager>().OnRepawn();
        Destroy(gameManagerInstance);
        SpawnPlayer();

        TargetPointer pointer = FindObjectOfType<TargetPointer>(true);
        pointer.Disable();
    }

    public void DestroyObjectAtRespawn(GameObject gameObject)
    {
        destroyOnRespawn.Add(gameObject);
    }

    public void Save()
    {
        dungeon.Save();
    }

    public void Load()
    {
        dungeon.Load();
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
