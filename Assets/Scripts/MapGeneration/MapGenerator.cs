using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _tilePrefab = null;
    [SerializeField] private GameObject _obstaclePrefab = null;

    [Header("Tile Config")]
    [SerializeField] private float _tileSize = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _outline = 0f;
    [SerializeField] private bool _centerAsStartPoint = true;

    [Header("Background Plane")]
    [SerializeField] private Transform _backgroundPlane = null;

    [Header("Maps")]
    [SerializeField] private int _mapIndex = 0;
    [SerializeField] private Map[] _maps = new Map[1];

    private string _generatedMapName = "GeneratedMap";
    private List<Coord> _coords = new List<Coord>();
    private Queue<Coord> _obstacles = null;
    private Queue<Coord> _spawnableTile = null;
    private Transform[,] _allTileTransform = null;

    private Map _currentMap;
    private NavMeshSurface _navMeshSurface;
    private Coord _startPoint;

    //EVENTS
    public Action OnMapGenerated;
    public Action<Vector3> OnValidPlayerPosition;

    #region SINGLETON
    static private MapGenerator _instance = null;
    static public MapGenerator Instance => _instance ? _instance : GameObject.FindObjectOfType<MapGenerator>();
    #endregion

    private void Awake()
    {
        //AssembleMapGeneration();
    }

    public void AssembleMapGeneration()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();
        GenerateMap();
        GenerateBackground();
        GenerateNavMesh();
    }

    public void BeginNewMapGeneration()
    {
        AssembleMapGeneration();
        _mapIndex = (++_mapIndex) % _maps.Length;
    }

    private void GenerateNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }

    private void GenerateBackground()
    {
        _backgroundPlane.localScale = new Vector3(_currentMap.width * _tileSize, 1f, _currentMap.height * _tileSize);
    }

    private void GenerateMap()
    {
        _currentMap = _maps[_mapIndex];
        System.Random rand = new System.Random(_currentMap.seed);
        _allTileTransform = new Transform[_currentMap.width, _currentMap.height];

        // Remove Old Generated Map
        var oldGeneratedMap = transform.Find(_generatedMapName);
        if (oldGeneratedMap != null)
        {
            DestroyImmediate(oldGeneratedMap.gameObject);
            _coords.Clear();
        }

        // Make A New Map
        var generatedMap = new GameObject(_generatedMapName).transform;
        generatedMap.parent = transform;

        for (int x = 0; x < _currentMap.width; ++x)
        {
            for (int y = 0; y < _currentMap.height; ++y)
            {
                var newCoord = new Coord(x, y);
                _coords.Add(newCoord);
                var tile = Instantiate(_tilePrefab, CoordToPosition(newCoord), Quaternion.Euler(90, 0, 0));
                tile.transform.localScale = Vector3.one * (1f - _outline) * _tileSize;
                tile.transform.parent = generatedMap;
                _allTileTransform[x, y] = tile.transform;
            }
        }

        if (_centerAsStartPoint == true)
            _startPoint = _currentMap.mapCenter;
        else
            _startPoint = _coords[UnityEngine.Random.Range(0, _coords.Count)];

        // Add a second List to control open tiles
        List<Coord> openTiles = new List<Coord>(_coords);

        // Add Random Obstacle with caution of not BANNING the PATH from start point
        _obstacles = new Queue<Coord>(Utility.Shuffle(_coords.ToArray(), _currentMap.seed));

        int obstacleCount = (int)(_currentMap.obstanclePercent * (_currentMap.width * _currentMap.height));
        bool[,] isObstacleFalg = new bool[_currentMap.width, _currentMap.height];
        int numberOfObstaclesInMap = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            var newObstacle = GetRandomObstacle();
            ++numberOfObstaclesInMap;
            isObstacleFalg[newObstacle.x, newObstacle.y] = true;
            if (newObstacle != _startPoint && CanPlaceObstacle(isObstacleFalg, numberOfObstaclesInMap))
            {
                var height = Mathf.Lerp(_currentMap.minObstacleHeight, _currentMap.maxObstacleHeight, (float)rand.NextDouble());
                var pos = CoordToPosition(newObstacle) + Vector3.up * height / 2f;
                var obstacle = Instantiate(_obstaclePrefab, pos, Quaternion.identity, generatedMap);
                obstacle.transform.localScale = new Vector3(_tileSize, height, _tileSize);
                Renderer obstacleRenderer = obstacle.GetComponent<Renderer>();
                Material newMaterial = new Material(obstacleRenderer.sharedMaterial);
                newMaterial.color = Color.Lerp(_currentMap.forgroundColor, _currentMap.backgroundColor, (float)rand.NextDouble());
                obstacleRenderer.sharedMaterial = newMaterial;

                // Remove the obstacle from openTiles
                openTiles.Remove(newObstacle);
            }
            else
            {
                --numberOfObstaclesInMap;
                isObstacleFalg[newObstacle.x, newObstacle.y] = false;
            }

            // Make random tiles for spawnning the enemy
            _spawnableTile = new Queue<Coord>(Utility.Shuffle(openTiles.ToArray(), _currentMap.seed));
        }

        OnMapGenerated?.Invoke();
        OnValidPlayerPosition?.Invoke(CoordToPosition(_startPoint));
    }

    public Transform GetTileFromPosition(Vector3 position)
    {
        Coord coord = PositionToCoord(position);
        return _allTileTransform[coord.x, coord.y];
    }

    private bool CanPlaceObstacle(bool[,] isObstacleFlag, int numberOfObstacleInMap)
    {
        bool[,] isCheckedBeforeFlag = new bool[_currentMap.width, _currentMap.height];

        Queue<Coord> coords = new Queue<Coord>();
        coords.Enqueue(_startPoint);
        int numberOfAccessible = 1;

        while (coords.Count > 0)
        {
            var coord = coords.Dequeue();
            isCheckedBeforeFlag[coord.x, coord.y] = true;
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    int newX = coord.x + x;
                    int newY = coord.y + y;
                    if (x == 0 || y == 0)
                    {
                        newX = Mathf.Clamp(newX, 0, _currentMap.width - 1);
                        newY = Mathf.Clamp(newY, 0, _currentMap.height - 1);
                        if (isObstacleFlag[newX, newY] == false && isCheckedBeforeFlag[newX, newY] == false)
                        {
                            coords.Enqueue(new Coord(newX, newY));
                            isCheckedBeforeFlag[newX, newY] = true;
                            ++numberOfAccessible;
                        }
                    }
                }
            }
        }
        return numberOfAccessible == (_currentMap.width * _currentMap.height) - numberOfObstacleInMap;
    }

    private Coord GetRandomObstacle()
    {
        var coord = _obstacles.Dequeue();
        _obstacles.Enqueue(coord);
        return coord;
    }

    public Transform GetRandomOpenTile()
    {
        var coord = _spawnableTile.Dequeue();
        _spawnableTile.Enqueue(coord);
        return _allTileTransform[coord.x, coord.y];
    }

    private Vector3 CoordToPosition(Coord coord)
    {
        return new Vector3(-_currentMap.width / 2f + 0.5f + coord.x, 0f, -_currentMap.height / 2f + 0.5f + coord.y) * _tileSize;
    }

    private Coord PositionToCoord(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x / _tileSize + (_currentMap.width - 1) / 2f);
        int y = Mathf.RoundToInt(pos.z / _tileSize + (_currentMap.height - 1) / 2f);
        x = Mathf.Clamp(x, 0, _currentMap.width - 1);
        y = Mathf.Clamp(y, 0, _currentMap.height - 1);
        return new Coord(x, y);
    }
}
