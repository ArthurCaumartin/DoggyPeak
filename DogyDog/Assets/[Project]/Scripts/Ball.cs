
using System;
using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public delegate void BallEvent();
    public static BallEvent OnBallGrab;
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private Grid _grid;
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void Launch(float animDuration, Action toToAfter)
    {
        float duration = animDuration;
        Vector3 startPos = transform.position;
        Vector2 targetPos = _grid.PickRandomBorderElement().transform.position;

        transform.DOScale(Vector3.one * 1.5f, duration / 2)
        .OnComplete(() => transform.DOScale(Vector3.one, duration / 2));

        DOTween.To((time) =>
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time);
        }, 0, 1, duration)
        .OnComplete(() =>
        {
            _collider.enabled = true;
            toToAfter?.Invoke();
        });
    }

    public void ResetPosition()
    {
        _collider.enabled = false;
        transform.position = _grid.ElementArray[0, 0].transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("BAAAAAAAAAAAAAAAAAAAA");
        PlayerMovement p = other.GetComponent<PlayerMovement>();
        if (p)
        {
            OnBallGrab.Invoke();
        }
    }
}