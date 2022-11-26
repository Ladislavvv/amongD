using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerInfo : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    public int colorIndex;

    public SpriteRenderer playerBody;

    public List<Color> _allPlayerColors = new List<Color>();
    public Text _playerName;

    public Color CurrentColor
    {
        get { return _allPlayerColors[colorIndex]; }
    }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            colorIndex = Random.Range(0, _allPlayerColors.Count);// -1 ����� �� ����
            _playerName.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            _playerName.text = GetPlayerName(photonView.OwnerActorNr);
            Destroy(GetComponentInChildren<Light2D>());
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Owner
            stream.SendNext(colorIndex);
        }
        else
        {
            //Remote
            colorIndex = (int)stream.ReceiveNext();
        }
    }


    void Update()
    {
        playerBody.color = _allPlayerColors[colorIndex];
    }

    private string GetPlayerName(int actorID)
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == actorID)
            {
                return player.Value.NickName;
            }
        }
        return "[none]";
    }
}
