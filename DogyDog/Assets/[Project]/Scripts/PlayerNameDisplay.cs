using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNameText;
    public void Start()
    {
        _playerNameText.text = LeaderBoard.currentPlayerName;
    }
}