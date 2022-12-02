using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviourPun
{
    public UIControl uiControl;
    public VotingManager votingManager;
    public MasterClient masterClient;
    private PhotonView _playerPhotonView;

    public SwipeTask _swipeTask;
    public KeypadTask _keypadTask;
    public StartReactorTask _startReactorTask;


    public int totalTasks;
    public int leftTaska;


    public int keyPad_left = 2;
    public int keyPad_total;

    public int Reactor_left = 2;
    public int Reactor_total;

    public int CardSwip_left = 2;
    public int CardSwipe_total;


    private void Awake()
    {
        if (!photonView.IsMine) { return; }
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }

       
    }
}
