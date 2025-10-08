using System.Collections;
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

    void Awake()
    {
        Instance = this;
        _pbText.text = "PB:00.000";
    }

    public void AddNewTime(float time)
    {
        string s = time.ToString("00.000");
        // print("New Time: " + time + " Last PB: " + _lastPb);
        if (time < _lastPb)
        {
            _lastPb = time;
            _pbText.text = "PB:" + s;
        }
        GameObject obj = Instantiate(_pbPrefab, _pbContainer.transform);
        obj.transform.SetSiblingIndex(0);
        TextMeshProUGUI textMesh = obj.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(TextAnimation(textMesh, s));
        // textMesh.text = "--------\nTime :\n" + s;
    }

    public IEnumerator TextAnimation(TextMeshProUGUI textMesh, string time)
    {
        string toSet = "------\nTime:\n" + time;
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
}