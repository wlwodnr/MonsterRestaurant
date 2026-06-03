using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PotatoCounter : MonoBehaviour
{
    [SerializeField] private Text counterText;
    [SerializeField] private string targetItemId = "Crop_Potato";

    private void Update()
    {
        if (InventoryManager.Instance != null && counterText != null)
        {
            int CurrentCount = InventoryManager.Instance.GetItemCount(targetItemId);
            counterText.text = CurrentCount.ToString();
        }
    }
}
