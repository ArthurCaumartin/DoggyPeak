using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _timer;
    private bool _isRuning = false;


    public void StartTimer()
    {
        _timer = 0;
        _isRuning = true;
    }

    void Update()
    {
        if (!_isRuning) return;
        _timer += Time.deltaTime;
        _timerText.text = _timer.ToString("00.000");
    }

    public void StopTimer()
    {
        _isRuning = false;
        //TODO post avec API
    }
}