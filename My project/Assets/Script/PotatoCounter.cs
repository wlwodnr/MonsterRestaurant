using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PotatoCounter : MonoBehaviour
{
    [SerializeField] private Text counterText;
    [SerializeField] private string targetItemName = "Potato";

    void Start()
    {
        if(InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += UpdatePotatoCountUI;
        }

        UpdatePotatoCountUI();
    }

    private void OnDestroy()
    {
        if(InventoryManager.Instance != null )
        {
            InventoryManager.Instance.OnInventoryChanged -= UpdatePotatoCountUI;
        }
    }

    private void UpdatePotatoCountUI()
    {
        if (counterText == null) return;

        if(InventoryManager.Instance == null)
        {
            Debug.Log("인벤토리 매니저 없음");
        }
        else
        {
            int currentCount = InventoryManager.Instance.GetItemCount(targetItemName);

            counterText.text = $"{targetItemName} : {currentCount}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
