using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    float horizontal;
    float vertical;
    public float speed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

    }
    private void FixedUpdate()
    {
        if (horizontal != 0) Orientate();
        if (vertical != 0) Move();
    }

    void Orientate()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0, 0, -horizontal) * rotationSpeed);
    }

    void Move()
    {
        transform.position += transform.up * vertical * speed;
    }
}
