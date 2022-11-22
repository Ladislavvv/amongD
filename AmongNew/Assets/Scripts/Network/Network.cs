using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Network : MonoBehaviourPunCallbacks
{
    public Text statusText;
    public CameraFollow playerCamera;

    private void Start()
    {
        statusText.text = "Connecting";
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "Connected to Master / joining room";
        PhotonNetwork.JoinOrCreateRoom("GameRoom", new RoomOptions() { MaxPlayers = 10 }, null);
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Connected to " + PhotonNetwork.CurrentRoom.Name;
        playerCamera.target = PhotonNetwork.Instantiate("Player",
            new Vector3(
                Random.Range(-4, 4),
                Random.Range(-4, 0),
                0), Quaternion.identity).transform;
    }
}
