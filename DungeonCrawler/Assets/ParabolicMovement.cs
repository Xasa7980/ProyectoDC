using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMovement : MonoBehaviour
{
    [SerializeField] float throwForce;
    [SerializeField] float velocity;
    [SerializeField] float fireAngle;
    public FixedJoystick joystick;
    public Vector3 startPoint;
    public Vector3 controlPoint;
    public Vector3 endPoint;

    public LayerMask mask;
    public int curveResolution = 20;
    [SerializeField] float moveSpeed;
    [SerializeField] float radius;
    [SerializeField] bool detectedGround;
    public float timer;

    private void Start()
    {
        Transform player = FindObjectOfType<PlayerMovement>().transform;
        joystick = player.GetComponent<PlayerMovement>().moveJoystick;
        startPoint = player.transform.forward + player.transform.right + (Vector3)joystick.Direction;
        controlPoint = player.transform.right + player.transform.forward * velocity + (Vector3)joystick.Direction * throwForce * 0.5f + Vector3.up * fireAngle;
        endPoint = (player.transform.forward + player.transform.right) * velocity + (Vector3)joystick.Direction * throwForce ;
        endPoint.y = 1;
    }
    private void Update()
    {
        detectedGround = Physics.CheckSphere(transform.position, radius, mask);
        if(!detectedGround)
        {
            BallMove(startPoint, controlPoint, endPoint);
        }
        else GetComponentInChildren<GrenadeSkill>().enabled = true;
    }
    private void BallMove(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        timer += Time.deltaTime;

        Vector3 bezierPosition;
        if (detectedGround) bezierPosition = transform.position;
        else
        {
            bezierPosition = Mathf.Pow(1f - timer, 2f) * startPoint +
                  2f * (1f - timer) * timer * controlPoint +
                  Mathf.Pow(timer, 2f) * endPoint;
        }
        transform.position = Vector3.Lerp(transform.position, bezierPosition, moveSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}