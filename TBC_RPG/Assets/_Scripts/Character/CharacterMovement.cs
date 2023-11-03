using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    int targetIndex;

    [SerializeField] int maxMovementDistance = 3;
    public int MovementDistance { get { return maxMovementDistance; } }

    Node[] path;
    bool isMoving;
    Vector3 currentWaypoint;

    public Node currentNode;

    // animations
    [SerializeField] Animator anim;
    [SerializeField] string runAnimation;
    [SerializeField] string idleAnimation;
    [SerializeField] float moveSpeed = 5f;

    public AttackController ac;

    private void Start()
    {
        currentWaypoint = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
            return;
    }

    public void SetPath(Node[] waypoints)
    {
        // set the current node as walkable
        NodeGrid.instance.NodeFromWorldPoint(transform.position).SetNodeCharacter(null, true);
        isMoving = true;
        path = waypoints;
        targetIndex = 0;
        currentWaypoint = path[0].worldPosition;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
        anim.Play(runAnimation);
    }

    IEnumerator FollowPath()
    {
        Vector3 nextWaypoint;

        while (true)
        {
            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                targetIndex++;
                // is this the end of the path?
                if (targetIndex >= path.Length)
                {
                    // stop the character from moving
                    isMoving = false;
                    // finalise the position
                    transform.position = currentWaypoint;
                    // set the animation back to idle
                    anim.Play(idleAnimation);
                    // make the node the character is currently on unwalkable so other characters can't move to the same node
                    path[targetIndex-1].SetNodeCharacter(this.gameObject, false);
                    // we have moved, change the status to wait for an attack
                    BattleManager.instance.ChangeBattleStatus(BATTLE_STATUS.WaitingForAttackSelection);
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            // if we aren't at the end of the path, look at the next waypoint
            if (targetIndex + 1 < path.Length)
            {
                nextWaypoint = path[targetIndex + 1].worldPosition;
                transform.LookAt(new Vector3(nextWaypoint.x, transform.position.y, nextWaypoint.z));
            }

            // move it
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
