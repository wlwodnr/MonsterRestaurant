using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> itemInventory = new Dictionary<string, int>();

    //public event Action OnInventoryChanged;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log($"중복된 InventoryManager가 있어 파괴합니다.");
            Destroy(gameObject);
            return;
        }
    }

    public void RefreshInventoryUI()
    {
        int potatoCount = GameManager.Instance.InventoryModel.GetItemCount("Crop_Potato");
        int coinCount = GameManager.Instance.InventoryModel.GetItemCount("Dokkaebi_Coin");
    }


    public void PrintInventoryDebug()
    {
        Debug.Log("====== 현재 인벤토리 보유 현황 ======");
        foreach (var slot in itemInventory)
        {
            Debug.Log($"- {slot.Key} : {slot.Value}개");
        }
        Debug.Log("=====================================");
    }

}
