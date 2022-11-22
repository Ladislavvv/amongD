using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : Photon.Pun.MonoBehaviourPun
{
    //Components
    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;
    //���������� ����������
    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    [SerializeField] float movementSpeed;

    //float direction = 1;

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
        if (photonView.IsMine)
        {
            myRB = GetComponent<Rigidbody>();
            myAvatar = transform.GetChild(0);
            myAnim = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            movementInput = WASD.ReadValue<Vector2>();
            if (movementInput.x != 0)
            {
                myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
            }
            myAnim.SetFloat("Speed", movementInput.magnitude);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
            myRB.velocity = movementInput * movementSpeed;
    }
}
