using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    static public Pathfinding instance;
    GridNode grid;
    private void Awake()
    {
        grid = GetComponent<GridNode>();
        instance = this;
    }
    public List<Node> FindPath(Vector3 startPoint, Vector3 endPoint)
    {
        List<Node> path = new List<Node>();
        Node startNode = grid.NodeFromWorldPosition(startPoint);
        Node endNode = grid.NodeFromWorldPosition(endPoint);

        List<Node> openNodes = new List<Node>();
        HashSet<Node> closedNodes = new HashSet<Node>();
        openNodes.Add(startNode);

        while(openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];

            //Change current node with the node with lowest fCost, if same, change by the lowest hCost
            for(int i = 1; i < openNodes.Count; i++)
            {
                if(openNodes[i].fCost < currentNode.fCost || openNodes[i].fCost == currentNode.fCost)
                {
                    if(openNodes[i].hCost < currentNode.hCost)
                        currentNode = openNodes[i];
                }
            }
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode == endNode)
            {
                path = RetraceNode(startNode, endNode);
                return path;
            }

            //Search neighbours and calculate hCost and gCost
            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                    continue;
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openNodes.Contains(neighbour))
                        openNodes.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Node>  RetraceNode(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path = SimplifyPath(path);
        path.Reverse();
        grid.path = path;
        return path;
    }

    List<Node> SimplifyPath(List<Node> path)
    {
        List<Node> newPath = new List<Node>();
        Vector2 directionOld = Vector2.zero;
        for(int i = 0; i < path.Count-1; i++)
        {
            Vector2 directionNew = new Vector2(path[i].gridX - path[i+1].gridX, path[i].gridY - path[i+1].gridY);
            if(directionNew != directionOld)
            {
                newPath.Add(path[i]);
            }
            directionOld = directionNew;
        }
        return newPath;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) 
            return (14 * dstY + 10 * (dstX - dstY));

        return (14 * dstX + 10 * (dstY - dstX));
    }
}
