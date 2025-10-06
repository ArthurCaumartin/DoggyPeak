using UnityEngine;
using DG.Tweening;
using System;

public class GridElement : MonoBehaviour
{
    [SerializeField] private GameObject _obstacleVisualPivot;
    public bool isBloked = false;
    private Grid _grid;

    public void HideObstacle()
    {
        if (_obstacleVisualPivot)
            _obstacleVisualPivot.SetActive(false);
    }

    public void EnableObstacle(bool enableObstacle, Action after = null)
    {
        if (enableObstacle == isBloked)
        {
            after?.Invoke();
            return;
        }

        isBloked = enableObstacle;
        if (_obstacleVisualPivot)
        {
            if (enableObstacle)
                _obstacleVisualPivot.SetActive(true);

            Vector3 targetScale = enableObstacle ? Vector3.one : Vector3.zero;
            _obstacleVisualPivot.transform.DOScale(targetScale, 0.2f)
            .OnComplete(() =>
            {
                if (!enableObstacle)
                {
                    // print($"Disable : {name} / after animation");
                    _obstacleVisualPivot.SetActive(false);
                }

                after?.Invoke();
            });
        }
    }

    public void Init(Grid grid)
    {
        _grid = grid;
    }
}
