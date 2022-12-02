using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Killable : MonoBehaviourPun {

    //public UIControl uiControl;
    public GameObject canvas;

    /// <summary>
    public GameObject[] playersTotal;
    private int impostersTotal;

    public int completedTasks = 0;
    public int tasksToDo = 0;
    private int flag = 0;
    /// </summary>

    //[HideInInspector]
    public bool IsImpostor = false;

    [SerializeField] private float _range = 10.0f;
    private LineRenderer _lineRenderer;
    private Killable _target;

    private void Awake() {
        if (!photonView.IsMine) { return; }
        _lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(SearchForKillable());
    }

    private void Start() {
        if (!photonView.IsMine) { return; }
        UIControl.Instance.CurrentPlayer = this;
        canvas = GameObject.Find("Canvas (1)");
        //tasksToDo = GameObject.FindGameObjectsWithTag("Player").Length * 5;
        //Debug.Log("tasksToDo: " + tasksToDo);
    }

    private void Update() {
        if (!photonView.IsMine) { return; }
        
        if (_target != null && IsImpostor && !_target.IsImpostor) 
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _target.transform.position);
        }
        else {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }

        //
        //playersTotal = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log("playersTotal: " + playersTotal.Length);

        //if (playersTotal.Length < 3) { return; }


        tasksToDo = playersTotal.Length * 5;
        Debug.Log("tasksToDo: " + tasksToDo);
        
        //

        /// IF TASKS ARE COMPLETED -> THE END, PLAYERS WIN
        
        if (completedTasks >= tasksToDo && completedTasks != 0)
        {
            Debug.Log("Players WIN! completedTasks == tasksToDo");
            canvas.GetComponent<UIControl>().OnPlayersWin();
        }

        /// 


        if (flag < 305) flag++;
        
        /// END OF THE GAME IF COUNT PF PLAYERS <= >= ==
        impostersTotal = 0;
        playersTotal = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("playersTotal: " + playersTotal.Length);

        //if (playersTotal.Length < 3) { return; }

        foreach (GameObject playerI in playersTotal)
        {
            if (playerI.GetComponent<Killable>().IsImpostor)
            {
                impostersTotal++;
            }
        }
        if (playersTotal.Length == impostersTotal)
        {
            //Debug.Log("Imposters WIN!");
            canvas.GetComponent<UIControl>().OnImpostersWin();
        }

        if (flag > 300)
        {
            if (impostersTotal <= 0)
            {
                Debug.Log("Players WIN! impostersTotal <= 0: '" + impostersTotal + "'");
                canvas.GetComponent<UIControl>().OnPlayersWin();
            }
        }
        //StartCoroutine(EndGameImposterLow());
        //if (impostersTotal <= 0)
        //{
        //    Debug.Log("Players WIN! impostersTotal <= 0: " + impostersTotal);
        //    canvas.GetComponent<UIControl>().OnPlayersWin();
        //}

        /// можно попробовать через RPC
    }

    private IEnumerator EndGameImposterLow()
    {
        while (true)
        {
        if (impostersTotal <= 0)
        {
            Debug.Log("Players WIN! impostersTotal <= 0: " + impostersTotal);
            canvas.GetComponent<UIControl>().OnPlayersWin();
        }

        yield return new WaitForSeconds(1f);
        }
    }


    private IEnumerator SearchForKillable() {
        while (true) {
            Killable newTarget = null;
            Killable[] killList = FindObjectsOfType<Killable>();

            foreach (Killable kill in killList) {
                if (kill == this) { continue; }
                float distance = Vector3.Distance(transform.position, kill.transform.position);
                if (distance > _range) { continue; }

                // A killable new target found
                newTarget= kill;
                UIControl.Instance.HasTarget = _target != null;

                break;
            }

            _target = newTarget;

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Kill() {
        if (_target == null || _target.IsImpostor) { return; }
        PhotonView pv = _target.GetComponent<PhotonView>();
        pv.RPC("KillRPC", RpcTarget.All);
    }

    [PunRPC]
    public void KillRPC() {
        if (!photonView.IsMine) { return; }
        
        PlayerDeadBody playerBody = PhotonNetwork.Instantiate("PlayerBody", transform.position, Quaternion.identity).GetComponent<PlayerDeadBody>();
        PlayerInfo playerInfo = GetComponent<PlayerInfo>();

        playerBody.SetColor(playerInfo._allPlayerColors[playerInfo.colorIndex]);
        //transform.position = new  Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 0f), 0);
        PhotonNetwork.Destroy(photonView);
        _target = null;
        UIControl.Instance.HasTarget = _target;
        //PhotonNetwork.Disconnect();
        UIControl.Instance.OnThisPlayerKilled();
    }

    [PunRPC]
    public void SetImpostor() {
        IsImpostor = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Owner
            stream.SendNext(completedTasks);
            //stream.SendNext(impostersTotal);
        }
        else
        {
            //Remote
            completedTasks = (int)stream.ReceiveNext();
            //impostersTotal = (int)stream.ReceiveNext();
        }
    }
}
