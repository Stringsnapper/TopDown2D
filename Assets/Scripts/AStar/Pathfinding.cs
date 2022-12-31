using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

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

    PathRequestManager requestManager;
    Grid grid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();

    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        
        if (startNode.walkable && targetNode.walkable)
        {

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
                    // return
                    pathSuccess = true;
                    break;
                }

                // foreach neighbour of the current node
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    // if a neighbour is not traversable or neighbour is in CLOSED
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        // skip to next neighbour
                        continue;
                    }

                    // if new path to neighbour is shorter OR neighbour is not in OPEN
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
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
        yield return null;
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
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
