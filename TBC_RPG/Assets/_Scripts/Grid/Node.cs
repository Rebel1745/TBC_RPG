using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public List<Node> nodeNeighbours;

    // sprite info
    // if the sprite type is changed somewhere else, it doesn't get updated here before the nodes get redrawn
    public NODE_SPRITE_TYPE spriteType;
    public GameObject spriteGO;
    public NODE_SPRITE_TYPE newSpriteType;
    public Quaternion spriteRotation;
    public bool isRedrawSprite = false;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public void UpdateSprite(NODE_SPRITE_TYPE newType, Quaternion rotation)
    {
        isRedrawSprite = true;
        newSpriteType = newType;
        spriteRotation = rotation;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
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

public enum NODE_SPRITE_TYPE
{
    Any, // used for resetting all sprites back to none
    None,
    Available,
    Path
}