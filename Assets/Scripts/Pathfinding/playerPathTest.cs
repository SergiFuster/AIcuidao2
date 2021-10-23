using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPathTest : MonoBehaviour
{
    List<Node> path = null;
    public float stepSize;
    public float distanceToNext;
    public float minRotationSpeed;
    public float maxRotationSpeed;
    private float rotationSpeed;
    bool positionReached = false;
    public Transform target;
    int index = 0;
    Vector3 currentTargetPosition;

    private void FixedUpdate()
    {
        if (path == null) lookForPath();
        else
        {
            if (positionReached) tryNextNode();
            else
            {
                moveToNode();
            }
        }
    }

    void lookForPath()
    {
        
        if((target.position - transform.position).magnitude > 1)
        {
            path = Pathfinding.instance.FindPath(transform.position, target.position);
            if(path != null && path.Count > 1)
            {
                Debug.Log("Path found");
                
                positionReached = false;
                index = 0;
                currentTargetPosition = path[index].worldPosition;
            }
        }
    }

    void moveToNode()
    {
        float distance = (currentTargetPosition - transform.position).magnitude;
        if (distance < distanceToNext) positionReached = true;
        else
        {
            Vector3 direction = (currentTargetPosition - transform.position).normalized;
            float degrees = Vector3.Angle(transform.up, direction);
            if(Mathf.Abs(degrees) > 45)
            {
                rotationSpeed = maxRotationSpeed;
            }
            else
            {
                rotationSpeed = minRotationSpeed;
            }
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed);
            transform.position += transform.up * stepSize;
        }
    }

    void tryNextNode()
    {
        if(path != null)
        {
            if (index >= path.Count-1 || index < 0)
                path = null;
            else
            {
                index++;
                positionReached = false;
                currentTargetPosition = path[index].worldPosition;
            }
        }
    }
}
