using UnityEngine;
using System;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    NodeGrid grid;

    public event EventHandler OnNodeSpriteUpdated;

    void Awake()
    {
        instance = this;
        grid = GetComponent<NodeGrid>();
    }

    public void DrawPathSprites(Node startNode, Node endNode)
    {
        // first remove any sprites that may already be created for an old path
        RemovePathNodes();

        // get the path
        Node[] path = FindPath(startNode, endNode);
        Quaternion rotation = Quaternion.identity;
        Node previousNode = startNode;

        // update the nodes along the path to the path sprite
        if (path != null && path.Length > 0)
        {
            for (int i = 0; i < path.Length; i++)
            {
                rotation = GetAngleOfRotationBetweenTwoNodes(previousNode, path[i]);
                path[i].UpdateSprite(NODE_SPRITE_TYPE.Path, rotation);

                previousNode = path[i];
            }

            // once all of the nodes have been set to the path sprite, trigger the redraw
            OnNodeSpriteUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    Quaternion GetAngleOfRotationBetweenTwoNodes(Node nodeA, Node nodeB)
    {
        if (nodeB.gridX - nodeA.gridX > 0) return Quaternion.Euler(0f, 90f, 0f);
        if (nodeB.gridX - nodeA.gridX < 0) return Quaternion.Euler(0f, 270f, 0f);
        if (nodeB.gridY - nodeA.gridY < 0) return Quaternion.Euler(0f, 180f, 0f);

        return Quaternion.identity;
    }

    public void RemovePathNodes()
    {
        grid.ResetSprites(NODE_SPRITE_TYPE.Path, NODE_SPRITE_TYPE.Available, Quaternion.identity);
    }

    public bool IsPathToTargetNodeValid(Node startNode, Node endNode)
    {
        Node[] path = FindPath(startNode, endNode);

        if (path != null && path.Length > 0)
            return true;
        else
            return false;
    }

    public void RemovePossibleMoveNodes()
    {
        grid.ResetSprites(NODE_SPRITE_TYPE.Available, NODE_SPRITE_TYPE.None, Quaternion.identity);
    }

    public void ShowPossibleMoveNodes(Node startNode, int distance)
    {
        Node endNode;

        RemovePossibleMoveNodes();

        for (int x = -distance; x <= distance; x++)
        {
            for (int y = -distance; y <= distance; y++)
            {
                endNode = grid.NodeFromXY(startNode.gridX + x, startNode.gridY + y);

                // if there is no valid endNode, no point checking further
                if (endNode == null) continue;

                // if x and y are zero, it is our starting position, not to be included
                if (startNode != endNode)
                {
                    //print(endPos);
                    if (endNode.walkable && IsPathToTargetNodeValid(startNode, endNode))
                    {
                        if (FindPath(startNode, endNode).Length <= distance)
                        {
                            endNode.UpdateSprite(NODE_SPRITE_TYPE.Available, Quaternion.identity);
                        }
                    }
                }
            }
        }

        OnNodeSpriteUpdated?.Invoke(this, EventArgs.Empty);
    }

    public Node[] FindPath(Node startNode, Node endNode)
    {
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                Node[] path = RetracePath(startNode, endNode);
                return path;
            }

            foreach (Node neighbour in currentNode.nodeNeighbours)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    public Node[] FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(endPos);

        return FindPath(startNode, endNode);
    }

    public Node[] FindPath(Node startNode, Vector3 endPos)
    {
        Node endNode = grid.NodeFromWorldPoint(endPos);

        return FindPath(startNode, endNode);
    }

    Node[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 10 * dstY + 10 * (dstX - dstY);
        return 10 * dstX + 10 * (dstY - dstX);
    }

    int GetDistanceIncludeDiagonals(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}