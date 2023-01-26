using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_CardSelection : MonoBehaviour
{
    [SerializeField] UI_CardLayout layout;
    [SerializeField] float speed = 5;
    UI_CardLayout[] cards;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StopAllCoroutines();
            StartCoroutine(Show());
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            StopAllCoroutines();
            StartCoroutine(Hide(0));
            StartCoroutine(SelectCard(0));
        }
    }

    IEnumerator Show()
    {
        float percent = 0;
        cards = GetComponentsInChildren<UI_CardLayout>(true);
        Vector3[] positions = new Vector3[3];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = -Vector3.right * 650 * (i - 1);
            cards[i].gameObject.SetActive(true);
        }

        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * speed;

            for(int i = 0; i < cards.Length; i++)
            {
                UI_CardLayout card = cards[i];

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
        cards = GetComponentsInChildren<UI_CardLayout>(true);
        Vector3[] positions = new Vector3[3];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = -Vector3.right * 650 * (i - 1);
        }

        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * speed * 2;

            for (int i = 0; i < cards.Length; i++)
            {
                UI_CardLayout card = cards[i];

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
            if (i == selected) continue;

            cards[i].gameObject.SetActive(false);
        }
    }

    IEnumerator SelectCard(int selected)
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.unscaledDeltaTime * speed * 0.75f;

            UI_CardLayout card = cards[selected];

            card.transform.localPosition = Vector3.Lerp(-Vector3.right * (selected - 1) * 650, Vector3.zero, percent);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        percent = 0;

        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * speed * 0.5f;

            UI_CardLayout card = cards[selected];

            card.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down * 300, percent);
            card.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.2f, percent);
            foreach (Image image in card.GetComponentsInChildren<Image>())
            {
                Color c = image.color;
                c.a = Mathf.Lerp(1, 0, percent);
                image.color = c;
            }

            yield return null;
        }
    }
}
