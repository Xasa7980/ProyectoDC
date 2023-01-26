using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CardLayout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Card card { get; private set; }

    [SerializeField] Image _mainImage;
    public Image mainImage => _mainImage;
    [SerializeField] Color classColor;
    [SerializeField] Image[] levels;

    [SerializeField] GameObject frontCover;
    [SerializeField] GameObject backCover;

    float percent = 0;

    public void CreateInstance(Card card, Transform parent = null)
    {
        UI_CardLayout instance = Instantiate(this, parent);

        instance.card = card;

        instance._mainImage.sprite = card.mainImage;

        for(int i = 0; i < instance.levels.Length; i++)
        {
            if (i <= card.level - 1)
            {
                instance.levels[i].gameObject.SetActive(true);
            }
            else
            {
                instance.levels[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        Flip(1);
        Debug.Log("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        Flip(-1);
        Debug.Log("Pointer Exit");
    }

    IEnumerator Flip(float dir)
    {
        Debug.Log(percent);

        Image[] frontImages = frontCover.GetComponentsInChildren<Image>();
        Image[] backImages = backCover.GetComponentsInChildren<Image>();

        if (dir > 0)
        {
            backCover.SetActive(true);

            while (percent < 1)
            {
                percent += Time.unscaledDeltaTime * 3 * dir;

                foreach (Image image in frontImages)
                {
                    Color c = image.color;
                    c.a = Mathf.Lerp(1, 0, percent);
                    image.color = c;
                }

                foreach (Image image in backImages)
                {
                    Color c = image.color;
                    c.a = Mathf.Lerp(0, 1, percent);
                    image.color = c;
                }

                yield return null;
            }

            frontCover.SetActive(false);

            percent = 1;
        }
        else if (dir < 0)
        {
            frontCover.SetActive(true);

            while (percent > 0)
            {
                percent += Time.unscaledDeltaTime * 3 * dir;

                foreach (Image image in frontImages)
                {
                    Color c = image.color;
                    c.a = Mathf.Lerp(1, 0, percent);
                    image.color = c;
                }

                foreach (Image image in backImages)
                {
                    Color c = image.color;
                    c.a = Mathf.Lerp(0, 1, percent);
                    image.color = c;
                }

                yield return null;
            }

            backCover.SetActive(false);

            percent = 0;
        }
    }
}
