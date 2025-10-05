using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GridElement _gridPrefab;
    [SerializeField] private Vector2Int _size;
    private GridElement[,] _gridElementArray;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _gridElementArray = new GridElement[_size.x, _size.y];
        _gridElementArray.LoopIn((x, y) =>
        {
            GridElement element = Instantiate(_gridPrefab, transform);
            element.Init(this);
            element.transform.position = new Vector3(x, y, 0);

            _gridElementArray[x, y] = element;
        });
    }


}
