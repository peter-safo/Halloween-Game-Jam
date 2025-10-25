using UnityEngine;

public class MannequinAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform floor;

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;
    public float fieldOfViewAngle = 60f;
    public float wanderRadius = 5f;
    public float pauseTime = 2f;

    private Vector3 targetPosition;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    private float groundY;

    void Start()
    {
        // Cache the mannequin's starting Y height to lock movement
        groundY = transform.position.y;
        ChooseNewTarget();
    }

    void Update()
    {
        if (player == null) return;

        bool isVisible = IsVisibleToPlayer();

        if (isVisible)
            return; // Freeze when looked at

        // Move toward target
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > 0.3f && !isPaused)
        {
            Vector3 dir = (targetPosition - transform.position).normalized;

            // Move only on the XZ plane
            dir.y = 0;

            transform.position += dir * moveSpeed * Time.deltaTime;

            // Keep mannequin locked to floor height
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        }
        else
        {
            HandlePauseAndRetarget();
        }

        // Keep it within the plane boundaries
        ConstrainToPlane();
    }

    void HandlePauseAndRetarget()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseTimer = pauseTime;
        }
        else
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                ChooseNewTarget();
            }
        }
    }

    void ChooseNewTarget()
    {
        // Pick a random point around current position on XZ plane
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 newTarget = new Vector3(randomCircle.x, 0, randomCircle.y);
        targetPosition = transform.position + newTarget;

        // Clamp to plane if needed
        ConstrainTargetToPlane();
    }

    bool IsVisibleToPlayer()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, direction);

        if (angle < fieldOfViewAngle)
        {
            if (Physics.Raycast(player.position, direction, out RaycastHit hit, 100f))
            {
                return hit.transform == transform;
            }
        }
        return false;
    }

    void ConstrainToPlane()
    {
        if (floor == null) return;

        // Assuming plane is centered at origin and scaled uniformly
        Vector3 planeCenter = floor.position;
        Vector3 planeSize = floor.localScale * 5f; // 1 unit scale = 10x10 plane in Unity

        float halfWidth = planeSize.x / 2f;
        float halfLength = planeSize.z / 2f;

        float clampedX = Mathf.Clamp(transform.position.x, planeCenter.x - halfWidth, planeCenter.x + halfWidth);
        float clampedZ = Mathf.Clamp(transform.position.z, planeCenter.z - halfLength, planeCenter.z + halfLength);

        transform.position = new Vector3(clampedX, groundY, clampedZ);
    }

    void ConstrainTargetToPlane()
    {
        if (floor == null) return;

        Vector3 planeCenter = floor.position;
        Vector3 planeSize = floor.localScale * 5f;

        float halfWidth = planeSize.x / 2f;
        float halfLength = planeSize.z / 2f;

        float clampedX = Mathf.Clamp(targetPosition.x, planeCenter.x - halfWidth, planeCenter.x + halfWidth);
        float clampedZ = Mathf.Clamp(targetPosition.z, planeCenter.z - halfLength, planeCenter.z + halfLength);

        targetPosition = new Vector3(clampedX, groundY, clampedZ);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }
#endif
}