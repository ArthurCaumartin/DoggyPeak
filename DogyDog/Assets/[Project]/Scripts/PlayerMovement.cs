using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Vector2Int _currentPosition;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _stunDuration = 5;
    private Vector2 velPos;
    private bool _canMove = true;

    private void Update()
    {
        Vector2 target = _grid.ElementArray[_currentPosition.x, _currentPosition.y].transform.position;
        transform.position = Vector2.SmoothDamp(transform.position, target, ref velPos, 1 / _speed);

    }

    public void ResetPosition()
    {
        transform.position = _grid.ElementArray[0, 0].transform.position;
    }

    private void TryMove(Vector2Int moveDirection)
    {
        if (!_grid.IsElementFreeToGo(_currentPosition + moveDirection))
        {
            StartCoroutine(Stun(_stunDuration));
            return;
        }
        _currentPosition += moveDirection;
    }

    private void OnMove(InputValue value)
    {
        if (!_canMove) return;
        Vector2 dir = value.Get<Vector2>();
        TryMove(new Vector2Int((int)dir.x, (int)dir.y));
    }

    private IEnumerator Stun(float duration)
    {
        _canMove = false;
        yield return new WaitForSeconds(duration);
        _canMove = true;
    }
}