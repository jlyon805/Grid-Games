using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGameManager : MonoBehaviour
{
    private Grid<SnakeObj> grid;
    private bool gameover = true;

    [SerializeField] private int width;
    [SerializeField] private int height;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<SnakeObj>(20, 20, 5f, Vector3.zero,
             (Grid<SnakeObj> g, int x, int y) => new SnakeObj(g, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class SnakeObj
{
    private Grid<SnakeObj> grid;
    int x, y;

    public SnakeObj(Grid<SnakeObj> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}
