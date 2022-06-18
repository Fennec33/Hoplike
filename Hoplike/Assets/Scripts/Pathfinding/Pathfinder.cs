using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Dictionary<HexCoordinate, PathfindingNode> _nodes = new Dictionary<HexCoordinate, PathfindingNode>();
    private List<PathfindingNode> _openList;
    private List<PathfindingNode> _closedList;
    private List<HexTile> _tiles;

    public void GeneratePathfindingNodes()
    {
        _tiles = GridManager.Instance.GetAllHexTiles();

        foreach (HexTile hex in _tiles)
        {
            var temp = new PathfindingNode(hex);
            _nodes.Add(hex.Coordinate, temp);
        }
    }
    
    public List<HexCoordinate> FindPath(HexCoordinate startHex, HexCoordinate goalHex)
    {
        PathfindingNode startNode = null;

        foreach (var hex in _nodes)
        {
            PathfindingNode node = hex.Value;
            node.DistanceToStartNode = int.MaxValue;
            node.cameFromNode = null;
        }
        
        if (_nodes.TryGetValue(startHex, out var nodeS))
        {
            startNode = nodeS;
            startNode.DistanceToStartNode = 0;
            startNode.CalculateDistanceToGoal(goalHex);
        }
        else
        {
            return null;
        }
        
        _openList = new List<PathfindingNode> { startNode };
        _closedList = new List<PathfindingNode>();
        
        while (_openList.Count > 0)
        {
            PathfindingNode currentNode = GetLowestCostNode(_openList);

            if (currentNode.Location == goalHex)
            {
                return CalculatePath(currentNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            List<HexCoordinate> neighborCoordinates = currentNode.Location.GetNeighbors();

            foreach (HexCoordinate hex in neighborCoordinates)
            {
                if (_nodes.TryGetValue(hex, out var testNode))
                {
                    if (_closedList.Contains(testNode)) continue;
                    if (!testNode.IsWalkable())
                    {
                        _closedList.Add(testNode);
                        continue;
                    }

                    if (testNode.DistanceToStartNode > currentNode.DistanceToStartNode + 1)
                    {
                        testNode.cameFromNode = currentNode;
                        testNode.DistanceToStartNode = currentNode.DistanceToStartNode + 1;

                        if (!_openList.Contains(testNode))
                        {
                            _openList.Add(testNode);
                        }
                    }
                }
            }
        }

        return null;
    }

    private PathfindingNode GetLowestCostNode(List<PathfindingNode> testList)
    {
        PathfindingNode lowestCostNode = testList[0];
        for (int i = 1; i < testList.Count; i++)
        {
            if (testList[i].GetNodeCost() < lowestCostNode.GetNodeCost())
            {
                lowestCostNode = testList[i];
            }
        }

        return lowestCostNode;
    }

    private List<HexCoordinate> CalculatePath(PathfindingNode endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        path.Add(endNode);
        PathfindingNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        List<HexCoordinate> coordinatePath = new List<HexCoordinate>();

        foreach (PathfindingNode node in path)
        {
            coordinatePath.Add(node.Location);
        }
        
        return coordinatePath;
    }
}
