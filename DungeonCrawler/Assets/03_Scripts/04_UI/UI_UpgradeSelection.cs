using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeSelection : MonoBehaviour
{
    [SerializeField] UI_UpgradeLayout layout;
    [SerializeField] float speed = 5;
    UI_UpgradeLayout[] cards;

    private void Start()
    {
        FindObjectOfType<DungeonController>().onRoomClearedUnityEvent.AddListener(() => Init());
        gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        StopAllCoroutines();

    //        if (cards != null)
    //        {
    //            foreach (UI_UpgradeLayout layout in cards)
    //            {
    //                if (layout == null) continue;

    //                Destroy(layout.gameObject);
    //            }
    //        }

    //        Init();
    //    }
    //}

    public void Init()
    {
        gameObject.SetActive(true);
        Player_UI_Manager manager = FindObjectOfType<Player_UI_Manager>();
        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        cards = new UI_UpgradeLayout[3];
        List<Upgrade> allUpgrades = new List<Upgrade>(Resources.LoadAll<Upgrade>(""));

        for(int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, allUpgrades.Count);
            Upgrade upgrade = allUpgrades[index];
            allUpgrades.RemoveAt(index);
            cards[i] = layout.CreateInstance(upgrade, this.transform);
            cards[i].gameObject.SetActive(false);
            int buttonIndex = i;
            cards[i].cardButton.onClick.AddListener(() => SelectUpgrade(buttonIndex));
            cards[i].cardButton.onClick.AddListener(() => manager.Show());
            cards[i].cardButton.onClick.AddListener(() => player.SetPlayerLockedState(false));
        }

        StopAllCoroutines();
        StartCoroutine(Show());
    }

    public void SelectUpgrade(int cardIndex)
    {
        StartCoroutine(SelectCardRoutine(cardIndex));
    }

    IEnumerator Show()
    {
        float percent = 0;
        Vector3[] positions = new Vector3[3];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = -Vector3.right * 450 * (i - 1);
            cards[i].gameObject.SetActive(true);
        }

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            for(int i = 0; i < cards.Length; i++)
            {
                UI_UpgradeLayout card = cards[i];

                //card.transform.localPosition = Vector3.Lerp(Vector3.right * 2000, positions[i], percent);
                card.transform.localPosition = Vector3.Lerp(positions[i] * 1.5f, positions[i], percent);
                card.transform.localScale = Vector3.one * Mathf.Lerp(1.5f, 1, percent);
                foreach(Image image in card.GetComponentsInChildren<Image>())
                {
                    Color c = image.color;
                    c.a = Mathf.Lerp(0, 1, percent);
                    image.color = c;
                }
            }

            yield return null;
        }
    }

    IEnumerator Hide(int selected)
    {
        float percent = 0;
        Vector3[] positions = new Vector3[3];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = -Vector3.right * 450 * (i - 1);
        }

        while (percent < 1)
        {
            percent += Time.deltaTime * speed * 2;

            for (int i = 0; i < cards.Length; i++)
            {
                UI_UpgradeLayout card = cards[i];

                if (i != selected)
                {
                    //card.transform.localPosition = Vector3.Lerp(positions[i], Vector3.right * 2000, percent);

                    card.transform.localPosition = Vector3.Lerp(positions[i], positions[i] * 0.5f, percent);
                    card.transform.localScale = Vector3.one * Mathf.Lerp(1, 0.5f, percent);
                    foreach (Image image in card.GetComponentsInChildren<Image>())
                    {
                        Color c = image.color;
                        c.a = Mathf.Lerp(1, 0, percent);
                        image.color = c;
                    }
                }
                //else
                //    card.transform.localPosition = Vector3.Lerp(positions[i], Vector3.zero, percent);
            }

            yield return null;
        }

        for(int i=0;i<cards.Length; i++)
        {
            Destroy(cards[i].gameObject);
        }

        gameObject.SetActive(false);
    }

    IEnumerator SelectCardRoutine(int selected)
    {
        float percent = 0;
        Image[] images = cards[selected].GetComponentsInChildren<Image>();

        while(percent < 1)
        {
            percent += Time.deltaTime * speed * 0.75f;

            cards[selected].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2, percent);
            foreach(Image image in images)
            {
                Color c = image.color;
                c.a = 1 - percent;
                image.color = c;
            }

            yield return null;
        }

        Upgrade upgrade = cards[selected].upgrade;
        FindObjectOfType<PlayerUpgrades>().AddUpgradeDelayed(upgrade, 1f);

        StartCoroutine(Hide(selected));
    }
}
