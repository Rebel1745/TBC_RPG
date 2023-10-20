using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    int targetIndex;
    float speed = 10;

    Node[] path;
    bool isMoving;
    Vector3 currentWaypoint;

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
        print(waypoints.Length);
        isMoving = true;
        path = new Node[waypoints.Length];
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
            if(Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    isMoving = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
                Gizmos.color = Color.black;
            foreach (Node n in path)
            {
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (5 - .1f));
            }
        }
    }
}
