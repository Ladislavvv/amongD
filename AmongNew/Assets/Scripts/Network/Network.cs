using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Network : MonoBehaviourPunCallbacks
{
    //public Text statusText;
    public MasterClient masterClient;
    public CameraFollow playerCamera;
    public UIControl uiControl;
    public ChatWindowUI chatWindowUI;
    public VotingManager votingManager;
    private PhotonView _playerPhotonView;

    private void Start()
    {
        //statusText.text = "Connected to " + PhotonNetwork.CurrentRoom.Name;
         GameObject newPlayer = PhotonNetwork.Instantiate("Player",
            new Vector3(
                Random.Range(-4, 4),
                Random.Range(-4, 0),
                0), Quaternion.identity);
        //statusText.text = "Connecting";
        //PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        //PhotonNetwork.ConnectUsingSettings();

        playerCamera.target = newPlayer.transform;
        chatWindowUI._playerInfo = newPlayer.GetComponent<PlayerInfo>();
        newPlayer.GetComponent<Player_Controller>()._uiControl = uiControl;
        newPlayer.GetComponentInChildren<PlayerDeadBodyReport>().Initialize(uiControl, votingManager);
        _playerPhotonView = newPlayer.GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            masterClient.Initialize();
        }
    }

    public void DestroyPlayer()
    {
        PhotonNetwork.Destroy(_playerPhotonView);
    }

    //public override void OnConnectedToMaster()
    //{
    //    //statusText.text = "Connected to Master / joining room";
    //    //PhotonNetwork.JoinOrCreateRoom("GameRoom", new RoomOptions() { MaxPlayers = 10 }, null);
    //}

    //public override void OnJoinedRoom()
    //{
    //    statusText.text = "Connected to " + PhotonNetwork.CurrentRoom.Name;
    //    playerCamera.target = PhotonNetwork.Instantiate("Player",
    //        new Vector3(
    //            Random.Range(-4, 4),
    //            Random.Range(-4, 0),
    //            0), Quaternion.identity).transform;
    //}
}
