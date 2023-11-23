using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class NodeGrid : MonoBehaviour
{
    public static NodeGrid instance;

    public Transform battleArea;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Object pools for the node sprites
    public ObjectPool<GameObject> availableSpritePool, pathSpritePool, attackSpritePool;
    [SerializeField] Transform nodeSpriteParent;

    // Node sprites
    [SerializeField] GameObject availableNodePrefab;
    [SerializeField] GameObject pathNodePrefab;
    [SerializeField] GameObject attackNodePrefab;

    void Awake()
    {
        instance = this;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        SetNodeNeighbours();
    }

    private void Start()
    {
        Pathfinding.instance.OnNodeSpriteUpdated += OnNodeSpriteUpdated;

        InitialisePools();
    }

    void InitialisePools()
    {
        availableSpritePool = new ObjectPool<GameObject>(() => {
            return Instantiate(availableNodePrefab, nodeSpriteParent);
        }, (obj) => obj.SetActive(true),
        (obj) => obj.SetActive(false),
        (obj) => Destroy(obj), false, 100, 100);

        pathSpritePool = new ObjectPool<GameObject>(() => {
            return Instantiate(pathNodePrefab, nodeSpriteParent);
        }, (obj) => obj.SetActive(true),
        (obj) => obj.SetActive(false),
        (obj) => Destroy(obj), false, 10, 100);

        attackSpritePool = new ObjectPool<GameObject>(() => {
            return Instantiate(attackNodePrefab, nodeSpriteParent);
        }, (obj) => obj.SetActive(true),
        (obj) => obj.SetActive(false),
        (obj) => Destroy(obj), false, 10, 100);
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = (transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2) + battleArea.position;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    void SetNodeNeighbours()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y].SetNeighbours(this, gridSizeX, gridSizeY);
            }
        }
    }

    public Node NodeFromXY(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
            return grid[x, y];
        else
            return null;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        worldPosition -= battleArea.position;
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnNodeSpriteUpdated(object sender, System.EventArgs e)
    {
        RedrawSprites();
    }

    void RedrawSprites()
    {
        Node currentNode;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                currentNode = NodeFromXY(x, y);
                if (currentNode != null && currentNode.isRedrawSprite)
                {
                    UpdateSprite(currentNode);
                }
            }
        }
    }

    void UpdateSprite(Node node)
    {
        // first, release the old spriteGO to the correct pool
        switch (node.spriteType)
        {
            case NODE_SPRITE_TYPE.None:
                break;
            case NODE_SPRITE_TYPE.Available:
                availableSpritePool.Release(node.spriteGO);
                break;
            case NODE_SPRITE_TYPE.Path:
                pathSpritePool.Release(node.spriteGO);
                break;
            case NODE_SPRITE_TYPE.Attack:
                attackSpritePool.Release(node.spriteGO);
                break;
        }

        // the grab the new sprite from the corresponding pool and set its position/rotation
        switch (node.newSpriteType)
        {
            case NODE_SPRITE_TYPE.None:
                break;
            case NODE_SPRITE_TYPE.Available:
                node.spriteGO = availableSpritePool.Get();
                node.spriteGO.transform.position = node.worldPosition;
                break;
            case NODE_SPRITE_TYPE.Path:
                node.spriteGO = pathSpritePool.Get();
                node.spriteGO.transform.position = node.worldPosition;
                node.spriteGO.transform.rotation = node.spriteRotation;
                break;
            case NODE_SPRITE_TYPE.Attack:
                node.spriteGO = attackSpritePool.Get();
                node.spriteGO.transform.position = node.worldPosition;
                break;
        }

        node.spriteType = node.newSpriteType;
        node.isRedrawSprite = false;
    }

    public void ResetSprites(NODE_SPRITE_TYPE from, NODE_SPRITE_TYPE to, Quaternion rotation)
    {
        Node currentNode;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                currentNode = NodeFromXY(x, y);
                if (from == NODE_SPRITE_TYPE.Any)
                {
                    currentNode.UpdateSprite(NODE_SPRITE_TYPE.None, Quaternion.identity);
                }
                else if (currentNode.spriteType == from)
                {
                    currentNode.UpdateSprite(to, rotation);
                }
            }
        }

        RedrawSprites();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(battleArea.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid == null)
            return;

        foreach (Node n in grid)
        {
            Gizmos.color = (n.walkable) ? Color.white : Color.red;
            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
        }
    }
}