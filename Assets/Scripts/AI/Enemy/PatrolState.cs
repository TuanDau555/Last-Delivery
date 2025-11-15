using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyBaseState
{
    #region Parameter
    private const string TAG = "Waypoint";

    private NavMeshAgent _navMeshAgent;
    private ConnectedWayPoint _currentWaypoint, _previousWaypoint;
    private Transform patrolArea;
    private EnemyStatsSO _statsSO;

    private bool _isTraveling;
    private bool _isWaiting;
    private float _waitTimer;
    private int _waypointVisited;
    private readonly Vector3 startPoint;
    public PatrolState(Enemy enemy, Animator animator, NavMeshAgent agent, EnemyStatsSO statsSO, Transform patrolRegion) : base(enemy, animator)
    {
        this.startPoint = enemy.transform.position;
        this._navMeshAgent = agent;
        this._statsSO = statsSO;
        this.patrolArea = patrolRegion;
    }
    #endregion

    #region Execute
    public override void OnEnter()
    {
        animator.CrossFade(PatrolHash, crossFadeDuration);
        Debug.Log("Enemy is patrol");
        InitializeAgent();

        // FindDestination();
    }

    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
    }


    public override void Update()
    {
        UpdateDestination();
    }

    #endregion

    private void InitializeAgent()
    {
        if (_navMeshAgent == null) return;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _statsSO.stats.walkSpeed;
        GetRandomWaypoint();
        
        FindDestination();
    }

    #region Waypoints
    private void GetRandomWaypoint()
    {
        if (_currentWaypoint == null)
        {
            // Grab all waypoint in the Scene
            GameObject[] waypoints;

            if (patrolArea != null)
            {
                waypoints = patrolArea
                    .GetComponentsInChildren<ConnectedWayPoint>()
                    .Select(wp => wp.gameObject)
                    .ToArray();
            }
            else
            {
                // fall back if forget to assign in Inspector
                waypoints = GameObject.FindGameObjectsWithTag(TAG);
            }
            
            if (waypoints.Length > 0)
            {
                while (_currentWaypoint == null)
                {
                    // Select random waypoint
                    int random = Random.Range(0, waypoints.Length);

                    // Grab random one and make a start point
                    ConnectedWayPoint startingPoint = waypoints[random].GetComponent<ConnectedWayPoint>();

                    // Now let enemy now the target point
                    if (startingPoint != null)
                    {
                        _currentWaypoint = startingPoint;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Failed to find waypoint in the scene");
            }
        }
    }
    #endregion

    #region Destination
    private void UpdateDestination()
    {
        if (_isTraveling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _isTraveling = false;
            _waypointVisited++;

            if (_statsSO.stats.patrolWaiting) // pass from Stats SO
            {
                animator.CrossFade(IdleHash, crossFadeDuration);
                
                _isWaiting = true; // go to wait condition logic...
                _waitTimer = 0f; // reset the wait count
            }
            else
            {
                FindDestination();
            }

        }
        if (_isWaiting)
        {
            //...increase the wait timer count
            _waitTimer += Time.deltaTime;

            // if over the max wait time
            if (_waitTimer >= _statsSO.stats.waitTime)
            {
                _isWaiting = false;
                animator.CrossFade(PatrolHash, crossFadeDuration);
                FindDestination();
            }
        }
    }

    private void FindDestination()
    {
        // Check if Enemy visited atleast one waypoint
        if (_waypointVisited > 0)
        {
            // find the next waypoint
            ConnectedWayPoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);

            // So the current waypoint is now the previous waypoint 
            _previousWaypoint = _currentWaypoint;

            // And the new current waypoint is the next waypoint
            _currentWaypoint = nextWaypoint;
        }

        // Now we have the destination, let the agent know it
        Vector3 targetPoint = _currentWaypoint.transform.position;
        _navMeshAgent.SetDestination(targetPoint);
        _isTraveling = true;

    }
    #endregion
}
