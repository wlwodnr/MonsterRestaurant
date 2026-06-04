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
        int constCount = 
    }

}
