using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TaskInteraction : MonoBehaviourPun {

    [SerializeField] private float _range = 10.0f;
    private LineRenderer _lineRenderer;
    private Interactible _target;

    private void Awake() {
        if (!photonView.IsMine) { return; }
        _lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(SearchForInteraction());
    }

    private void Update() {
        if (!photonView.IsMine) { return; }

        if (_target != null) {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _target.transform.position);
        }
        else {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }

    private IEnumerator SearchForInteraction() {
        while (true) {
            Interactible newTarget = null;
            Interactible[] interactionList = FindObjectsOfType<Interactible>();

            UIControl.Instance.HasInteractible = false;

            foreach (Interactible interactible in interactionList) {                
                float distance = Vector3.Distance(transform.position, interactible.transform.position);
                if (distance > _range) { continue; }

                // An interactible new target found
                newTarget = interactible;
                UIControl.Instance.HasInteractible = true;
                //UIControl.Instance.CurrentInteractible = _target;

                break;
            }

            // Reset the previous interactible, if any
            if (UIControl.Instance.CurrentInteractible != newTarget &&
                UIControl.Instance.CurrentInteractible != null) {
                UIControl.Instance.CurrentInteractible.Use(false);
            }

            _target = newTarget;
            UIControl.Instance.CurrentInteractible = _target;

            yield return new WaitForSeconds(0.25f);
        }
    }

}