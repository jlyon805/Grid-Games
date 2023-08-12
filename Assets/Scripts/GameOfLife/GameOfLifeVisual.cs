using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLifeVisual : MonoBehaviour
{
    private Grid<LifeCell> grid;
    private Mesh mesh;
    private bool updateMesh;

    // Start is called before the first frame update
    void Awake()
    {
        this.mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        // Set Index format to 32-bit for XBOX HUEG grid visuals
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateLifeGrid();
        }
    }

    public void SetGrid(Grid<LifeCell> grid)
    {
        this.grid = grid;
        UpdateLifeGrid();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<LifeCell>.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void UpdateLifeGrid()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                LifeCell gridVal = grid.GetGridObject(x, y);
                float gridValNormalized = gridVal.IsAlive() ?1f : 0f;
                Vector2 gridValUV = new Vector2(gridValNormalized, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValUV, gridValUV);
            }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
