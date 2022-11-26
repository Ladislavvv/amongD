using Photon.Pun;
using UnityEngine;

public class PlayerDeadBodyReport : MonoBehaviourPun {

    private UIControl _uiControl;
    private VotingManager _votingManager;

    public void Initialize(UIControl uiControl, VotingManager votingManager) {
        _uiControl = uiControl;
        _votingManager = votingManager;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_votingManager == null) { return; }
        if (collision.gameObject.GetComponent<PlayerDeadBody>() == null) { return; }

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

    //private void OnTriggerStay2D(Collider2D collision) {
    //    if (_votingManager == null) { return; }
    //    PhotonView targetPhotonView = collision.gameObject.GetComponent<PhotonView>();

    //    if (_votingManager.WasBodyReported(targetPhotonView.OwnerActorNr) && targetPhotonView.gameObject.CompareTag("DeadBody"))
    //    {
    //        if (_votingManager.DeadBodyInProximity == targetPhotonView) 
    //        {
    //            _uiControl.HasDeadBodyInRange = false;
    //        }
    //    }
    //}
}