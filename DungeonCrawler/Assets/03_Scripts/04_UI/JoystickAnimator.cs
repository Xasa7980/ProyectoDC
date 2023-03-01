using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAnimator : MonoBehaviour
{
    [SerializeField] float speed = 5;

    [SerializeField] float minRadius = 3;
    [SerializeField] float maxRadius = 6;

    float alpha = 0;
    float scale = 0;
    [SerializeField] float maxScale = 1;

    [SerializeField] Image interactionArea;
    float interactionAreaAlpha = 1;
    float interactionAreaScale = 1;
    float interactionAreaBaseAlpha;

    [SerializeField] AnimationCurve influenceCurve;
    [SerializeField] float influenceRange = 90;
    [SerializeField] FixedJoystick handle;
    [SerializeField] Transform ringContainer;
    [SerializeField] Image[] ring;

    private void Start()
    {
        interactionAreaBaseAlpha = interactionArea.color.a;
        interactionAreaAlpha = interactionAreaBaseAlpha;
    }

    private void Update()
    {
        Vector2 direction = -handle.Direction;
        //float angleRad = Mathf.Atan2(direction.y, direction.x) * 2;
        //float angleDeg = (360 + Mathf.Atan2(direction.x, -direction.y) * (180 / Mathf.PI)) % 360;

        if (direction != Vector2.zero)
        {
            float radDiff = maxRadius - minRadius;

            for (int i = 0; i < ring.Length; i++)
            {
                //int diff = Mathf.Abs(i - ringIndex);

                float angle = Vector2.Angle(ring[i].transform.parent.up, -direction);
                float influence = influenceCurve.Evaluate(1 - Mathf.Clamp01(angle / (influenceRange)/2));
                float displacement = radDiff * influence;
                Vector2 position = Vector2.up * (minRadius + displacement);
                ring[i].transform.localPosition = Vector2.MoveTowards(ring[i].transform.localPosition, position, speed * Time.deltaTime);

                alpha = Mathf.MoveTowards(alpha, 1, Time.deltaTime);
                Color color = ring[i].color;
                color.a = alpha;
                ring[i].color = color;
            }

            scale = Mathf.MoveTowards(scale, maxScale, Time.deltaTime * 5);
            ringContainer.localScale = Vector3.one * scale;

            interactionAreaAlpha = Mathf.MoveTowards(interactionAreaAlpha, 0, Time.deltaTime * 5);
            Color iColor = interactionArea.color;
            iColor.a = interactionAreaAlpha;
            interactionArea.color = iColor;
            interactionAreaScale = Mathf.MoveTowards(interactionAreaScale, 0, Time.deltaTime * 5);
            interactionArea.transform.localScale = Vector3.one * interactionAreaScale;
        }
        else
        {
            for (int i = 0; i < ring.Length; i++)
            {
                ring[i].transform.localPosition = Vector2.MoveTowards(ring[i].transform.localPosition, Vector2.up * minRadius, speed * Time.deltaTime);

                alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime);
                Color color = ring[i].color;
                color.a = alpha;
                ring[i].color = color;
            }

            scale = Mathf.MoveTowards(scale, 0, Time.deltaTime);
            ringContainer.localScale = Vector3.one * scale;

            interactionAreaAlpha = Mathf.MoveTowards(interactionAreaAlpha, interactionAreaBaseAlpha, Time.deltaTime * 5);
            Color iColor = interactionArea.color;
            iColor.a = interactionAreaAlpha;
            interactionArea.color = iColor;
            interactionAreaScale = Mathf.MoveTowards(interactionAreaScale, 1, Time.deltaTime * 5);
            interactionArea.transform.localScale = Vector3.one * interactionAreaScale;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, minRadius);
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, maxRadius);
#endif
    }
}
