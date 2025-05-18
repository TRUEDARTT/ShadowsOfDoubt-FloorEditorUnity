using TMPro;
using UnityEngine;

public class GridWall : MonoBehaviour
{
    public bool xAxis;
    public GridSquare negativeSquare;
    public GridSquare positiveSquare;
    public string wallType = "";
    
    public Transform wallMesh;
    public TMP_Text wallTextFront;
    public TMP_Text wallTextBack;
    
    public void ChangeWallType(bool active, string type)
    {
        if (active)
        {
            // Remove first
            negativeSquare.NodeSaveData.w_d.RemoveAll(wd => (xAxis ? wd.w_o.x : wd.w_o.y) > 0);
            positiveSquare.NodeSaveData.w_d.RemoveAll(wd => (xAxis ? wd.w_o.x : wd.w_o.y) < 0);
            
            // Then add the replacement
            var facingVector = xAxis ? new Vector2(0.5f, 0) : new Vector2(0, 0.5f);
            negativeSquare.NodeSaveData.w_d.Add(new WallSaveData() { p_n = type, w_o = facingVector });
            positiveSquare.NodeSaveData.w_d.Add(new WallSaveData() { p_n = type, w_o = -facingVector });
        }
        else
        {
            negativeSquare.NodeSaveData.w_d.RemoveAll(wd => (xAxis ? wd.w_o.x : wd.w_o.y) > 0);
            positiveSquare.NodeSaveData.w_d.RemoveAll(wd => (xAxis ? wd.w_o.x : wd.w_o.y) < 0);
        }
        
        UpdateVisuals();
    }
    
    public void UpdateVisuals()
    {
        bool hasNegativeWall = false;
        bool hasPositiveWall = false;

        string negativeWallType = "";
        string positiveWallType = "";
        
        wallType = "";
        
        // Check negative square's walls
        foreach (var wallData in negativeSquare.NodeSaveData.w_d)
        {
            var checkValue = xAxis ? wallData.w_o.x : wallData.w_o.y;
            // For x-axis walls, positive x means wall facing right
            // For z-axis walls, positive y means wall facing forward
            if (checkValue > 0)
            {
                negativeWallType = wallData.p_n;
                hasNegativeWall = true;
                break;
            }
        }
        
        // Check positive square's walls
        foreach (var wallData in positiveSquare.NodeSaveData.w_d)
        {
            var checkValue = xAxis ? wallData.w_o.x : wallData.w_o.y;
            // For x-axis walls, negative x means wall facing left
            // For z-axis walls, negative y means wall facing back
            if (checkValue < 0)
            {
                positiveWallType = wallData.p_n;
                hasPositiveWall = true;
                break;
            }
        }

        if (negativeWallType == positiveWallType)
        {
            wallType = negativeWallType;
        }
        else
        {
            Debug.LogWarning($"Non matching wall types between ({negativeSquare.NodeSaveData.f_c.x}, {negativeSquare.NodeSaveData.f_c.y}) ({negativeWallType}) and ({positiveSquare.NodeSaveData.f_c.x}, {positiveSquare.NodeSaveData.f_c.y}) ({positiveWallType})");
        }

        // Only show wall if both squares have matching walls
        bool wallIsActive = hasNegativeWall && hasPositiveWall;
        
        SetWallHeight(wallIsActive);
        
        wallTextFront.gameObject.SetActive(wallIsActive);
        wallTextBack.gameObject.SetActive(wallIsActive);

        if (wallType != "")
        {
            wallTextFront.text = WallManager.DoorWindowPresets[wallType];
            wallTextBack.text = WallManager.DoorWindowPresets[wallType];
        }
        
        if (hasNegativeWall != hasPositiveWall)
        {
            Debug.LogError($"Wall mismatch at {name}: negative={hasNegativeWall}, positive={hasPositiveWall}");
        }
    }

    private void SetWallHeight(bool active)
    {
        if (active)
        {
            transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
            wallMesh.localScale = new Vector3(0.1f, 0.5f, wallMesh.localScale.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 0.0025f, transform.position.z);
            wallMesh.localScale = new Vector3(0.05f, 0.05f, wallMesh.localScale.z);
        }
    }
}