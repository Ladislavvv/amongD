using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private InputField _roomInput;
    [SerializeField] private RoomItemUI _roomItemUIPrefab;
    [SerializeField] private Transform _roomListParent;

    [SerializeField] private PlayerItemUI _playerItemUIPrefab;
    [SerializeField] private Transform _playerListParent;

    [SerializeField] private Text _statusField;
    [SerializeField] private Button _leaveRoomButton;
    [SerializeField] private Button _startGameButton;

    [SerializeField] private Text _currentLocationText;

    [SerializeField] private GameObject _roomListWindow;
    [SerializeField] private GameObject _playerListWindow;
    [SerializeField] private GameObject _createRoomWindow;

    // Player Name Functionality
    [SerializeField] private InputField _playerNameInput;
    [SerializeField] private Text _playerNameLabel;
    private bool _isPlayerNameChanging;

    

    private List<RoomItemUI> _roomList = new List<RoomItemUI>();
    private List<PlayerItemUI> _playerList = new List<PlayerItemUI>();
    private List<RoomInfo> _currentRoomList;

    private void Start()
    {
        Initialize();
        Connect();
    }

    #region PhotonCallbacks

    public override void OnConnectedToMaster()
    {
        _statusField.text = "Connected to master server";
        PhotonNetwork.NickName = "Player" + Random.Range(0, 999);
        _playerNameLabel.text = PhotonNetwork.NickName;
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (_statusField == null) { return; }
        _statusField.text = "Disconnected";
    }

    public override void OnJoinedLobby()
    {
        _currentLocationText.text = "Rooms";
    }

    public override void OnJoinedRoom()
    {
        _statusField.text = "Joined " + PhotonNetwork.CurrentRoom.Name;
        _currentLocationText.text = PhotonNetwork.CurrentRoom.Name;

        _leaveRoomButton.interactable = true;

        if (PhotonNetwork.IsMasterClient)
        {
            _startGameButton.interactable = true;
        }

        ShowWindow(false);
        UpdatePlayerList();
    }

    public override void OnLeftRoom()
    {
        if (_statusField != null)
        {
            _statusField.text = "LOBBY";
        }

        if (_currentLocationText != null)
        {
            _currentLocationText.text = "Rooms";
        }

        _leaveRoomButton.interactable = false;
        _startGameButton.interactable = false;

        ShowWindow(true);
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    #endregion

    #region Functionality
    private void Initialize()
    {
        _leaveRoomButton.interactable = false;
    }

    private void Connect()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        // Clear the current list of rooms
        for (int i = 0; i < _roomList.Count; i++)
        {
            Destroy(_roomList[i].gameObject);
        }

        _roomList.Clear();
        _currentRoomList = new List<RoomInfo>(roomList);

        // Generate a new list with the updated info
        for (int i = 0; i < roomList.Count; i++)
        {
            // skip empty rooms
            if (roomList[i].PlayerCount == 0) { continue; }

            string roomPassword = string.Empty;
            bool isPrivate = false;

            if (roomList[i].CustomProperties.ContainsKey("pwd"))
            {
                roomPassword = (string)roomList[i].CustomProperties["pwd"];
                isPrivate = true;
            }

            RoomItemUI newRoomItem = Instantiate(_roomItemUIPrefab);
            newRoomItem.LobbyNetworkParent = this;
            newRoomItem.Initialize(roomList[i].Name, roomPassword, isPrivate);
            newRoomItem.transform.SetParent(_roomListParent);
            newRoomItem.transform.localScale = Vector3.one;

            _roomList.Add(newRoomItem);
        }
    }

    private void UpdatePlayerList()
    {
        // Clear the current player list
        for (int i = 0; i < _playerList.Count; i++)
        {
            Destroy(_playerList[i].gameObject);
        }

        _playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null) { return; }

        // Generate a new list of players
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItemUI newPlayerItem = Instantiate(_playerItemUIPrefab);

            newPlayerItem.transform.SetParent(_playerListParent);
            newPlayerItem.Initialize(player.Value.NickName);
            newPlayerItem.transform.localScale = Vector3.one;

            _playerList.Add(newPlayerItem);
        }
    }

    private void ShowWindow(bool isRoomList)
    {
        _roomListWindow.SetActive(isRoomList);
        _playerListWindow.SetActive(!isRoomList);
        _createRoomWindow.SetActive(isRoomList);
    }

    public void JoinRoom(string roomName, bool isPrivate, string password)
    {
        if (!isPrivate)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomInput.text) == false)
        {
            PhotonNetwork.CreateRoom(_roomInput.text, new RoomOptions() { MaxPlayers = 10 }, null);
        }
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region ButtonCallbacks

    public void OnBackToMainMenuPressed()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("main_menu_scene");
    }

    public void OnStartGamePressed()
    {
        PhotonNetwork.LoadLevel("Game_scene");//SampleScene 1 // Game_scene game_sceneOther
    }


    public void OnChangePlayerNamePressed()
    {
        if (_isPlayerNameChanging == false)
        {
            _playerNameInput.text = _playerNameLabel.text;
            _playerNameLabel.gameObject.SetActive(false);
            _playerNameInput.gameObject.SetActive(true);
            _isPlayerNameChanging = true;
        }
        else
        {
            // check for empty or long names
            if (string.IsNullOrEmpty(_playerNameInput.text) == false && _playerNameInput.text.Length <= 12)
            {
                _playerNameLabel.text = _playerNameInput.text;
                PhotonNetwork.LocalPlayer.NickName = _playerNameInput.text;
                photonView.RPC("ForcePlayerListUpdate", RpcTarget.All);
            }

            _playerNameLabel.gameObject.SetActive(true);
            _playerNameInput.gameObject.SetActive(false);
            _isPlayerNameChanging = false;
        }
    }

    [PunRPC]
    public void ForcePlayerListUpdate()
    {
        UpdatePlayerList();
    }

    #endregion
}