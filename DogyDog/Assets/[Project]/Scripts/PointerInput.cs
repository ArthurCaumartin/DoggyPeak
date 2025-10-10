using UnityEngine;
using UnityEngine.EventSystems;

public class PointerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private float _minDragDistance = 50;

    private Vector2 _startPos;
    private Vector2 _endPos;
    private bool _isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _startPos = eventData.position;
        _isDragging = true;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isDragging) return;
        _isDragging = false;
        _endPos = eventData.position;
        Vector2 dragVector = _endPos - _startPos;
        if (dragVector.magnitude < _minDragDistance) return;

        dragVector.Normalize();
        Vector2Int moveDirection = Vector2Int.zero;

        if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
        {
            moveDirection = dragVector.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            moveDirection = dragVector.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        _player.TryMove(moveDirection);
    }
}
