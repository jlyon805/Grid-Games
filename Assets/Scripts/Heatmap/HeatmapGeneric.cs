using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapGeneric : MonoBehaviour
{
    private Grid<HeatmapGridObj> grid;
    private Mesh mesh;
    private bool updateMesh;

    public const int HEATMAP_MIN = 0;
    public const int HEATMAP_MAX = 100;

    private void Awake()
    {
        this.mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateHeatMap();
        }
    }

    public void SetGrid(Grid<HeatmapGridObj> grid)
    {
        this.grid = grid;
        UpdateHeatMap();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<HeatmapGridObj>.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void UpdateHeatMap()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                HeatmapGridObj gridObj = grid.GetGridObject(x, y);
                float gridValNormalized = gridObj.GetValueNormalized();
                Vector2 gridValUV = new Vector2(gridValNormalized, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValUV, gridValUV);
            }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
