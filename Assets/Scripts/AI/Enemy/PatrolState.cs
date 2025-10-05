using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyBaseState
{
    #region Parameter
    private const string TAG = "Waypoint";

    private NavMeshAgent _navMeshAgent;
    private ConnectedWayPoint _currentWaypoint, _previousWaypoint;

    // These two get from Stats
    private bool _enemyWaiting;
    private float _waitTime;

    private bool _isTraveling;
    private bool _isWaiting;
    private float _waitTimer;
    private float _enemySpeed;
    private int _waypointVisited;
    private readonly Vector3 startPoint;
    public PatrolState(Enemy enemy, NavMeshAgent agent, bool enemyWaiting, float waitTime, float enemySpeed) : base(enemy)
    {
        this.startPoint = enemy.transform.position;
        this._navMeshAgent = agent;
        this._enemyWaiting = enemyWaiting;
        this._waitTime = waitTime;
        this._enemySpeed = enemySpeed;
    }
    #endregion

    #region Execute
    public override void OnEnter()
    {
        Debug.Log("Enemy is patrol");
        InitializeAgent();
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
        _navMeshAgent.speed = _enemySpeed;
        GetRandomWaypoint();
        
        FindDestination();
    }

    #region Waypoints
    private void GetRandomWaypoint()
    {
        if (_currentWaypoint == null)
        {
            // Grab all waypoint in the Scene
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag(TAG);

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
                        Debug.Log($"Start Point: {startingPoint.name}");
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

            if (_enemyWaiting) // pass from Stats SO
            {
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
            if (_waitTimer >= _waitTime)
            {
                _isWaiting = false;
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
            Debug.Log($"Reached waypoint {_currentWaypoint.name}, moving to next...");
        }

        // Now we have the destination, let the agent know it
        Vector3 targetPoint = _currentWaypoint.transform.position;
        _navMeshAgent.SetDestination(targetPoint);
        _isTraveling = true;

    }
    #endregion
}
