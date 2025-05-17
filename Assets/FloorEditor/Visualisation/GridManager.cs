using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 21;
    public int height = 21;
    public GameObject squarePrefab;
    public GameObject wallPrefab;
    public float cellSize = 1;

    private GridSquare[,] grid;

    public GridSquare[,] Grid => grid;

    [InitializeOnLoadMethod]
    private static void OnProjectReload()
    {
        GameObject.FindAnyObjectByType<GridManager>()?.ClearGrid();
    }
    
    public void CreateGrid()
    {
        // Clean up existing grid if any
       ClearGrid();

        grid = new GridSquare[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = transform.position + new Vector3(x * cellSize, 0, y * cellSize);
                GameObject square = PrefabUtility.InstantiatePrefab(squarePrefab, transform) as GameObject;
                square.transform.position = position;
                square.name = $"Square_{x}_{y}";
                square.hideFlags = HideFlags.HideAndDontSave;
                SceneVisibilityManager.instance.DisablePicking(square, true);
                
                grid[x, y] = square.GetComponent<GridSquare>();
                grid[x, y].NodeSaveData.f_c = new Vector2Int(x, y);
                
                if (x < 3 || y < 3 || x > width - 4 || y > height - 4)
                    grid[x, y].IsLocked = true;
                
                if (x > 0)
                {
                    var xWallGO = PrefabUtility.InstantiatePrefab(wallPrefab, transform) as GameObject;
                    xWallGO.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    xWallGO.transform.position = position - new Vector3(0.5f, 0, 0f);
                    xWallGO.name = $"XWall_{x}_{y}";
                    xWallGO.hideFlags = HideFlags.HideAndDontSave;
                    SceneVisibilityManager.instance.DisablePicking(xWallGO, true);
                    
                    var xWall = xWallGO.GetComponent<GridWall>();
                    xWall.negativeSquare = grid[x - 1, y];
                    xWall.positiveSquare = grid[x, y];
                    xWall.xAxis = true;
                    xWall.UpdateVisuals();
                }

                if (y > 0)
                {
                    var zWallGO = PrefabUtility.InstantiatePrefab(wallPrefab, transform) as GameObject;
                     zWallGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
                     zWallGO.transform.position = position - new Vector3(0, 0, 0.5f);
                     zWallGO.name = $"zWall{x}_{y}";
                     zWallGO.hideFlags = HideFlags.HideAndDontSave;
                     SceneVisibilityManager.instance.DisablePicking(zWallGO, true);
                     
                     var zWall = zWallGO.GetComponent<GridWall>();
                     zWall.negativeSquare = grid[x, y - 1];
                     zWall.positiveSquare = grid[x, y];
                     zWall.xAxis = false;
                     zWall.UpdateVisuals();
                }
                
                grid[x, y].UpdateVisuals();
            }
        }
        
        SceneView.RepaintAll();
    }

    public void ClearGrid()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    
    public GridSquare GetSquareAt(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return grid[x, y];
        }
        return null;
    }
}