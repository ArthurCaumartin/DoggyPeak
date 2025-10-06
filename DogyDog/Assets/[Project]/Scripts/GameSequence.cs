using System.Collections;
using UnityEngine;

public class GameSequence : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Ball _ball;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Grid _grid;

    private IEnumerator Start()
    {
        _playerMovement.CanMove = false;
        _playerMovement.ResetPosition();
        _ball.ResetPosition();
        yield return new WaitForSeconds(1f);
        StartSequence();
    }

    private void StartSequence()
    {
        print("Start Sequence");
        _ball.Launch(1.5f, () =>
        {
            _grid.EnableRandomObstacle(6);
            _timer.StartTimer();
            _playerMovement.CanMove = true;
        });
    }

    public void EndSequence()
    {
        _timer.StopTimer();
        _playerMovement.CanMove = false;
        StartCoroutine(StartDelay(1));
    }

    private IEnumerator StartDelay(float delay)
    {
        for (int x = 0; x < _grid.Size.x; x++)
        {
            for (int y = 0; y < _grid.Size.y; y++)
            {
                if (x == _playerMovement.CurrentPos.x && y == _playerMovement.CurrentPos.y)
                    continue;
                yield return new WaitForSeconds((delay / 2) / (_grid.Size.x * _grid.Size.y));
                _grid.ElementArray[x, y].EnableObstacle(true);
            }
        }

        for (int x = 0; x < _grid.Size.x; x++)
        {
            for (int y = 0; y < _grid.Size.y; y++)
            {
                yield return new WaitForSeconds((delay / 2) / (_grid.Size.x * _grid.Size.y));
                _grid.ElementArray[x, y].EnableObstacle(false);
            }
        }
        _playerMovement.ResetPosition();
        _ball.ResetPosition();
        yield return new WaitForSeconds(0.3f);

        StartSequence();
    }
}
