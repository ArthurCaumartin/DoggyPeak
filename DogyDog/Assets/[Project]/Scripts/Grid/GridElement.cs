using UnityEngine;

public class GridElement : MonoBehaviour
{
    [SerializeField] private GameObject _obstacleVisualPivot;
    public bool isBloked = false;
    private Grid _grid;

    public void EnableObstacle(bool value)
    {
        isBloked = value;
        if (_obstacleVisualPivot)
            _obstacleVisualPivot.SetActive(value);
    }

    public void Init(Grid grid)
    {
        _grid = grid;
    }
}
