using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Manager : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] float speed = 5;
    [SerializeField] Animator alarmVigentteAnim;

    private void Start()
    {
        DungeonController controller = FindObjectOfType<DungeonController>();
        controller.onRoomClearedUnityEvent.AddListener(InitOnRoomClearedUnityEvent);
        controller.onPlayerDetected += InitOnPlayerDetected;
    }

    public void OnRepawn()
    {
        DungeonController controller = FindObjectOfType<DungeonController>();
        controller.onRoomClearedUnityEvent.RemoveListener(InitOnRoomClearedUnityEvent);
        controller.onPlayerDetected -= InitOnPlayerDetected;
    }

    void InitOnRoomClearedUnityEvent()
    {
        Hide();
        alarmVigentteAnim.SetBool("Active", false);
        //controller.onPlayerDetected += () => alarmVigentteAnim.SetBool("Active", true);
    }

    void InitOnPlayerDetected()
    {
        alarmVigentteAnim.SetBool("Active", true);
    }

    public void Show()
    {
        StartCoroutine(ShowRoutine());
    }

    public void Hide()
    {
        StartCoroutine(HideRoutine());
    }

    IEnumerator ShowRoutine()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            container.localScale = Vector3.Lerp(Vector3.one * 2, Vector3.one, percent);

            yield return null;
        }

        container.localScale = Vector3.one;
    }

    IEnumerator HideRoutine()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            container.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2, percent);

            yield return null;
        }

        container.localScale = Vector3.one * 2;
    }
}
