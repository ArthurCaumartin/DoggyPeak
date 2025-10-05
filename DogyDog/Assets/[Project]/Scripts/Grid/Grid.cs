using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private List<GridElement> _gridPrefabList;
    [SerializeField] private Vector2Int _size;
    [Space]
    [SerializeField] private List<ForcedGridElement> _forcedElementList;
    private GridElement[,] _gridElementArray;

    public Vector2Int Size => _size;
    public GridElement[,] ElementArray => _gridElementArray;


    private void Start()
    {
        Initialize();
    }

    public bool IsElementFreeToGo(Vector2Int position)
    {
        if (position.x < 0 || position.x >= _size.x || position.y < 0 || position.y >= _size.y)
            return false;
        return !_gridElementArray[position.x, position.y].isBloked;
    }

    private void Initialize()
    {
        _gridElementArray = new GridElement[_size.x, _size.y];
        _gridElementArray.LoopIn((x, y) =>
        {
            GridElement toInstantiate = _gridPrefabList[Random.Range(0, _gridPrefabList.Count)];
            foreach (var item in _forcedElementList)
            {
                if (item.position.x == x && item.position.y == y)
                    toInstantiate = item.element;
            }

            GridElement element = Instantiate(toInstantiate, transform);
            element.Init(this);
            element.transform.position = new Vector3(x, y, 0);

            _gridElementArray[x, y] = element;
        });
        transform.position = -new Vector3(_size.x - 1f, _size.y - 1f, 0) / 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gridElementArray.LoopIn((x, y) =>
            {
                _gridElementArray[x, y].GetComponentInChildren<SpriteRenderer>().color = Color.white;
            });

            GridElement element = GetRandomBorderTopRight();
            element.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }

    public GridElement GetRandomBorderTopRight()
    {
        bool top = Random.value > 0.5f;
        int randomX, randomY;
        if (top)
        {
            randomX = Random.Range(1, _size.x);
            randomY = _size.y - 1;
        }
        else
        {
            randomX = _size.x - 1;
            randomY = Random.Range(1, _size.y);
        }
        print($"top: {top}");
        print($"randomX: {randomX} | randomY: {randomY}");
        return _gridElementArray[randomX, randomY];
    }
}
