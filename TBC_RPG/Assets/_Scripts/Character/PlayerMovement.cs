using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int characterId; // used for DEBUG only
    [SerializeField] CharacterMovement cm;
    [SerializeField] float timeBetweenMousePositionCheck = 0.1f;

    [SerializeField] LayerMask whatIsGround;

    Node nodeUnderMouse = null;

    public event EventHandler OnMouseOverNodeChange;

    private void Start()
    {
        OnMouseOverNodeChange += OnMouseOverNodeChanged;
        //InvokeRepeating("GetCurrentNodeFromMousePosition", 0f, timeBetweenMousePositionCheck);
    }

    void Update()
    {
        if (BattleManager.instance.currentCharacterId != characterId) return;

        GetCurrentNodeFromMousePosition();

        if (BattleManager.instance.battleStatus == BATTLE_STATUS.WaitingForMove)
        {
            CheckForMovementClick();
        }

        if (BattleManager.instance.battleStatus == BATTLE_STATUS.WaitingForTarget)
            CheckForTargetClick();
    }

    void CheckForTargetClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            print(nodeUnderMouse.characterOnNode.name);
            if(nodeUnderMouse.characterOnNode != null && nodeUnderMouse.characterOnNode.GetComponent<PlayerMovement>().characterId != characterId)
            {
                BattleManager.instance.SelectTarget(nodeUnderMouse.characterOnNode);
            }
        }
    }

    void CheckForMovementClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            cm.currentNode = NodeGrid.instance.NodeFromWorldPoint(transform.position);
            transform.position = cm.currentNode.worldPosition;

            if (cm.currentNode != nodeUnderMouse)
            {
                Node[] path = Pathfinding.instance.FindPath(cm.currentNode, nodeUnderMouse);

                if (path != null && path.Length <= cm.MovementDistance)
                {
                    NodeGrid.instance.ResetSprites(NODE_SPRITE_TYPE.Any, NODE_SPRITE_TYPE.None, Quaternion.identity);
                    cm.SetPath(path);
                }
            }
        }
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

            if (cm.currentNode != nodeUnderMouse)
            {
                Node[] path = Pathfinding.instance.FindPath(cm.currentNode, nodeUnderMouse);

                if (path != null && path.Length <= cm.MovementDistance)
                {
                    NodeGrid.instance.ResetSprites(NODE_SPRITE_TYPE.Any, NODE_SPRITE_TYPE.None, Quaternion.identity);
                    cm.SetPath(path);
                }
            }
        }
    }

    public void ShowAvailableMoves()
    {
        cm.currentNode = NodeGrid.instance.NodeFromWorldPoint(transform.position);
        transform.position = cm.currentNode.worldPosition;

        Pathfinding.instance.ShowPossibleMoveNodes(cm.currentNode, cm.MovementDistance);
    }
}
