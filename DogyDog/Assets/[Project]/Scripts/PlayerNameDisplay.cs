using TMPro;
using UnityEngine;

public class BpDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _playerBpText;
    public void Update()
    {
        _playerNameText.text = LeaderBoard.currentPlayerName;
        string pbTexte = "00.000";
        if (LeaderBoard.currentPlayerPB < 1000)
            pbTexte = "PB:" + LeaderBoard.currentPlayerPB.ToString("00.000");

        _playerBpText.text = pbTexte;
    }
}