using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get; private set; }

    [SerializeField]
    private GameObject GameObject_Cutsomer;


    private CustomerData _currentCustomerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log($"중복된 RestaurantManager가 있어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    public void StartRandomCustomerRestaurant()
    {
        int randomOrderIndex = Random.Range(1, 11);
        string randomCustomerId = $"Dokkaebi_Order_{randomOrderIndex}";

        Debug.Log($"[레스토랑] 오늘 무작위로 선정된 주문 ID: {randomCustomerId}");

        SetupTodayCustomer(randomCustomerId);
    }

    public void SetupTodayCustomer(string customerId)
    {
        _currentCustomerData = GameDataManager.Instance.GetCustomerData(customerId);

        if(_currentCustomerData != null )
        {
            Debug.Log($"[레스토랑] 오늘 손님 세팅 완료: {_currentCustomerData.Name}");

            if(GameObject_Cutsomer != null)
            {
                GameObject_Cutsomer.SetActive(true);
            }
            
            UIManger.Instance.OpenRestaurantDialogue(_currentCustomerData);
        }
        else
        {
            Debug.LogError($"[RestaurantManager] {customerId} 데이터를 로드하지 못했습니다.");
        }
    }
    public void ServeDish(string seletedDishId)
    {
        if(_currentCustomerData  == null)
        {
            Debug.LogError("[RestaurantManager] 현재 레스토랑에 활성화된 손님이 없습니다.");
            return;
        }
        DishData dishData = GameDataManager.Instance.GetDishData(seletedDishId);
        if (dishData == null)
        {
            return;
        }
        string costItemId = dishData.CostItemID;
        int costCount = dishData.CostCount;

        int currentPotatoCount = GameManager.Instance.InventoryModel.GetItemCount(costItemId);

        if(currentPotatoCount < costCount)
        {
            Debug.LogWarning($"[레스토랑] 재료 부족! 필요량: {costCount}, 보유량: {currentPotatoCount}");
            return;
        }
        GameManager.Instance.InventoryModel.RemoveItem(costItemId, costCount);

        int finalRewardPrice = dishData.Price;
        string rewardItemId = dishData.PriceItemID;

        if(_currentCustomerData.CorrectAnswer == seletedDishId)
        {
            finalRewardPrice *= 2;
            Debug.Log($"[정답] 도깨비의 기호에 일치하여 보상 2배 지급 적용. 금액: {finalRewardPrice}");
        }
        else
        {
            Debug.Log($"[일반 서빙] 요구와 다른 음식을 주어 기본 가치만 지급합니다. 금액: {finalRewardPrice}");
        }

        GameManager.Instance.InventoryModel.AddItem(rewardItemId, finalRewardPrice);
        if (GameObject_Cutsomer != null)
        {
            GameObject_Cutsomer.SetActive(false);
        }
    }

}
