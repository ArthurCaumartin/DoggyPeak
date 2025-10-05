using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    private GirdElement[] _gridElementArray;


    private void Start()
    {

    }

    private void Initialize()
    {

    }


}


public class GirdElement : MonoBehaviour
{
    private Grid _grid;





    public void Init(Grid grid)
    {
        _grid = grid;
    }
}





// public static class A