using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    public HexCoordinate Location { get; private set; }
    public HexTile Tile { get; private set; }
    public PathfindingNode cameFromNode;
    
    public int DistanceToStartNode;
    public int EstimatedDistanceToGoal { get; private set; }

    public int GetNodeCost()
    {
        return DistanceToStartNode + EstimatedDistanceToGoal;
    }

    public PathfindingNode(HexTile tile)
    {
        Tile = tile;
        Location = tile.Coordinate;
    }

    public bool IsWalkable()
    {
        return Tile.isWalkable();
    }

    public int CalculateDistanceToGoal(HexCoordinate goal)
    {
        EstimatedDistanceToGoal = Location.DistanceFrom(goal);
        return EstimatedDistanceToGoal;
    }
}
