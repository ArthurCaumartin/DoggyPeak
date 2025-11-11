using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputFieldNameSetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    void Start()
    {
        // _inputField.onEndEdit.AddListener((value) => SetPlayerName());
    }

    public void SetPlayerName()
    {
        if(_inputField.text.Trim() == "")
        {
            return;
        }
        print("Player Name Set To: " + _inputField.text);
        LeaderBoard.currentPlayerName = _inputField.text.ToLower();
        SceneManager.LoadScene("Game");
    }
}