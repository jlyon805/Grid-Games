using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTest : MonoBehaviour
{
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private PathfindingMovement characterPathfinding;
    AStarPathfinding pathfinding;
    private void Start()
    {
        pathfinding = new AStarPathfinding(10, 10);
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Utilities.GetMouseWorldPosition();

            pathfinding.GetGrid().GetCartesianPosition(position, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
                for (int i = 0; i < path.Count - 1; i++)
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 1f);
            characterPathfinding.SetTargetPosition(position);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position = Utilities.GetMouseWorldPosition();

            pathfinding.GetGrid().GetCartesianPosition(position, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
}
