using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    public Transform target; // Set this to the Lich's transform in the Inspector
    public float strikeRange = 2f; // Distance for striking
    public float wanderRadius = 5f; // Range for wandering
    public float moveSpeed = 2f;

    private Vector2[] path;
    private int targetIndex;
    private Animator animator; // For controlling animation states

    private enum State { Stationary, Walking, Striking }
    private State currentState = State.Stationary;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Wander());
    }

    void Update()
    {
        float distanceToLich = Vector2.Distance(transform.position, target.position);

        if (distanceToLich <= strikeRange)
        {
            Strike();
        }
        else if (path != null)
        {
            MoveAlongPath();
        }
        else
        {
            // If no path, start wandering
            if (currentState != State.Walking)
                StartCoroutine(Wander());
        }
    }

    private void Strike()
    {
        currentState = State.Striking;
        animator.SetBool("isStriking", true);
        animator.SetBool("isWalking", false);
    }

    private void MoveAlongPath()
    {
        currentState = State.Walking;
        animator.SetBool("isWalking", true);
        animator.SetBool("isStriking", false);

        if (targetIndex < path.Length)
        {
            Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 targetPos = path[targetIndex];

            transform.position = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(currentPos, targetPos) < 0.1f)
            {
                targetIndex++;
            }
        }
        else
        {
            // Reached destination
            path = null;
            currentState = State.Stationary;
            animator.SetBool("isWalking", false);
        }
    }

    private IEnumerator Wander()
    {
        while (currentState == State.Stationary)
        {
            Debug.Log("Wandering...");  // Check if Wander is running
            Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
            Vector2 wanderDestination = new Vector2(transform.position.x, transform.position.y) + randomDirection;

            path = AStarPathfinding(transform.position, wanderDestination);
            targetIndex = 0;

            if (path.Length > 0)
            {
                Debug.Log("Wander path generated!");  // Path found
            }
            else
            {
                Debug.LogWarning("No path found during wander!");  // No valid path
            }

            yield return new WaitForSeconds(2f); // Wait before wandering again
        }
    }


    private Vector2[] AStarPathfinding(Vector2 start, Vector2 target)
    {
        Debug.Log($"Finding path from {start} to {target}");
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node(start, 0, Heuristic(start, target));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => a.fCost.CompareTo(b.fCost));
            Node currentNode = openList[0];

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.position == target)
            {
                Debug.Log("Path found!");
                return ReconstructPath(currentNode);
            }

            foreach (Node neighbor in GetAdjacentNodes(currentNode, target))
            {
                if (closedList.Contains(neighbor)) continue;

                float tentativeGCost = currentNode.gCost + 1;

                if (tentativeGCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = Heuristic(neighbor.position, target);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        Debug.LogWarning("No valid path found!");
        return new Vector2[0]; // No path found
    }


    private float Heuristic(Vector2 start, Vector2 target)
    {
        return Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y);
    }

    private Vector2[] ReconstructPath(Node targetNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = targetNode;

        while (currentNode != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path.ToArray();
    }

    private List<Node> GetAdjacentNodes(Node currentNode, Vector2 target)
    {
        List<Node> neighbors = new List<Node>();
        Vector2[] directions = {
            new Vector2(1, 0), new Vector2(-1, 0), // Right, Left
            new Vector2(0, 1), new Vector2(0, -1)  // Up, Down
        };

        foreach (Vector2 direction in directions)
        {
            Vector2 neighborPos = currentNode.position + direction;

            if (!CheckForCollision(currentNode.position, neighborPos))
            {
                Node neighbor = new Node(neighborPos, currentNode.gCost + 1, Heuristic(neighborPos, target), currentNode);
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private bool CheckForCollision(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end);

        if (hit.collider != null)
        {
            Debug.Log($"Collision detected with {hit.collider.name} from {start} to {end}");
            if (!hit.collider.CompareTag("Player"))
            {
                return true; // Blocked by an obstacle
            }
        }
        return false; // No collision
    }
}

public class Node
{
    public Vector2 position;
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;
    public Node parent;

    public Node(Vector2 position, float gCost, float hCost, Node parent = null)
    {
        this.position = position;
        this.gCost = gCost;
        this.hCost = hCost;
        this.parent = parent;
    }
}