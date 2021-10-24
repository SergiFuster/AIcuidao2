using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] waypointsTransform;
    public Transform[] buttons;
    Vector3[] waypoints;
    FieldOfView fov;
    Vector3[] path;
    int targetIndex;

    StateMachine stateMachine;

    public bool cyclical = true;
    int current = 0;
    public float moveSpeed;
    public float lookSpeed;
    public float radius = 0.2f;
    bool backward = false;

    private void Start()
    {
        fov = GetComponent<FieldOfView>();
        stateMachine = new StateMachine();

        #region PATROLING STATE
        var patroling = stateMachine.CreateState("patroling");

        patroling.onEnter = delegate
        {
            Debug.Log("Now patroling waypoints");
        };

        patroling.onFrame = delegate
        {
            if (fov.targetDetected)
            {
                stateMachine.TransitionTo("alert");
            }
            LookAtWaypoint();

            if (cyclical)
                Cyclical();
            else
                BackNForth();

            // Trigger to state change
        };

        #endregion

        #region ALERT
        var alert = stateMachine.CreateState("alert");

        alert.onEnter = delegate
        {
            Vector3 targetButton = buttons[0].position;
            for(int i = 1; i < buttons.Length; i ++)
            {
                if (Vector3.Distance(transform.position, buttons[i].position) < Vector3.Distance(transform.position, targetButton))
                    targetButton = buttons[i].position;
            }
            PathRequestManager.RequestPath(transform.position, targetButton, OnPathFound);
        };

        #endregion
        
        waypoints = new Vector3[waypointsTransform.Length];
        for(int i = 0; i < waypointsTransform.Length; i++)
        {
            waypoints[i] = waypointsTransform[i].position;
        }
        transform.position = waypoints[current];
    }
    private void FixedUpdate()
    {
        stateMachine.Update();
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            Vector3 dir = (currentWaypoint - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, lookSpeed);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed);
            yield return null;
        }

        //START ALARM GAMESTATE
    }
    public void Cyclical() //patrulla en bucle
    {
        //selección de waypoint
        if (Vector3.Distance(waypoints[current], transform.position) < radius)
        {
            current += 1;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }
        Vector3 dir = (waypoints[current] - transform.position).normalized;
        //ir al waypoint
        transform.position += dir * moveSpeed;
    }

    public void BackNForth() //patrulla ida y vuelta
    {
        //selección de waypoint
        if (Vector3.Distance(waypoints[current], transform.position) < radius)
        {
            if (backward) //a la vuelta
            {
                current -= 1;
                if (current < 0)
                {
                    backward = false;
                    current = 0;
                }
            }
            else //a la ida
            {
                current += 1;
                if (current >= waypoints.Length)
                {
                    backward = true;
                    current = waypoints.Length - 1;
                }
            }
        }
        Vector3 dir = (waypoints[current] - transform.position).normalized;
        //ir al waypoint
        transform.position += dir * moveSpeed;
    }

    public void LookAtWaypoint() //rotación enemigo
    {
        
        Vector3 dir = (waypoints[current] - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, lookSpeed);

    }
}
