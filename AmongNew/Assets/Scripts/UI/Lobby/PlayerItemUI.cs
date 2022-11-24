using UnityEngine;
using UnityEngine.UI;

public class PlayerItemUI : MonoBehaviour {

    [SerializeField] private Text _playerName;

    public void Initialize(string playerName) {
        _playerName.text = playerName;
    }
}