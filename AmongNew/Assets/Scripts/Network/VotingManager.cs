using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotingManager : MonoBehaviourPun {

    public static VotingManager Instance;

    [SerializeField] private GameObject _emergencyMeetingWindow;

    [SerializeField] private Button _skipVoteBtn;
    [SerializeField] private Network _network;

    [SerializeField] private VotePlayerItem _votePlayerItemPrefab;
    [SerializeField] private Transform _votePlayerItemContainer;
    [HideInInspector] private bool HasAlreadyVoted;
    private List<VotePlayerItem> _votePlayerItemList = new List<VotePlayerItem>();

    [SerializeField] private GameObject _kickPlayerWindow;
    [SerializeField] private GameObject _kickedPlayerWindow;
    [SerializeField] private Text _kickPlayerText;

    private List<int> _playersThatVotedList = new List<int>();
    private List<int> _playersThatHaveBeenVotedList = new List<int>();
    private List<int> _playersThatHaveBeenKickedOutList = new List<int>();

    [HideInInspector] public PhotonView DeadBodyInProximity;

    private List<int> _reportedDeadBodiesList = new List<int>();

    private void Awake() {
        Instance = this;
    }

    public bool WasBodyReported(int actorNumber) {
        return _reportedDeadBodiesList.Contains(actorNumber);
    }

    public void ReportDeadBody() {
        Debug.Log("In ReportDeadBody-1");
        if (DeadBodyInProximity == null) { return; }
        Debug.Log("In ReportDeadBody-2 Reported");
        if (_reportedDeadBodiesList.Contains(DeadBodyInProximity.OwnerActorNr)) {
            // the body was already reported. do nothing
            return;
        }

        photonView.RPC("ReportDeadBodyRPC", RpcTarget.All, DeadBodyInProximity.OwnerActorNr);
    }

    [PunRPC]
    public void ReportDeadBodyRPC(int actorNumber) {
        _reportedDeadBodiesList.Add(actorNumber);

        _playersThatVotedList.Clear();
        _playersThatHaveBeenVotedList.Clear();
        HasAlreadyVoted = false;
        ToggleAllButtons(true);

        PopulatePlayerList();
        _emergencyMeetingWindow.SetActive(true);
    }

    private void PopulatePlayerList()
    {
        // Clear the previous vote player list.

        for (int i = 0; i < _votePlayerItemList.Count; i++)
        {
            Destroy(_votePlayerItemList[i].gameObject);
        }

        _votePlayerItemList.Clear();

        // Create new vote player list.
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // Do not add the current player to the list
            if (player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                continue;
            }

            // Do not add the killed players to the list
            if (_reportedDeadBodiesList.Contains(player.Value.ActorNumber))
            {
                continue;
            }

            // Do not add the players that have been kicked out
            if (_playersThatHaveBeenKickedOutList.Contains(player.Value.ActorNumber))
            {
                continue;
            }

            VotePlayerItem newPlayerItem = Instantiate(_votePlayerItemPrefab, _votePlayerItemContainer);
            newPlayerItem.Initialize(player.Value, this);
            _votePlayerItemList.Add(newPlayerItem);
        }
    }

    private void ToggleAllButtons(bool areOn)
    {
        _skipVoteBtn.interactable = areOn;
        foreach (VotePlayerItem votePlayerItem in _votePlayerItemList)
        {
            votePlayerItem.ToggleButton(areOn);
        }
    }

    public void CastVote(int targetActorNumber)
    {
        if (HasAlreadyVoted) { return; }

        HasAlreadyVoted = true;
        ToggleAllButtons(false);
        photonView.RPC("CastPlayerVoteRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, targetActorNumber);
    }

    [PunRPC]
    public void CastPlayerVoteRPC(int actorNumber, int targetActorNumber)
    {
        int remainingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - _reportedDeadBodiesList.Count - _playersThatHaveBeenKickedOutList.Count;

        // Set the status of the player that has just voted

        foreach (VotePlayerItem votePlayerItem in _votePlayerItemList)
        {
            if (votePlayerItem.ActorNumber == actorNumber)
            {
                votePlayerItem.UpdateStatus(targetActorNumber == -1 ? "SKIPPED" : "VOTED");
                break;
            }
        }

        // Log the player that just voted / and who voted for

        if (!_playersThatVotedList.Contains(actorNumber))
        {
            _playersThatVotedList.Add(actorNumber);
            _playersThatHaveBeenVotedList.Add(targetActorNumber);
        }

        if (!PhotonNetwork.IsMasterClient) { return; }
        if (_playersThatVotedList.Count < remainingPlayers) { return; }

        // Count all the votes
        Dictionary<int, int> playerVoteCount = new Dictionary<int, int>();

        foreach (int votedPlayer in _playersThatHaveBeenVotedList)
        {
            if (!playerVoteCount.ContainsKey(votedPlayer))
            {
                playerVoteCount.Add(votedPlayer, 0);
            }

            playerVoteCount[votedPlayer]++;
        }

        // Get the most voted player
        int mostVotedPlayer = -1;
        int mostVotes = int.MinValue;

        foreach (KeyValuePair<int, int> playerVote in playerVoteCount)
        {
            if (playerVote.Value > mostVotes)
            {
                mostVotes = playerVote.Value;
                mostVotedPlayer = playerVote.Key;
            }
        }

        // End the voting session
        if (mostVotes >= remainingPlayers / 2)
        {
            // Kick the player or skip
            //KickPlayerRPC(mostVotedPlayer);
            photonView.RPC("KickPlayerRPC", RpcTarget.All, mostVotedPlayer);///////////добавил сам
        }
    }

    [PunRPC] // добавил сам
    //[PunRPC]
    public void KickPlayerRPC(int actorNumber)
    {
        _emergencyMeetingWindow.SetActive(false);
        _kickPlayerWindow.SetActive(true);

        string playerName = string.Empty;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == actorNumber)
            {
                playerName = player.Value.NickName;
                break;
            }
        }

        _kickPlayerText.text = actorNumber == -1 ? "No one has been kicked out" : "Player " + playerName + " has been kicked out";

        StartCoroutine(FadeKickPlayerWindow(actorNumber));
    }

    private IEnumerator FadeKickPlayerWindow(int actorNumber)
    {
        yield return new WaitForSeconds(2.5f);
        _kickPlayerWindow.SetActive(false);
        _emergencyMeetingWindow.SetActive(false);

        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            _network.DestroyPlayer();
            _kickedPlayerWindow.SetActive(true);
        }
    }
}