using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] waypoints;
    

    private int _currentWaypointIndex = 0;


    private void Awake () {
    foreach (Transform coord in waypoints) 
    {
        coord.SetParent(null, true);
    }
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].position) < 0.1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
