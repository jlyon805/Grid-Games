using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapTest : MonoBehaviour
{
    [SerializeField] private HeatmapGeneric heatmapGeneric;
    [SerializeField] private HeatmapBool heatmapBool;
    private Grid<HeatmapGridObj> grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<HeatmapGridObj>(20, 10, 5f, Vector3.zero, (Grid<HeatmapGridObj> g, int x, int y) => new HeatmapGridObj(g, x, y));

        heatmapGeneric.SetGrid(grid);
        //heatmapBool.SetGrid(grid);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = Utilities.GetMouseWorldPosition();
            HeatmapGridObj heatmapGridObj = grid.GetGridObject(position);
            if (heatmapGridObj != null)
                heatmapGridObj.AddValue(5);
        }

        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetGridObject(Utilities.GetMouseWorldPosition()));
        }
    }
}

public class HeatmapGridObj
{
    private Grid<HeatmapGridObj> grid;
    private int value;

    private int x, y;

    private const int HEATMAP_MIN = 0;
    private const int HEATMAP_MAX = 100;

    public HeatmapGridObj(Grid<HeatmapGridObj> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        Mathf.Clamp(value, HEATMAP_MIN, HEATMAP_MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / HEATMAP_MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
