using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    int targetIndex;
    float speed = 10;

    [SerializeField] int maxMovementDistance = 3;
    public int MovementDistance { get { return maxMovementDistance; } }

    Node[] path;
    bool isMoving;
    Vector3 currentWaypoint;

    public Node currentNode;

    private void Start()
    {
        currentWaypoint = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        //FollowPath();
    }

    public void SetPath(Node[] waypoints)
    {
        isMoving = true;
        path = waypoints;
        targetIndex = 0;
        currentWaypoint = path[0].worldPosition;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    isMoving = false;
                    transform.position = currentWaypoint;
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
