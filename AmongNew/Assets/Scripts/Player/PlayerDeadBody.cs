using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadBody : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private SpriteRenderer _bodyFill;

    public void SetColor(Color color)
    {
        _bodyFill.color = color;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //owner
            stream.SendNext(_bodyFill.color.r);
            stream.SendNext(_bodyFill.color.g);
            stream.SendNext(_bodyFill.color.b);
        }
        else
        {
            //remote
            float red = (float)stream.ReceiveNext();
            float green = (float)stream.ReceiveNext();
            float blue = (float)stream.ReceiveNext();
            _bodyFill.color = new Color(red, green, blue, 1.0f);
        }
    }
}
