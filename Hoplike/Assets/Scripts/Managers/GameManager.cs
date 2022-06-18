using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Pathfinder pathfinder;
    [SerializeField] private LevelGenerator _levelGenerator;
    
    
    
    enum GameState
    {
        GameStart,
        PlayerTurn,
        AnimatePlayerTurn,
        EnemyTurn,
        PostBattle
    }

    private void Start()
    {
        gridManager.GenerateGrid();
        pathfinder.GeneratePathfindingNodes();
        _levelGenerator.GenerateLevel();
    }
}
