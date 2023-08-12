using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid<TGridObj>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private readonly float cellSize;
    private Vector3 originPosition;
    private TGridObj[,] gridArray;
    private TextMesh[,] debugTextArray;
    private bool showDebug = false;
    private bool showText = false;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObj>, int, int, TGridObj> createGridObj)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObj[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
                gridArray[x, y] = createGridObj(this, x, y);

        if (showDebug)
        {
            if(showText)
             debugTextArray = new TextMesh[width, height];

            for (int i = 0; i < gridArray.GetLength(0); i++)
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    if(showText)
                        debugTextArray[i, j] = Utilities.CreateWorldText(gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                if(showText)
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetCartesianPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObj value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridValueChanged != null)
                OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            if(showDebug) debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null)
            OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObj value)
    {
        int x, y;
        GetCartesianPosition(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObj GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        else
            return default(TGridObj);
    }

    public TGridObj GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetCartesianPosition(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

/*    public void AddValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }*/

/*    public void AddValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange)
    {
        int lowerValAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

        GetCartesianPosition(worldPosition, out int originX, out int originY);
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                int addValueAmount = value;
                if (radius > fullValueRange)
                    addValueAmount -= lowerValAmount * (radius - fullValueRange);

                AddValue(originX + x, originY + y, addValueAmount);

                if (x != 0)
                    AddValue(originX - x, originY + y, addValueAmount);
                if (y != 0)
                {
                    AddValue(originX + x, originY - y, addValueAmount);
                    if (x != 0)
                        AddValue(originX - x, originY - y, addValueAmount);
                }
            }
        }
    }*/

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public void SetWidth(int w)
    {
        width = w;
    }

    public void SetHeight(int h)
    {
        height = h;
    }

    public float GetCellSize()
    {
        return cellSize;
    }
}
