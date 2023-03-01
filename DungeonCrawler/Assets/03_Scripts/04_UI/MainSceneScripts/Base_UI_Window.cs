using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base_UI_Window : MonoBehaviour
{
    [SerializeField] float speed;

    ImageInfo[] images;
    TextInfo[] texts;
    RawInfo[] raws;

    bool initialized = false;

    public bool active { get; private set; }

    float percent;

    public void Init()
    {
        Image[] images = GetComponentsInChildren<Image>();
        this.images = new ImageInfo[images.Length];
        for(int i = 0; i < images.Length; i++)
        {
            this.images[i] = new ImageInfo(images[i]);
        }

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        this.texts = new TextInfo[texts.Length];
        for(int i = 0;i < texts.Length; i++)
        {
            this.texts[i] = new TextInfo(texts[i]);
        }

        RawImage[] raws = GetComponentsInChildren<RawImage>();
        this.raws = new RawInfo[raws.Length];
        for(int i = 0; i < raws.Length; i++)
        {
            this.raws[i] = new RawInfo(raws[i]);
        }

        active = gameObject.activeSelf;
        initialized = true;
    }

    public void Show()
    {
        if (active) return;

        if (!initialized) Init();

        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
        active = true;
    }

    public void Hide()
    {
        if(!active) return;

        if (!initialized) Init();

        StopAllCoroutines();
        StartCoroutine(HideRoutine());
        active = false;
    }

    IEnumerator ShowRoutine()
    {
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            percent = Mathf.Clamp01(percent);

            foreach (ImageInfo image in images)
            {
                image.Update(percent);
            }

            foreach (TextInfo text in texts)
            {
                text.Update(percent);
            }

            foreach(RawInfo raw in raws)
            {
                raw.Update(percent);
            }

            yield return null;
        }
    }

    IEnumerator HideRoutine()
    {
        while (percent > 0)
        {
            percent -= Time.deltaTime * speed;

            percent = Mathf.Clamp01(percent);

            foreach (ImageInfo image in images)
            {
                image.Update(percent);
            }

            foreach (TextInfo text in texts)
            {
                text.Update(percent);
            }

            foreach (RawInfo raw in raws)
            {
                raw.Update(percent);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

    struct ImageInfo
    {
        readonly Image image;
        readonly float maxAlpha;

        public ImageInfo(Image image)
        {
            this.image = image;
            this.maxAlpha = image.color.a;
        }

        public void Update(float percent)
        {
            Color color = image.color;
            color.a = Mathf.Lerp(0, maxAlpha, percent);
            image.color = color;
        }
    }

    struct TextInfo
    {
        readonly TextMeshProUGUI text;
        readonly float maxAlpha;

        public TextInfo(TextMeshProUGUI text)
        {
            this.text = text;
            this.maxAlpha = text.color.a;
        }

        public void Update(float percent)
        {
            Color color = text.color;
            color.a = Mathf.Lerp(0, maxAlpha, percent);
            text.color = color;
        }
    }

    struct RawInfo
    {
        readonly RawImage image;
        readonly float maxAlpha;

        public RawInfo(RawImage image)
        {
            this.image = image;
            this.maxAlpha = image.color.a;
        }

        public void Update(float percent)
        {
            Color color = image.color;
            color.a = Mathf.Lerp(0, maxAlpha, percent);
            image.color = color;
        }
    }
}
