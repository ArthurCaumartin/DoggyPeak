using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] private RectTransform _pbContainer;
    [SerializeField] private GameObject _pbPrefab;
    [SerializeField] private TextMeshProUGUI _pbText;
    private float _lastPb = 100;
    private char[] _randomChar = new[] { '@', '#', '0', '%', '$' };
    private List<GameObject> _bpList = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        _pbText.text = "PB:00.000";

        LeaderBoard.OnLeaderBoardFetched -= PrintLeaderBoard;
        LeaderBoard.OnLeaderBoardFetched += PrintLeaderBoard;
        LeaderBoard.FetchLeaderBoard();
    }

    public void AddNewTime(Player player)
    {
        string s = player.player + "\n" + player.time.ToString("00.000");
        // print("New Time: " + time + " Last PB: " + _lastPb);
        if (player.time < _lastPb)
        {
            _lastPb = player.time;
            _pbText.text = "PB:" + s;
            AudioManager.Instance.PlaySound(AudioManager.Instance.GoodBoySound);
        }
        GameObject obj = Instantiate(_pbPrefab, _pbContainer.transform);
        obj.transform.SetSiblingIndex(0);
        TextMeshProUGUI textMesh = obj.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(TextAnimation(textMesh, s));
        // textMesh.text = "--------\nTime :\n" + s;
    }

    public void PrintLeaderBoard(Player[] playerArray)
    {
        foreach (var item in _bpList)
            Destroy(item);
        _bpList.Clear();
        for (int i = 0; i < playerArray.Length; i++)
        {
            GameObject newPb = Instantiate(_pbPrefab, _pbContainer.transform);
            _bpList.Add(newPb);
            string content = playerArray[i].player + "\n" + "T : " + playerArray[i].time.ToString("00.000");
            StartCoroutine(TextAnimation(newPb.GetComponentInChildren<TextMeshProUGUI>(), content));
        }
    }

    public IEnumerator TextAnimation(TextMeshProUGUI textMesh, string content)
    {
        string toSet = "---------\n" + content;
        string random = "";
        for (int i = 0; i < toSet.Length; i++)
        {
            if (toSet[i] == '\n')
            {
                random += '\n';
                continue;
            }
            random += _randomChar[Random.Range(0, _randomChar.Length)];
            textMesh.text = random;
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < toSet.Length; i++)
        {
            char[] chars = textMesh.text.ToCharArray();
            chars[i] = toSet[i];
            for (int j = i + 1; j < toSet.Length; j++)
            {
                if (toSet[j] == '\n')
                {
                    chars[j] = '\n';
                    continue;
                }
                chars[j] = _randomChar[Random.Range(0, _randomChar.Length)];
            }
            textMesh.text = new string(chars);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void OnDestroy()
    {
        LeaderBoard.OnLeaderBoardFetched -= PrintLeaderBoard;
    }
}
