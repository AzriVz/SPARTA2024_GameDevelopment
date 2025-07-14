using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] waypoints;
    private bool isMoving = false;
    public CurtainRandomizer randomizer;

    private int _currentWaypointIndex = 0;


    public void Start()
    {
        StartCoroutine(CurtainOpen());
    }

    public IEnumerator CurtainOpen()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(randomizer.GameSequence());
        yield return new WaitForSeconds(1.5f);
        StartMoving();
    }

    public void StartMoving()
    {
        isMoving = true;
    }
    private void Awake () {
    foreach (Transform coord in waypoints) 
    {
        coord.SetParent(null, true);
    }
    }
    void Update()
    {
        if (!isMoving) return;
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].position) < 0.1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
