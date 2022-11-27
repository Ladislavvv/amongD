using Photon.Pun;
using UnityEngine;

public class PlayerDeadBodyReport : MonoBehaviourPun {

    private UIControl _uiControl;
    private VotingManager _votingManager;

    public void Initialize(UIControl uiControl, VotingManager votingManager) {
        _uiControl = uiControl;
        _votingManager = votingManager;
        Debug.Log("cs12 InInitialize _uiControl:" + _uiControl + " - _votingManager: " + _votingManager);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("In OnTriggerEnter2D - 1");
        if (_votingManager == null) { return; }
        Debug.Log("In OnTriggerEnter2D - 2");
        if (collision.gameObject.GetComponent<PlayerDeadBody>() == null) { return; }

        Debug.Log("In OnTriggerEnter2D - 3");
        if (_votingManager.WasBodyReported(photonView.OwnerActorNr) == false) {
            Debug.Log("HasDeadBodyInRange = true; #####");
            _uiControl.HasDeadBodyInRange = true;
            _votingManager.DeadBodyInProximity = collision.gameObject.GetComponent<PhotonView>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (_votingManager == null) { return; }

        if (_votingManager.DeadBodyInProximity == collision.gameObject.GetComponent<PhotonView>()) {
            _uiControl.HasDeadBodyInRange = false;
            _votingManager.DeadBodyInProximity = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("In OnTriggerStay2D - 1");
        if (_votingManager == null) { return; }
        Debug.Log("In OnTriggerStay2D - 2");

        PhotonView targetPhotonView = collision.gameObject.GetComponent<PhotonView>();
        //if (targetPhotonView == null) { return; }
        Debug.Log("In OnTriggerStay2D - 3");

        if (_votingManager.WasBodyReported(targetPhotonView.OwnerActorNr))// && targetPhotonView.gameObject.CompareTag("DeadBody"))
        {
            Debug.Log("In OnTriggerStay2D - 4");

            if (_votingManager.DeadBodyInProximity == targetPhotonView)
            {
                Debug.Log("In OnTriggerStay2D - 5");
                _uiControl.HasDeadBodyInRange = false;
            }
        }
    }
}