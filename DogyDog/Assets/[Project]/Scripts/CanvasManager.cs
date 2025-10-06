using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] private RectTransform _pbContainer;
    [SerializeField] private GameObject _pbPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void AddPB(string value)
    {
        GameObject obj = Instantiate(_pbPrefab, _pbContainer.transform);
        obj.transform.SetSiblingIndex(0);
        TextMeshProUGUI textMesh = obj.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = "--------\nTime :\n" + value;
    }
}