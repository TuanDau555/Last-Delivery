using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWayPoint : MonoBehaviour
{
    [SerializeField]
    [Range(1, 100)]
    [Tooltip("Radius to connect waypoints together")]
    private float connectedRadius;
    // TODO: we could add priority, instead of choosing random Enemy could choose based on weight point
    [SerializeField] private List<ConnectedWayPoint> _connection;
    [SerializeField] private bool isDrawGizmos;

    private const string TAG = "Waypoint";

    void Start()
    {
        InitializedWaypoint();
    }

    private void InitializedWaypoint()
    {
        // Grab all waypoints in the Scene
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag(TAG);

        _connection = new List<ConnectedWayPoint>();
        foreach (GameObject _waypoint in waypoints)
        {
            ConnectedWayPoint nextWaypoint = _waypoint.GetComponent<ConnectedWayPoint>();

            // found a waypoint
            if (nextWaypoint != null)
            {
                // Evaluate the distance between two points, check if it connected 
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position)
                <= connectedRadius && nextWaypoint != this)
                {
                    _connection.Add(nextWaypoint);
                }
            }
        }
    }

    public ConnectedWayPoint NextWaypoint(ConnectedWayPoint previousWaypoint)
    {
        // No waypoint
        if (_connection.Count == 0)
            return null;
        // Only one waypoint (Dead End route)
        else if (_connection.Count == 1 && _connection.Contains(previousWaypoint))
            return previousWaypoint;

        // Find a random waypoint 
        ConnectedWayPoint nextWaypoint;
        do
        {
            int nextIndex = UnityEngine.Random.Range(0, _connection.Count);
            nextWaypoint = _connection[nextIndex];
        } while (nextWaypoint == previousWaypoint);
        return nextWaypoint;
    }
    
    private void OnDrawGizmos()
    {
        if (isDrawGizmos)
        {    
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, connectedRadius);
        }
    }
}
