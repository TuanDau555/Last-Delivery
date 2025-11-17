using System.Collections;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private Transform playerRef;

    private float _lostSightTimer;
    private float fovCheckInterval = 0.2f;
    [SerializeField] private Collider[] rangeChecks;

    public float _viewRadius { private get; set; }
    public float _viewAngle { private get; set; }
    public float _attackRange { private get; set; }
    public bool canSeePlayer { get; private set; }
    public bool inAttackRange { get; private set; }
    public Vector3 directionToTarget { get; private set; }

    void Start()
    {
        // Init Enemy Stats
        _viewRadius = enemyStatsSO.stats.radius;
        _viewAngle = enemyStatsSO.stats.angle;
        _attackRange = enemyStatsSO.stats.attackDistance;

        StartCoroutine(FOVRoutine());
        StartCoroutine(FindPlayerRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(fovCheckInterval);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck(fovCheckInterval);
        }
    }

    private IEnumerator FindPlayerRoutine()
    {
        while (playerRef == null)
        {
            playerRef = FindObjectOfType<PlayerController>()?.transform;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void FieldOfViewCheck(float delta)
    {

        // Reset flag this tick; will be set true again if we find valid sight
        bool sawTargetThisCheck = false;
        inAttackRange = false;

        // Get all targets in view radius (It just player by the way XD)
        rangeChecks = Physics.OverlapSphere(transform.position, _viewRadius, enemyStatsSO.stats.targetMask);

        // if found something in the range
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;

            directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            bool isBlocked = Physics.Raycast(transform.position, directionToTarget, distanceToTarget, enemyStatsSO.stats.obstructionMask);

            // Attack if in range
            if (distanceToTarget <= _attackRange && !isBlocked)
            {
                canSeePlayer = true;
                inAttackRange = true;
            }

            // Check if the target is in the angle of view
            // We dived angle by 2 because we want to get half of the angle to check both side
            if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2)
            {

                // if no obstruction infront of the enemy
                if (!isBlocked)
                {
                    sawTargetThisCheck = true;
                    _lostSightTimer = 0f; // reset the timer
                    if (!canSeePlayer)
                    {
                        canSeePlayer = true;
                        Debug.Log($"{gameObject.name} gained sight of {playerRef.name}");
                    }
                }
            }
        }

        if (!sawTargetThisCheck && !inAttackRange)
        {
            _lostSightTimer += delta;

            if (_lostSightTimer >= enemyStatsSO.stats.lostSightDelay)
            {
                if (canSeePlayer)
                {
                    canSeePlayer = false;
                    Debug.Log($"{playerRef.name} has been lost by {gameObject.name} (timeout {_lostSightTimer:F2}s).");
                }
            }
        }
        // NOTE: canSeePlayer only can set true if see player; only set false when timer it larger than threshold
    }

    #region Editor
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _viewRadius);
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _attackRange);
        
        Vector3 viewAngleA = DirFromAngle(transform.eulerAngles.y, -_viewAngle / 2);
        Vector3 viewAngleB = DirFromAngle(transform.eulerAngles.y, _viewAngle / 2);

        Handles.DrawLine(transform.position, transform.position + viewAngleA * _viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewAngleB * _viewRadius);

        if(canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(transform.position, playerRef.transform.position);
        }
    }

    private Vector3 DirFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    } 
    #endregion
}
