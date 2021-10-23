using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    public Vector3 offset;
    public float lerpSpeed;
    private void Update()
    {
        Vector3 newPosition = target.position + offset;
        Vector3 positionLerped = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
        transform.position = positionLerped;
    }
}
