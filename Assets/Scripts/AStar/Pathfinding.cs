using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    // -------------- A* algorithm ---------------
    // https://www.youtube.com/watch?v=mZfyt03LDH4&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=3
    //
    // OPEN set of nodes to be evaluated
    // CLOSED set of node already evaluated

    // loop
    //  current = node in OPEN with the lowest f_cost
    //  remove current from OPEN
    //  add current to CLOSED

    //  if current is the target node
    //      return
    //  foreach neighbour of the current node
    //      if a neighbour is not traversable or neighbour is in CLOSED
    //          skip to next neighbour
    //      if new path to neighbour is shorter OR neighbour is not in OPEN
    //          set f_cost of neighbour
    //          set parent of neighbour
    //          if neighbour is not in OPEN
    //              add neighbour to OPEN

    // -------------------------------------------
    // Pythagoras * 10 
    // Each Horizontal or Vertical move costs 10
    // Each diagonal move costs 14
    //
    // if x > y
    //  14y + 10(x-y)
    // if x < y
    //  14x + 10(y-x)
    // -------------------------------------------

    public Transform seeker, target;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }
    void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        // OPEN set of nodes to be evaluated
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);

        // CLOSED set of node already evaluated
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        // loop
        while (openSet.Count > 0)
        {
            // current = node in OPEN with the lowest f_cost
            // lowest fCost or lowest hCost if fCost is same
            // remove current from OPEN
            Node currentNode = openSet.RemoveFirst();

            // add current to CLOSED
            closedSet.Add(currentNode);

            // if current is the target node
            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + "ms");
                RetracePath(startNode, targetNode);
                // return
                return;
            }

            // foreach neighbour of the current node
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // if a neighbour is not traversable or neighbour is in CLOSED
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    // skip to next neighbour
                    continue;
                }

                // if new path to neighbour is shorter OR neighbour is not in OPEN
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // set f_cost of neighbour
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(currentNode, targetNode);

                    // set parent of neighbour
                    neighbour.parent = currentNode;

                    // if neighbour is not in OPEN
                    if (!openSet.Contains(neighbour))
                    {
                        // add neighbour to OPEN
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        // -------------------------------------------
        // Pythagoras * 10 
        // Each Horizontal or Vertical move costs 10
        // Each diagonal move costs 14
        //
        // if x > y
        //  14y + 10(x-y)
        // if x < y
        //  14x + 10(y-x)
        // -------------------------------------------

        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10*(distX - distY);
        }
        return 14 * distX + 10*(distY - distX);

    }
}
