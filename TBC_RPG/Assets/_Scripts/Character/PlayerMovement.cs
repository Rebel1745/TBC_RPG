using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterMovement cm;
    [SerializeField] float timeBetweenMousePositionCheck = 0.1f;
    Node nodeUnderMouse = null;

    public event EventHandler OnMouseOverNodeChange;

    private void Start()
    {
        OnMouseOverNodeChange += OnMouseOverNodeChanged;
        InvokeRepeating("GetCurrentNodeFromMousePosition", 0f, timeBetweenMousePositionCheck);
    }

    void Update()
    {
        CheckForMouseClick();
    }

    void OnMouseOverNodeChanged(object sender, System.EventArgs e)
    {
        if (nodeUnderMouse == null) return;

        if (nodeUnderMouse.spriteType == NODE_SPRITE_TYPE.Available && cm.currentNode != nodeUnderMouse)
            Pathfinding.instance.DrawPathSprites(cm.currentNode, nodeUnderMouse);
        else
            Pathfinding.instance.RemovePathNodes();
    }

    void GetCurrentNodeFromMousePosition()
    {
        Node previousNode = nodeUnderMouse;
        nodeUnderMouse = NodeGrid.instance.NodeFromWorldPoint(GetMouseWorldPosition());

        if (previousNode != nodeUnderMouse)
            OnMouseOverNodeChange?.Invoke(this, EventArgs.Empty);
    }

    void CheckForMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            cm.currentNode = NodeGrid.instance.NodeFromWorldPoint(transform.position);
            transform.position = cm.currentNode.worldPosition;

            Node endNode = NodeGrid.instance.NodeFromWorldPoint(GetMouseWorldPosition());

            if (cm.currentNode != endNode)
            {
                Node[] path = Pathfinding.instance.FindPath(cm.currentNode, endNode);

                if (path != null && path.Length <= cm.MovementDistance)
                {
                    NodeGrid.instance.ResetSprites(NODE_SPRITE_TYPE.Any, NODE_SPRITE_TYPE.None, Quaternion.identity);
                    cm.SetPath(path);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            cm.currentNode = NodeGrid.instance.NodeFromWorldPoint(transform.position);
            transform.position = cm.currentNode.worldPosition;

            Pathfinding.instance.ShowPossibleMoveNodes(cm.currentNode, cm.MovementDistance);
        }
    }

    // testing stuff for now
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
