using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Node[,] grid;
    public bool showNodes;
    public float nodeRadius;
    public Vector3 gridWorldSize;
    int gridSizeX, gridSizeY;
    Vector3 worldBottomLeft;
    float nodeDiameter;
    public Transform player;
    Node playerNode;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }

    private void Update()
    {
        playerNode = NodeFromWorldPosition(player.position);
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = ((worldPosition.x - transform.position.x) + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = ((worldPosition.y - transform.position.y) + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

        return grid[x, y];
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPosition = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapBox(worldPosition, new Vector2(nodeDiameter - .1f, nodeDiameter - .1f),0, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPosition, x, y);
            }
        }
    }
    

    public List<Node> path;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
        if(grid != null)
        {
            if(path != null)
            {
                if (showNodes)
                {
                    foreach (Node n in grid)
                    {
                        if (n.walkable)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                        else
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                    }
                }
                
                Gizmos.color = Color.black;
                foreach (Node n in path)
                {
                    if (path.IndexOf(n) != 0)
                    {
                        Gizmos.DrawLine(n.worldPosition, path[path.IndexOf(n) - 1].worldPosition);
                    }

                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
            else
            {
                if (showNodes)
                {
                    foreach (Node n in grid)
                    {
                        if (n == playerNode)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                        else if (n.walkable)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                        else
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                    }
                }
                
            }
            
        }
    }  
}