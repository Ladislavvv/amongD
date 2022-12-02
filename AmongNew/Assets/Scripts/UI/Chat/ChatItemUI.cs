using UnityEngine;
using UnityEngine.UI;

public class ChatItemUI : MonoBehaviour {

    [SerializeField] private Text _text;
    [SerializeField] private Image _image;
    [SerializeField] public Text _playerName;

    public void Initialize(string text, Color color, string name) {//, string name
        _text.text = text;
        _image.color = color;
        _playerName.text = name;

    }

}