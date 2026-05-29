using System;
using UnityEngine;

public class CropObject : MonoBehaviour
{
    public Vector3Int CellPosition {  get; private set; }
    public string CropId { get; private set; }

    public void Initialize(Vector3Int cellPos, string cropId)
    {
        CellPosition = cellPos;
        CropId = cropId;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("수확 시도");
            if (CropManager.Instance != null) 
            {
                CropManager.Instance.HandleCropInteraction(this);
            }
        }
    }
}