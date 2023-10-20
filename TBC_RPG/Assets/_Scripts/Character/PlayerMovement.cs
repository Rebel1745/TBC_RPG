using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterMovement cm;

    [SerializeField] Transform target;

    void Update()
    {
        CheckForMouseClick();       
    }

    void CheckForMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Node startNode = NodeGrid.instance.NodeFromWorldPoint(transform.position);
            Node endNode = NodeGrid.instance.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)));
            //Node endNode = NodeGrid.instance.NodeFromWorldPoint(target.position);
            print("Start: (" + startNode.gridX + ", " + startNode.gridY + ") - End: (" + endNode.gridX + ", " + endNode.gridY + ")");
            Node[] path = Pathfinding.instance.FindPath(startNode.worldPosition, endNode.worldPosition);

            if (path != null)
                cm.SetPath(path);
        }
    }
}
