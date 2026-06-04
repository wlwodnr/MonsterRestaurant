using UnityEngine;
using System.Collections.Generic;
public class InventoryModel
{
    private Dictionary<string, int> _itemInventory = new Dictionary<string, int>();

    public void AddItem(string itemId, int count)
    {
        if (count <= 0)
        {
            return;
        }
        if(_itemInventory.ContainsKey(itemId))
        {
            _itemInventory[itemId] = count;
        }
        else
        {
            _itemInventory.Add(itemId, count);
        }
        Debug.Log($"[장부 갱신] {itemId}가 {count}개 추가되었습니다. 현재 총 수량: {_itemInventory[itemId]}개");
    }

    public void RemoveItem(string itemId, int count)
    {
        if(count <= 0)
        {
            return;
        }
        if(_itemInventory.ContainsKey(itemId))
        {
            _itemInventory[itemId] -= count;
            Debug.Log($"[장부 갱신] {itemId}가 {count}개 차감되었습니다. 남은 수량: {_itemInventory[itemId]}개");
        }
        else
        {
            Debug.Log($"[장부 경고] {itemId}를 차감하려 했으나 장부 잔액이 부족하거나 아이템이 없습니다.");
        }
    }

    public int GetItemCount(string itemId)
    {
        if(_itemInventory.TryGetValue(itemId, out var count))
        {
            return count;
        }
        return 0;
    }
}
