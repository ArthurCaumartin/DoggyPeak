using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class Grid : MonoBehaviour
{
    [SerializeField] private List<GridElement> _gridPrefabList;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2 _offset;
    [Space]
    [SerializeField] private List<ForcedGridElement> _forcedElementList;
    private GridElement[,] _gridElementArray;

    public Vector2Int Size => _size;
    public GridElement[,] ElementArray => _gridElementArray;


    private void Awake()
    {
        Initialize();
        _gridElementArray.LoopIn((x, y) =>
        {
            _gridElementArray[x, y].EnableObstacle(false);
        });
    }

    public GridElement PickRandomBorderElement()
    {
        List<GridElement> elementList = new List<GridElement>();
        _gridElementArray.LoopIn((x, y) =>
        {
            if ((x == _size.x - 1 && y > 0) || (y == _size.y - 1 && x > 0))
                elementList.Add(_gridElementArray[x, y]);
        });

        return elementList[Random.Range(0, elementList.Count)];
    }

    public void EnableRandomObstacle(int count)
    {
        List<GridElement> elementList = new List<GridElement>();
        _gridElementArray.LoopIn((x, y) =>
        {
            if (x < _size.x - 1 && y < _size.y - 1)
            {
                if ((x >= 0 && y > 2) || (y >= 0 && x > 2))
                    elementList.Add(ElementArray[x, y]);
            }
        });


        _gridElementArray.LoopIn((x, y) =>
        {
            _gridElementArray[x, y].EnableObstacle(false);
        });


        System.Random rng = new System.Random();
        List<GridElement> elements =
        elementList.Cast<GridElement>().OrderBy(_ => rng.Next()).Take(count).ToList();
        elements.ForEach(e => e.EnableObstacle(true));
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
            element.transform.localPosition = new Vector3(x + (_offset.x * x), y + (_offset.y * y), 0);

            _gridElementArray[x, y] = element;
        });
        // transform.position = new Vector3(-_size.x / 2f, -_size.y / 2f, 0);
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
