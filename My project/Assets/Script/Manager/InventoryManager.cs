using System;
using System.Collections.Generic;
using UnityEngine;
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

    //public void AddItem(string itemId, int count)
    //{
    //    if(string.IsNullOrEmpty(itemId))
    //    {
    //        Debug.Log("들어오는 아이템의 id값이 없습니다.");
    //        return;
    //    }
    //    if(itemInventory.ContainsKey(itemId))
    //    {
    //        itemInventory[itemId] += count;
    //    }
    //    else
    //    {
    //        itemInventory[itemId] = count;
    //    }

    //        Debug.Log($"[Inventory] 아이템 획득: {itemId}을(를) {count}만큼 획득하여 총 {itemInventory[itemId]}개가 되었습니다.");

    //}

    //public bool RemoveItem(string itemId, int count)
    //{
    //    if(!itemInventory.ContainsKey(itemId) || itemInventory[itemId] < count)
    //    {
    //        Debug.LogWarning($"[Inventory] {itemId}의 수량이 부족하여 소모할 수 없습니다.");
    //        return false;
    //    }

    //    itemInventory[itemId] -= count;
    //    Debug.Log($"[Inventory] {itemId} 소모: -{count} (현재 수량: {itemInventory[itemId]})");

    //    return true;
    //}

    //public int GetItemCount(string itemId)
    //{
    //    if (itemInventory.ContainsKey(itemId))
    //    {
    //        return itemInventory[itemId];
    //    }
    //    Debug.Log("해당 아이템은 없습니다.");
    //    return 0;
    //}

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
