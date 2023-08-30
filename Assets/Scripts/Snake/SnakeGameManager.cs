using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGameManager : MonoBehaviour
{
    private Grid<PathNode> grid;
    private bool gameover = true;

    private int width = 20;
    private int height = 20;

    [SerializeField] private Snake snake;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<PathNode>(width, height, 1f, new Vector3(-(width/2), -(height/2), 0f),
             (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public (int, int) GetGridSize()
    {
        return (grid.GetWidth(), grid.GetHeight());
    }
}
