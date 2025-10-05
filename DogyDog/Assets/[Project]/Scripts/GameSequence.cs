using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameSequence : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Ball _ball;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Grid _grid;

    private void Start()
    {
        StartSequence();
    }

    private void StartSequence()
    {
        _playerMovement.ResetPosition();
        _ball.Launch(() =>
        {
            _grid.EnableRandomObstacle(6);
            _timer.StartTimer();
        });

    }

    private void EndSequence()
    {
        _timer.StopTimer();
        StartCoroutine(StartDelay(3));
    }

    void OnEnable()
    {
        Ball.OnBallGrab += EndSequence;
    }

    void OnDisable()
    {
        Ball.OnBallGrab -= EndSequence;
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartSequence();
    }
}
