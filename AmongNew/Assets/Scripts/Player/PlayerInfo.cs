using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerInfo : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    public int colorIndex;

    public SpriteRenderer playerBody;

    public List<Color> _allPlayerColors = new List<Color>();

    public Color CurrentColor
    {
        get { return _allPlayerColors[colorIndex]; }
    }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            colorIndex = Random.Range(0, _allPlayerColors.Count - 1);// -1 ����� �� ����
        }
        else
        {
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
}
