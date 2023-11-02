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

    [SerializeField] LayerMask whatIsGround;

    void GetCurrentNodeFromMousePosition()
    {
        Node previousNode = nodeUnderMouse;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, whatIsGround))
        {
            nodeUnderMouse = NodeGrid.instance.NodeFromWorldPoint(hitInfo.point);

            if (previousNode != nodeUnderMouse)
                OnMouseOverNodeChange?.Invoke(this, EventArgs.Empty);
        }
    }

    void CheckForMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            cm.currentNode = NodeGrid.instance.NodeFromWorldPoint(transform.position);
            transform.position = cm.currentNode.worldPosition;

            Node endNode = NodeGrid.instance.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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
}
