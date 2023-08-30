using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    private Grid<LifeCell> grid;
    private bool running = false;

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private GameOfLifeVisual lifeVisual;

    private float runningTime;
    private float runningTimeMax;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<LifeCell>(width, height, 5f, Vector3.zero,
             (Grid<LifeCell> g, int x, int y) => new LifeCell(g, x, y));

        lifeVisual.SetGrid(grid);
        runningTime = .25f;
        runningTimeMax = runningTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Utilities.GetMouseWorldPosition();
            LifeCell cell = grid.GetGridObject(position);
            if(cell != null)
                cell.SetAlive();
        }

        runningTime += Time.deltaTime;

        if (running && (runningTime >= runningTimeMax))
        {
            UpdateFrame();
            runningTime -= runningTimeMax;
        }
    }

    public void UpdateFrame()
    {
        bool[,] nextState = new bool[width, height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                LifeCell cell = grid.GetGridObject(x, y);
                int neighbours = cell.GetNeighbours();

                if (!(nextState[x,y] = cell.IsAlive()) && neighbours == 0)
                    continue;

                if (cell.IsAlive())
                {
                    if (neighbours < 2 || neighbours > 3)
                        nextState[x, y] = false;
                }
                else
                {
                    if (neighbours == 3)
                        nextState[x, y] = true;
                }
            }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                LifeCell cell = grid.GetGridObject(x, y);
                if (cell.IsAlive() != nextState[x, y])
                    cell.SetAlive();
            }
        }
    }

    public ref Grid<LifeCell> GetGrid()
    {
        return ref grid;
    }

    public void StartLife()
    {
        running = !running;
    }

    public void PopulateRandom()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (Random.value >= 0.5) grid.GetGridObject(x, y).SetAlive();
    }

    public void Clear()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid.GetGridObject(x, y).Death();
    }
}

public class LifeCell
{
    private Grid<LifeCell> grid;
    private byte status; // ###NNNA -- #: Unused, N: number of neighbours, A: alive/dead

    private int x, y;

    public LifeCell(Grid<LifeCell> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        status = 0;
    }

    public void SetAlive()
    {
        if (IsAlive())
            Death();
        else
            Birth();
    }

    public void Death()
    {
        LifeCell[] neighbours = new LifeCell[8];
        //int xLeft, xRight, yUp, yDown;
        //int w = grid.GetWidth();
        //int h = grid.GetHeight();

        /*  xLeft = (x == 0) ? 0 : -1;
            xRight = (x == w - 1) ? 0 : 1;
            yUp = (y == h - 1) ? 0 : 1;
            yDown = (y == 0) ? 0 : -1;

            neighbours.Add(grid.GetGridObject(x + xLeft, y + yDown));
            neighbours.Add(grid.GetGridObject(x + xLeft, y));
            neighbours.Add(grid.GetGridObject(x + xLeft, y + yUp));
            neighbours.Add(grid.GetGridObject(x, y + yDown));
            neighbours.Add(grid.GetGridObject(x, y + yUp));
            neighbours.Add(grid.GetGridObject(x + xRight, y + yDown));
            neighbours.Add(grid.GetGridObject(x + xRight, y));
            neighbours.Add(grid.GetGridObject(x + xRight, y + yUp));  */

        neighbours[0] = grid.GetGridObject(x - 1, y + 1);
        neighbours[1] = grid.GetGridObject(x, y + 1);
        neighbours[2] = grid.GetGridObject(x + 1, y + 1);
        neighbours[3] = grid.GetGridObject(x - 1, y);
        neighbours[4] = grid.GetGridObject(x + 1, y);
        neighbours[5] = grid.GetGridObject(x - 1, y - 1);
        neighbours[6] = grid.GetGridObject(x, y - 1);
        neighbours[7] = grid.GetGridObject(x + 1, y - 1);

        status &= 0x0E;

        foreach (LifeCell neighbour in neighbours)
            if (neighbour != null)
                neighbour.status -= 0x02;

        grid.TriggerGridObjectChanged(x, y);
    }

    public void Birth()
    {
        LifeCell[] neighbours = new LifeCell[8];

        neighbours[0] = grid.GetGridObject(x - 1, y + 1);
        neighbours[1] = grid.GetGridObject(x, y + 1);
        neighbours[2] = grid.GetGridObject(x + 1, y + 1);
        neighbours[3] = grid.GetGridObject(x - 1, y);
        neighbours[4] = grid.GetGridObject(x + 1, y);
        neighbours[5] = grid.GetGridObject(x - 1, y - 1);
        neighbours[6] = grid.GetGridObject(x, y - 1);
        neighbours[7] = grid.GetGridObject(x + 1, y - 1);

        status |= 0x01;

        foreach (LifeCell neighbour in neighbours)
            if (neighbour != null)
                neighbour.status += 0x02;

        grid.TriggerGridObjectChanged(x, y);
    }

    public bool IsAlive() { return ((status & 0x01) == 1); }

    public override string ToString()
    {
        return IsAlive() ? "Alive" : "Dead";
    }

    public char ToPlaintextChar()
    {
        return IsAlive() ? 'O' : '.';
    }

    public int ToInt()
    {
        return status & 0x01;
    }

    public int GetNeighbours()
    {
        return status >> 1;
    }

    private void IncNeighbours()
    {
        status += 0x02;
    }

    private void DecNeighbours()
    {
        status -= 0x02;
    }
}
