using UnityEngine;

public class CropObject : MonoBehaviour
{
    public Vector3Int CellPosition {  get; private set; }
    public string CropId { get; private set; }

    public void Initialized(Vector3Int cellPos, string cropId)
    {
        CellPosition = cellPos;
        CropId = cropId;
    }

    //private void OnMouseDown()
    //{
    //    if(CropManager.Instance != null)
    //    {
    //        CropManager.Instance
    //    }
    //}
}