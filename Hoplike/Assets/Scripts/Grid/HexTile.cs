using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private bool walkable = true;
    

    public GameObject Terrain;
    public GridItem Occupant;
    
    private SpriteRenderer _spriteRenderer;
    public HexCoordinate Coordinate { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _spriteRenderer.color = defaultColor;
    }

    public void SetCoordinate(HexCoordinate coordinate)
    {
        if (GridManager.Instance.GetHexTileAtCoordinate(coordinate) == this)
        {
            Coordinate = coordinate;
        }
    }

    public bool isWalkable()
    {
        return walkable;
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.color = highlightColor;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        ActionManager.Instance.HexPressed(Coordinate);
    }
}
