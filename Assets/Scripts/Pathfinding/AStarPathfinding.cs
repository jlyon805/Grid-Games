using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static AStarPathfinding Instance { get; private set; }

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public AStarPathfinding(int width, int height)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero,
                  (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }
    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        grid.GetCartesianPosition(startPosition, out int startX, out int startY);
        grid.GetCartesianPosition(endPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
            return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (PathNode node in path)
                vectorPath.Add(new Vector3(node.x, node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode node = grid.GetGridObject(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.prevNode = null;
            }

        startNode.gCost = 0;
        startNode.hCost = CalculateDist(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestCostNode(openList);
            if (currentNode == endNode)
            {
                //destination reached
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDist(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.prevNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDist(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }

        //out of Nodes in openList, no path found
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode node)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (node.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(node.x - 1, node.y));
            if (node.y - 1 >= 0) neighbourList.Add(GetNode(node.x - 1, node.y - 1));
            if (node.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(node.x - 1, node.y + 1));
        }
        if (node.x + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(node.x + 1, node.y));
            if (node.y - 1 >= 0) neighbourList.Add(GetNode(node.x + 1, node.y - 1));
            if (node.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(node.x + 1, node.y + 1));
        }
        if (node.y - 1 >= 0) neighbourList.Add(GetNode(node.x, node.y - 1));
        if (node.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(node.x, node.y + 1));

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.prevNode != null)
        {
            path.Add(currentNode.prevNode);
            currentNode = currentNode.prevNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDist(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * remaining;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private PathNode GetLowestCostNode(List<PathNode> nodeList)
    {
        PathNode lowestCostNode = nodeList[0];

        for (int i = 1; i < nodeList.Count; i++)
            if (nodeList[i].fCost < lowestCostNode.fCost)
                lowestCostNode = nodeList[i];

        return lowestCostNode;
    }
}
