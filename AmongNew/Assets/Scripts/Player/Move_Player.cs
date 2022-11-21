using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    public float speed = 4;

    Vector3 velocity;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
    }

    private void Update()
    {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
    }
}

