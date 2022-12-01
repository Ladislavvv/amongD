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

    public List<GameObject> _hats = new List<GameObject>();
    private int _hatindex = -1;

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
            _hatindex = PlayerPrefs.GetInt("PlayerHat");
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
            stream.SendNext(_hatindex);
        }
        else
        {
            //Remote
            colorIndex = (int)stream.ReceiveNext();
            _hatindex = (int)stream.ReceiveNext();
        }
    }


    void Update()
    {
        playerBody.color = _allPlayerColors[colorIndex];

        for (int  i = 0; i < _hats.Count; i++)
        {
            if(i == _hatindex)
                _hats[i].SetActive(true);
        }
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
