using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour {

    public LobbyNetworkManager LobbyNetworkParent;
    public string Password => _password;
    private string _password;
    private bool _isPrivate;

    [SerializeField] private Text _roomName;
    [SerializeField] private GameObject _privateRoomLock;

    public void Initialize(string roomName, string password, bool isPrivate) {
        _roomName.text = roomName;
        _password = password;
        _isPrivate = isPrivate;

        if (_privateRoomLock != null) {
            _privateRoomLock.SetActive(!string.IsNullOrEmpty(_password));
        }

        if (isPrivate && string.IsNullOrEmpty(_password)) {
            gameObject.SetActive(false);
        }
    }

    public void OnJoinPressed() {
        LobbyNetworkParent.JoinRoom(_roomName.text, _isPrivate, _password);
    }
}