using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float lookSpeed;

    NavMeshAgent agent;
    [SerializeField] bool _updatePosition = true;
    [SerializeField] bool _updateRotation = true;

    public float remainingDistance => agent.remainingDistance;
    public Vector3 desiredVelocity => agent.desiredVelocity;
    public Vector3 nextPosition { get=>agent.nextPosition; set => agent.nextPosition = value;}

    public bool updateRotation => _updateRotation;
    public bool updatePosition => _updatePosition;

    public bool overrideController { get; set; }

    public Vector3 destination { get; private set; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = updatePosition;
        agent.updateRotation = updateRotation;
    }

    private void Update()
    {
        if (overrideController) return;

        if (!updateRotation && agent.desiredVelocity != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(agent.desiredVelocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lookSpeed * Time.deltaTime);
        }

        if (!updatePosition)
        {
            agent.nextPosition = transform.position;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        float sqrDst = (this.destination - destination).sqrMagnitude;
        if (sqrDst >= 0.25f * 0.25f)
        {
            agent.SetDestination(destination);
            this.destination = destination;
        }
    }
}
