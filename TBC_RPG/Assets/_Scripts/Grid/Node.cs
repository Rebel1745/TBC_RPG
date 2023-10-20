using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public List<Node> nodeNeighbours;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void SetNeighbours(NodeGrid g, int gridSizeX, int gridSizeY)
    {
        List<Node> neighbours = new List<Node>();

        // The neighbours are only nodes up, down, left, and right of the node, does not include diagonals

        // add node to the left
        if (gridX - 1 >= 0)
            neighbours.Add(g.NodeFromXY(gridX - 1, gridY));
        // add node to the right
        if (gridX + 1 < gridSizeX)
            neighbours.Add(g.NodeFromXY(gridX + 1, gridY));
        // add node abovet
        if (gridY + 1 < gridSizeY)
            neighbours.Add(g.NodeFromXY(gridX, gridY + 1));
        // add node below
        if (gridY - 1 >= 0)
            neighbours.Add(g.NodeFromXY(gridX, gridY - 1));

        nodeNeighbours = neighbours;
    }
}