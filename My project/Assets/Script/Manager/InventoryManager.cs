using UnityEngine;
using System.Collections.Generic;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> itemInventory = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log($"중복된 InventoryManager가 있어 파괴합니다.");
            Destroy(gameObject);
            return;
        }
    }

    public void AddItem(string name, int count)
    {
        if (count <= 0)
        {
            Debug.Log("인벤토리에 추가를 시도하였으나, 잘못된 입력");
            return;
        }

        if (itemInventory.ContainsKey(name))
        {
            itemInventory[name] += count;
        }
        else
        {
            itemInventory.Add(name, count);
        }
        Debug.Log($"[Inventory] 아이템 획득: {name}을(를) {count}만큼 획득하여 총 {itemInventory[name]}개가 되었습니다.");

    }

    public bool RemoveItem(string name, int count)
    {
        if (count <= 0)
        {
            Debug.Log("인벤토리에 빼기를 시도하였으나, 잘못된 입력");
            return false;
        }
        else if (itemInventory.ContainsKey(name))
        {
            if (itemInventory[name] < count)
            {
                Debug.Log("인벤토리의 갯수보다 더 많은 양을 빼기를 시도하였습니다. 빼기 실패");
                return false;
            }
            itemInventory[name] -= count;
            return true;
        }
        else
        {
            Debug.Log("없는 아이템을 빼려고 시도하셨습니다. 실패");
            return false;
        }
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
