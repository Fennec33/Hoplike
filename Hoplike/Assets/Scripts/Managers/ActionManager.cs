using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    [SerializeField] private GridItem player;

    enum ActionState
    {
        Move
    }
    
    private void Awake()
    {
        Instance = this;
    }

    public void HexPressed(HexCoordinate hex)
    {
        HexCoordinate zero = new HexCoordinate(0, 0);
        List<HexCoordinate> path = GridManager.Instance.pathfinder.FindPath(zero, hex);

        foreach (HexCoordinate point in path)
        {
            GridManager.Instance.GetHexTileAtCoordinate(point).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
