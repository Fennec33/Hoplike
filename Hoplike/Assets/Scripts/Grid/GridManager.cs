using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    
    [SerializeField] private HexTile tilePrefab;
    public Pathfinder pathfinder;
    

    private readonly HexCoordinate[] _positions = new HexCoordinate[79]
    {
        new HexCoordinate(0, 5),
        new HexCoordinate(0, 4),
        new HexCoordinate(0, 3),
        new HexCoordinate(0, 2),
        new HexCoordinate(0, 1),
        new HexCoordinate(0, 0),
        new HexCoordinate(0, -1),
        new HexCoordinate(0, -2),
        new HexCoordinate(0, -3),
        new HexCoordinate(0, -4),
        new HexCoordinate(0, -5),
        new HexCoordinate(1, 4),
        new HexCoordinate(1, 3),
        new HexCoordinate(1, 2),
        new HexCoordinate(1, 1),
        new HexCoordinate(1, 0),
        new HexCoordinate(1, -1),
        new HexCoordinate(1, -2),
        new HexCoordinate(1, -3),
        new HexCoordinate(1, -4),
        new HexCoordinate(1, -5),
        new HexCoordinate(2, 3),
        new HexCoordinate(2, 2),
        new HexCoordinate(2, 1),
        new HexCoordinate(2, 0),
        new HexCoordinate(2, -1),
        new HexCoordinate(2, -2),
        new HexCoordinate(2, -3),
        new HexCoordinate(2, -4),
        new HexCoordinate(2, -5),
        new HexCoordinate(3, 2),
        new HexCoordinate(3, 1),
        new HexCoordinate(3, 0),
        new HexCoordinate(3, -1),
        new HexCoordinate(3, -2),
        new HexCoordinate(3, -3),
        new HexCoordinate(3, -4),
        new HexCoordinate(3, -5),
        new HexCoordinate(4, 1),
        new HexCoordinate(4, 0),
        new HexCoordinate(4, -1),
        new HexCoordinate(4, -2),
        new HexCoordinate(4, -3),
        new HexCoordinate(4, -4),
        new HexCoordinate(4, -5),
        new HexCoordinate(-1, 5),
        new HexCoordinate(-1, 4),
        new HexCoordinate(-1, 3),
        new HexCoordinate(-1, 2),
        new HexCoordinate(-1, 1),
        new HexCoordinate(-1, 0),
        new HexCoordinate(-1, -1),
        new HexCoordinate(-1, -2),
        new HexCoordinate(-1, -3),
        new HexCoordinate(-1, -4),
        new HexCoordinate(-2, 5),
        new HexCoordinate(-2, 4),
        new HexCoordinate(-2, 3),
        new HexCoordinate(-2, 2),
        new HexCoordinate(-2, 1),
        new HexCoordinate(-2, 0),
        new HexCoordinate(-2, -1),
        new HexCoordinate(-2, -2),
        new HexCoordinate(-2, -3),
        new HexCoordinate(-3, 5),
        new HexCoordinate(-3, 4),
        new HexCoordinate(-3, 3),
        new HexCoordinate(-3, 2),
        new HexCoordinate(-3, 1),
        new HexCoordinate(-3, 0),
        new HexCoordinate(-3, -1),
        new HexCoordinate(-3, -2),
        new HexCoordinate(-4, 5),
        new HexCoordinate(-4, 4),
        new HexCoordinate(-4, 3),
        new HexCoordinate(-4, 2),
        new HexCoordinate(-4, 1),
        new HexCoordinate(-4, 0),
        new HexCoordinate(-4, -1)

    };

    private Dictionary<HexCoordinate, HexTile> _tiles = new Dictionary<HexCoordinate, HexTile>();

    private float _hexSize = 1;
    private float _hexHeight;
    private float _hexWidth;

    private void Awake()
    {
        Instance = this;
        
        _hexHeight = math.sqrt(3) * _hexSize;
        _hexWidth = _hexSize * 2;
    }

    public void GenerateGrid()
    {
        foreach (HexCoordinate position in _positions)
        {
            Vector2 spawnLocation = HexGridCoordinateToWorldPosition(position);
            var spawnedTile = Instantiate(tilePrefab, new Vector3(spawnLocation.x, spawnLocation.y), quaternion.identity);
            spawnedTile.transform.SetParent(transform);
            spawnedTile.transform.name = "Q:" + position.Q.ToString() + " R:" + position.R.ToString();
            _tiles[position] = spawnedTile;
            spawnedTile.SetCoordinate(position);
        }
    }

    public List<HexTile> GetListOfHexTiles(List<HexCoordinate> coordinates)
    {
        List<HexTile> answer = new List<HexTile>();
        HexTile tile;
        
        foreach (HexCoordinate coor in coordinates)
        {
            tile = GetHexTileAtCoordinate(coor);
            
            if (coor == null) { break; }
            
            answer.Add(tile);
        }

        return answer;
    }

    public List<HexTile> GetAllHexTiles()
    {
        List<HexTile> answer = new List<HexTile>();

        foreach (KeyValuePair<HexCoordinate, HexTile> hex in _tiles)
        {
            answer.Add(hex.Value);
        }

        return answer;
    }

    public HexTile GetHexTileAtCoordinate(HexCoordinate key)
    {
        if (_tiles.TryGetValue(key, out var hex))
        {
            return hex;
        }
        else
        {
            return null;
        }
    }
    
    public HexTile GetHexTileAtCoordinate(int q, int r)
    {
        HexCoordinate pos = new HexCoordinate(q, r);
        
        if (_tiles.TryGetValue(pos, out var hex))
        {
            return hex;
        }
        else
        {
            return null;
        }
    }

    public HexCoordinate GetCoordinateOfHex(HexTile tile)
    {
        return _tiles.FirstOrDefault(x => x.Value == tile).Key;
    }

    public Vector2 HexGridCoordinateToWorldPosition(HexCoordinate hex)
    {
        int q = hex.Q;
        int r = hex.R;
        
        float x = q * _hexWidth * 0.75f;
        float y = r * _hexHeight + (q * _hexHeight / 2);

        return new Vector2(x, y);
    }
}
