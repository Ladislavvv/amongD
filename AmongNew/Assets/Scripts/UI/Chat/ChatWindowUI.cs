using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindowUI : MonoBehaviourPun {

    [SerializeField] private ChatItemUI _chatItemPrefab;
    [SerializeField] private Transform _container;

    [SerializeField] private InputField _inputText;
    public PlayerInfo _playerInfo;
    //public Text _playerName;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SendMessage();
        }
    }

    private void OnEnable() {
        _inputText.text = string.Empty;
        _inputText.ActivateInputField();
    }

    private void SendMessage() {
        // Do not send empty messages
        if (string.IsNullOrEmpty(_inputText.text)) { return; }
        if (_playerInfo == null) { return; }

        photonView.RPC("ReceiveMessageRPC", RpcTarget.All, _inputText.text, _playerInfo.CurrentColor.r, _playerInfo.CurrentColor.g, _playerInfo.CurrentColor.b, _playerInfo._playerName.text);
        //
        //_playerName.text = PhotonNetwork.LocalPlayer.NickName;
        //
        _inputText.text = string.Empty;
        _inputText.ActivateInputField();
    }

    private void InstantiateChatItem(string text, Color color, string _playerName) {
        ChatItemUI newChatItem = Instantiate(_chatItemPrefab);

        newChatItem.transform.SetParent(_container);
        newChatItem.transform.position = Vector3.zero;
        newChatItem.transform.localScale = Vector3.one;

        newChatItem.Initialize(text, color, _playerName);
    }

    [PunRPC]
    public void ReceiveMessageRPC(string text, float red, float green, float blue, string _playerName) {
        InstantiateChatItem(text, new Color(red, green, blue), _playerName);
    }

    public void OnSendButtonPressed() {
        SendMessage();
    }

}