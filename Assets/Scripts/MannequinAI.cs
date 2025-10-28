using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class MannequinAI : MonoBehaviour
{
    public Transform player;
    public float wanderRadius = 8f;
    public float wanderInterval = 3f;
    public float fieldOfViewAngle = 60f;
    public float stopDuration = 5f;

    private NavMeshAgent agent;
    //private float timer;
    private bool isVisible;
    private float stopTimer;
    private bool stopped;
    private bool wasVisibleLastFrame;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChooseNewDestination();
    }

    void Update()
    {
        if (player == null) return;

        isVisible = IsVisibleToPlayer();

        if (isVisible)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.updatePosition = false;
            agent.updateRotation = false;
            wasVisibleLastFrame = true;
            return;
        }

        if (wasVisibleLastFrame && isVisible)
        {
            StartPause();
            wasVisibleLastFrame = false;
        }

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        if (stopped)
        {
            stopTimer -= Time.deltaTime;
            agent.velocity = Vector3.zero;
            if (stopTimer <= 0f)
            {
                stopped = false;
                ChooseNewDestination();
            }
            return;
        }


        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!stopped)
            {
                stopped = true;
                stopTimer = stopDuration;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
            }
        }
    }

    void StartPause()
    {
        stopped = true;
        stopTimer = stopDuration;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    void ChooseNewDestination()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
        agent.SetDestination(newPos);
    }

    bool IsVisibleToPlayer()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, dir);

        if (angle < fieldOfViewAngle)
        {
            if (Physics.Raycast(player.position, dir, out RaycastHit hit, 100f))
                return hit.transform == transform;
        }
        return false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, dist, NavMesh.AllAreas))
            return hit.position;
        return origin;
    }
}
