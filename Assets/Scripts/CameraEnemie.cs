using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnemie : MonoBehaviour
{
    float initialRotation;
    public float arc;
    StateMachine stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = Mathf.Abs(transform.eulerAngles.z);
        Debug.Log("initial rotations: " + initialRotation);
        stateMachine = new StateMachine();
        var searching = stateMachine.CreateState("searching");
        searching.onEnter = delegate
        {
            Debug.Log("Now camera is searching");
        };

        searching.onFrame = delegate
        {
            var angle = Mathf.Sin(Time.time) * arc / 2f;
            Debug.Log("Before: " + angle);
            angle += initialRotation;
            Debug.Log("After: " + angle);
            transform.eulerAngles = Vector3.forward * angle;
        };
    }
    private void Update()
    {
        stateMachine.Update();
    }
}
