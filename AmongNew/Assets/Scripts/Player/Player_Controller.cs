using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    //Components
    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;
    //Управление персонажем
    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    [SerializeField] float movementSpeed;

    float direction = 1;

    private void OnEnable()
    {
        WASD.Enable();   
    }

    private void OnDisable()
    {
        WASD.Disable();
    }

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAvatar = transform.GetChild(0);
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        movementInput = WASD.ReadValue<Vector2>();
        if (movementInput.x != 0)
        {
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
        }

        myAnim.SetFloat("Speed", movementInput.magnitude);
    }

    private void FixedUpdate()
    {
        myRB.velocity = movementInput * movementSpeed;
    }
}
