using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterClient : MonoBehaviourPun {

    [SerializeField] private GameObject _impostorWindow;
    [SerializeField] private Text _impostorText;
    public void Initialize() {
        if (PhotonNetwork.IsMasterClient) {
            StartCoroutine(PickImpostor());
        }
    }
    #region Impostor
    private IEnumerator PickImpostor() {
        GameObject[] players;
        List<int> playerIndex = new List<int>();
        int tries = 0;
        int impostorNumber = 0;
        int impostorNumberFinal = 0;
        do { // Get all the players in the game
            players = GameObject.FindGameObjectsWithTag("Player");
            tries++;
            yield return new WaitForSeconds(0.25f);
        } while ((players.Length < PhotonNetwork.PlayerList.Length) && (tries < 5));
        // Initialize the player index list
        for (int i = 0; i < players.Length; i++) { playerIndex.Add(i); }
        // Based on the player number, pick how many impostors to have
        impostorNumber = players.Length < 6 ? 1 : 2;
        impostorNumberFinal = impostorNumber;
        // Assign the impostor
        while (impostorNumber > 0) {
            int pickedImpostorIndex = playerIndex[Random.Range(0, playerIndex.Count)];
            playerIndex.Remove(pickedImpostorIndex);
            PhotonView pv = players[pickedImpostorIndex].GetComponent<PhotonView>();
            pv.RPC("SetImpostor", RpcTarget.All);
            impostorNumber--;
        }
        photonView.RPC("ImpostorPicked", RpcTarget.All, impostorNumberFinal);
    }
    [PunRPC]
    public void ImpostorPicked(int impostorNumber) {
        StartCoroutine(ShowImpostorAnimation(impostorNumber));
    }
    private IEnumerator ShowImpostorAnimation(int impostorNumber) {
        _impostorWindow.SetActive(true);
        _impostorText.gameObject.SetActive(true);
        _impostorText.text = 
            "There " + (impostorNumber < 2 ? "is" : "are") + " " + impostorNumber + 
            " impostor" + (impostorNumber > 1 ? "s" : string.Empty) + " among us...";
        yield return new WaitForSeconds(3);
        _impostorWindow.SetActive(false);
    }
    #endregion
}