using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject[] waypoints;
    public bool cyclical = true;
    int current = 0;
    public float moveSpeed;
    public float lookSpeed;
    float radius = 0.2f;
    bool backward = false;

    public GameObject player;
    bool followPlayer = false;


    void Update()
    {
        // pulsar Space para simular perseguir al jugador
        // volver a pulsar para retomar la patrulla

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (!followPlayer)
                followPlayer = true;
            else followPlayer = false;
        }
        
        if (!followPlayer) //si está en modo patrulla
        {
            if (cyclical)
                Cyclical();
            else BackNForth();
            LookAtWaypoint();
        }
        else //si debe "seguir al jugador"
        {
            GoToPlayer();
            LookAtPlayer();
        } 
    }

    void Cyclical() //patrulla en bucle
    {
        //selección de waypoint
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < radius)
        {
            current += 1;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }

        //ir al waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * moveSpeed);
    }

    void BackNForth() //patrulla ida y vuelta
    {
        //selección de waypoint
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < radius)
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

        //ir al waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * moveSpeed);
    }

    void GoToPlayer() //ir al "jugador"
    {
        if (Vector3.Distance(player.transform.position, transform.position) >= radius)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
    }

    void LookAtWaypoint() //rotación enemigo
    {
        Vector3 dir = waypoints[current].transform.position - transform.position;

        transform.right = Vector3.Lerp(transform.right, dir, Time.deltaTime * lookSpeed);
    }

    void LookAtPlayer() //rotación enemigo
    {
        Vector3 dir = player.transform.position - transform.position;

        transform.right = Vector3.Lerp(transform.right, dir, Time.deltaTime * lookSpeed);
    }

}
