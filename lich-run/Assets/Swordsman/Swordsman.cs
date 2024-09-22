using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 position; // The x,y coordinates of this node
    public float gCost; // Distance from the start node
    public float hCost; // Estimated distance to the target (heuristic)
    public float fCost => gCost + hCost; // Total cost (g + h)
    public Node parent; // Reference to the parent node for backtracking

    public Node(Vector2 position, float gCost, float hCost, Node parent = null)
    {
        this.position = position;
        this.gCost = gCost;
        this.hCost = hCost;
        this.parent = parent;
    }
}

public class SoldierAI : MonoBehaviour
{
    public Transform target; // Set this to the lich's transform
    public float strikeRange = 2f; // Distance for striking
    public float wanderRadius = 5f; // Range for wandering
    public float moveSpeed = 2f;

    private Vector2[] path;
    private int targetIndex;
    private Animator animator; // For controlling the animation states

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
            Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
            Vector2 wanderDestination = new Vector2(transform.position.x, transform.position.y) + randomDirection;

            path = FindPath(transform.position, wanderDestination);
            targetIndex = 0;

            yield return new WaitForSeconds(2f); // Wait before wandering again
        }
    }

    private Vector2[] FindPath(Vector2 startPos, Vector2 targetPos)
    {
        // Implement A* pathfinding logic here using the pseudocode provided
        // This function should return a list of Vector2 waypoints forming the path
        return AStarPathfinding(startPos, targetPos);
    }

    private Vector2[] AStarPathfinding(Vector2 start, Vector2 target)
    {
        // Initialize open and closed lists
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        // Add the start node to the open list
        Node startNode = new Node(start, 0, Heuristic(start, target));
        openList.Add(startNode);

        // While there are nodes to evaluate
        while (openList.Count > 0)
        {
            // Sort the open list by fCost (or find the node with the lowest fCost)
            openList.Sort((a, b) => a.fCost.CompareTo(b.fCost));
            Node currentNode = openList[0];

            // Remove current node from open list and add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If we have reached the target, reconstruct the path
            if (currentNode.position == target)
            {
                return ReconstructPath(currentNode);
            }

            // Evaluate adjacent nodes
            foreach (Node neighbor in GetAdjacentNodes(currentNode, target))
            {
                // Skip if neighbor is in closed list
                if (closedList.Contains(neighbor)) continue;

                float tentativeGCost = currentNode.gCost + 1; // g(n) is always +1 for adjacent nodes

                // Check if this path to the neighbor is better
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

        // Return an empty array if no path was found
        return new Vector2[0];
    }

    // Heuristic: Using Manhattan distance (city block distance) for the heuristic
    private float Heuristic(Vector2 start, Vector2 target)
    {
        return Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y);
    }

    // Reconstruct the path by backtracking from the target node to the start node
    private Vector2[] ReconstructPath(Node targetNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = targetNode;

        // Backtrack through the parent nodes to reconstruct the path
        while (currentNode != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        // Reverse the path to go from start to target
        path.Reverse();
        return path.ToArray();
    }

    // Get the valid adjacent nodes for a given node
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

            // Check if the neighbor is a valid position (add collision detection here)
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
        return hit.collider != null && !hit.collider.CompareTag("Player");
    }
}