using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameSequence _sequence;
    [Space]
    [SerializeField] private Sprite _baseSprite;
    [SerializeField] private Sprite _grabBallSprite;
    [SerializeField] private SpriteRenderer _sRenderer;
    [SerializeField] private Transform _ballPivot;
    [Space]
    [SerializeField] private Grid _grid;
    [SerializeField] private Vector2Int _currentPosition;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _rotationSpeed = 5;
    [SerializeField] private float _stunDuration = 5;
    private Vector2 velPos;
    private bool _canMove = true;

    private Vector2 _targetDirection;
    private bool _canTriggerEnd = false;

    public bool CanMove { get => _canMove; set => _canMove = value; }
    public Vector2Int CurrentPos => _currentPosition;

    private void Update()
    {
        if (!_canMove) return;
        Vector2 target = _grid.ElementArray[_currentPosition.x, _currentPosition.y].transform.position;
        transform.position = Vector2.SmoothDamp(transform.position, target, ref velPos, 1 / _speed);

        float angle = -Vector2.SignedAngle(_targetDirection, Vector2.right);
        float lerpAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * _rotationSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, lerpAngle);
    }

    public void ResetPosition()
    {
        _sRenderer.sprite = _baseSprite;
        _canTriggerEnd = false;
        _currentPosition = Vector2Int.zero;
        transform.position = _grid.ElementArray[0, 0].transform.position;
    }

    public void TryMove(Vector2Int moveDirection)
    {
        if (!_canMove) return;
        if (!_grid.IsElementFreeToGo(_currentPosition + moveDirection))
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.BlockSound);

            // StartCoroutine(Stun(_stunDuration));
            _canMove = false;
            Vector2 target = (Vector2)transform.position + ((Vector2)moveDirection / 2);
            // print($"target = P {transform.position} +  I {moveDirection} = {target}");
            float duration = Mathf.Clamp(_stunDuration - 0.1f, 0, 100);
            transform.DOMove(target, duration / 2)
            .OnComplete(() =>
            {
                target = _grid.ElementArray[_currentPosition.x, _currentPosition.y].transform.position;
                transform.DOMove(target, duration / 2)
                .OnComplete(() => _canMove = true);
            });

            return;
        }
        AudioManager.Instance.PlaySound(AudioManager.Instance.BarkClip);
        _currentPosition += moveDirection;
        if (moveDirection != Vector2Int.zero)
            _targetDirection = new Vector2(moveDirection.x, moveDirection.y);
    }

    private void OnMove(InputValue value)
    {
        if (!_canMove) return;
        Vector2 dir = value.Get<Vector2>();
        TryMove(new Vector2Int((int)dir.x, (int)dir.y));
    }

    public IEnumerator Stun(float duration)
    {
        _canMove = false;
        yield return new WaitForSeconds(duration);
        _canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.GrabSound);
            _sRenderer.sprite = _grabBallSprite;
            ball.transform.SetParent(_ballPivot);
            ball.transform.localPosition = Vector3.zero;
            ball.SetSprite(false);
            _canTriggerEnd = true;
            return;
        }

        if (other.tag == "EndTrigger" && _canTriggerEnd)
        {
            _canTriggerEnd = false;
            _sequence.EndSequence();
        }
    }

}
