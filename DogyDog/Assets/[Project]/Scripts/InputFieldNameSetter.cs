using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputFieldNameSetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    void Start()
    {
        _inputField.onEndEdit.AddListener((value) => SetPlayerName());
    }

    public void SetPlayerName()
    {
        print("Player Name Set To: " + _inputField.text);
        LeaderBoard.currentPlayerName = _inputField.text;
        SceneManager.LoadScene("Game");
    }
}