using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    //Components
    Rigidbody myRB;
    [SerializeField] Transform myAvatar;
    Animator myAnim;

    public UIControl _uiControl;
    //���������� ����������
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
        if (photonView.IsMine)
        {
            myRB = GetComponent<Rigidbody>();
            //myAvatar = transform.GetChild(0);
            //Debug.Log("myAvatr: " + myAvatar);
            myAnim = GetComponent<Animator>();
}
    }

    private void Update()
    {
        myAvatar.localScale = new Vector2(direction, 1);
        if (photonView.IsMine && !_uiControl.IsChatWindowActive)
        {
            movementInput = WASD.ReadValue<Vector2>();
            myAnim.SetFloat("Speed", movementInput.magnitude);
            if (movementInput.x != 0)
            {
                direction = Mathf.Sign(movementInput.x);
            }
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
            myRB.velocity = movementInput.normalized * movementSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(direction);
        }
        else
        {
            direction = (float)stream.ReceiveNext();
        }
    }
}
