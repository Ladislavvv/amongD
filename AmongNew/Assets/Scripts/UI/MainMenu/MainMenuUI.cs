using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private string _lobbyScene = "lobby_scene";

    public void OnStartGamePressed() {
        SceneManager.LoadScene(_lobbyScene);
    }

    public void OnQuitPressed() {
        Application.Quit();
    }

}