using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    public int colorIndex;

    public SpriteRenderer playerBody;

    public List<Color> _allPlayerColors = new List<Color>();

    private void Awake()
    {
        if (photonView.IsMine)
        {
            colorIndex = Random.Range(0, _allPlayerColors.Count - 1);// -1 ����� �� ����
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
}
