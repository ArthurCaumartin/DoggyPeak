using UnityEngine;

public class GridElement : MonoBehaviour
{
    public bool isBloked = false;
    private Grid _grid;


    public void Init(Grid grid)
    {
        _grid = grid;
    }
}
