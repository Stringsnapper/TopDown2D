using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    // General cost of movement to node
    public int gCost;

    // Heuristic cost - the estimated distance between the node and the target
    public int hCost;

    public Node parent;
    int heapIndex;

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    // Full cost
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex { 
        get { return heapIndex; } 
        set { heapIndex = value; } 
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
