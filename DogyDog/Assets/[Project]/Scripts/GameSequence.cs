using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameSequence : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Ball _ball;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Grid _grid;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        StartSequence();
    }

    private void StartSequence()
    {
        _playerMovement.ResetPosition();
        _ball.Launch(0.2f, () =>
        {
            _grid.EnableRandomObstacle(6);
            _timer.StartTimer();
            _playerMovement.CanMove = true;
        });

    }

    private void EndSequence()
    {
        _timer.StopTimer();
        StartCoroutine(StartDelay(3));
        _playerMovement.CanMove = false;
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
