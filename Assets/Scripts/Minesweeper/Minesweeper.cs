using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StatusFlags : byte
{
    clear, // 0b0000
    flag = 1,      // 0b0000_0001
    uncovered = 2,  // 0b0000_0010
    one = 4,   // 0b0000_0100
    two = 8,   // 0b0000_1000
    three = 12, // 0b0000_1100
    four = 16,  // 0b0001_0000
    five = 20,  // 0b0001_0100
    six = 24,   // 0b0001_1000
    seven = 28, // 0b0001_1100
    eight = 32, // 0b0010_0000
    mine = 36,  // 0b0010_0100
}

public class Minesweeper : MonoBehaviour
{
    private Grid<Tile> grid;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int mines;

    private bool gameover;

    private void Awake()
    {
        ClearGrid();
    }

    private void Update()
    {
        if (!gameover)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = Utilities.GetMouseWorldPosition();
                Tile tile = grid.GetGridObject(position);
                if (tile != null && !tile.IsFlagged())
                    tile.Uncover();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 position = Utilities.GetMouseWorldPosition();
                Tile tile = grid.GetGridObject(position);
                if (tile != null && !tile.IsCovered())
                    tile.Flag();
            }
        }
    }

    private void PopulateGrid()
    {
        Stack<(int, int)> mineStack = new Stack<(int, int)>();

        while (mineStack.Count != mines)
        {
            (int, int) xy = (Random.Range(0, width), Random.Range(0, height));
            if (!mineStack.Contains(xy))
                mineStack.Push(xy);
        }

        while (mineStack.Count > 0)
        {
            grid.GetGridObject(mineStack.Peek().Item1, mineStack.Peek().Item2).SetMine();
            mineStack.Pop();
        }
    }

    private void ClearGrid()
    {
        grid = new Grid<Tile>(width, height, 5f, Vector3.zero,
            (Grid<Tile> g, int x, int y) => new Tile(g, x, y));

        gameover = true;
    }

    private void NewGame()
    {
        ClearGrid();
        PopulateGrid();
        gameover = false;
    }
}

public class Tile
{
    private Grid<Tile> grid;
    private byte status; // ##NNNNCF-- #: unused, N: Neighbours/status, C: covered/uncovered, 

    private int x, y;

    public Tile(Grid<Tile> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        status = 0;
    }

    public void SetMine()
    {
        status = (byte)StatusFlags.mine;

        List<Tile> neighbours = GetNeighbours(this);

        foreach (Tile neighbour in neighbours)
            if ((neighbour.status & (byte)StatusFlags.mine) != (byte)StatusFlags.mine)
                neighbour.status += 4;

        grid.TriggerGridObjectChanged(x, y);
    }

    public byte GetStatus()
    {
        return status;
    }

    public void Uncover()
    {

        grid.TriggerGridObjectChanged(x, y);
        if (status == (byte)StatusFlags.clear)
        {
            // BFS here
        }
    }

    private List<Tile> GetNeighbours(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        List<Tile> neighbours = new List<Tile>
        {
            grid.GetGridObject(x - 1, y + 1),
            grid.GetGridObject(x, y + 1),
            grid.GetGridObject(x + 1, y + 1),
            grid.GetGridObject(x - 1, y),
            grid.GetGridObject(x + 1, y),
            grid.GetGridObject(x - 1, y - 1),
            grid.GetGridObject(x, y - 1),
            grid.GetGridObject(x + 1, y - 1)
        };

        foreach (Tile neighbour in neighbours)
            if (neighbour == null)
                neighbours.Remove(neighbour);

        return neighbours;
    }
}