using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private Transform playerRef;
    [SerializeField] private bool canSeePlayer;

    [SerializeField] private float _viewRadius;
    [SerializeField] private float _viewAngle;

    void Start()
    {
        _viewRadius = enemyStatsSO.stats.radius;
        _viewAngle = enemyStatsSO.stats.angle;
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        // Get all targets in view radius (It just player by the way XD)
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _viewRadius, enemyStatsSO.stats.targetMask);

        // if found something in the range
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if the target is in the angle of view
            // We dived angle by 2 because we want to get half of the angle to check both side
            if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // if no obstruction infront of the enemy
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, enemyStatsSO.stats.obstructionMask))
                {
                    canSeePlayer = true;
                    Debug.Log($"{gameObject.name} see {playerRef.name}");
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        // make sure Enemy can't see player at Start
        // And when player out of range
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            Debug.Log($"{playerRef.name} has out of {gameObject.name} range");

        }
    }

    #region Editor
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _viewRadius);

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
