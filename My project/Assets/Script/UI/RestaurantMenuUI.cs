using UnityEngine;
using UnityEngine.UI;

public class RestaurantMenuUI : MonoBehaviour
{
    [Header("Menu Button")]
    [SerializeField]
    private Button Button_DishPotatoPancake;
    [SerializeField]
    private Button Button_DishSteamedPotato;

    [Header("MenuInfo")]
    [SerializeField]
    private Text Text_PancakePotatoInfo;
    [SerializeField]
    private Text Text_SteamedPotatoInfo;

    private void Awake()
    {
        if(Button_DishPotatoPancake != null)
        {
            Button_DishPotatoPancake.onClick.RemoveAllListeners();
            Button_DishPotatoPancake.onClick.AddListener (() => OnClickDishButton("Potato_Food_01"));
        }
        if(Button_DishSteamedPotato != null)
        {
            Button_DishSteamedPotato.onClick.RemoveAllListeners();
            Button_DishSteamedPotato.onClick.AddListener(() => OnClickDishButton("Potato_Food_02"));
        }
    }

    public void SetupMenuPanel()
    {
        Debug.Log("[RestaurantMenuUI] 메뉴판 활성화 및 세팅 완료.");
        DishData pancakePotatoData = GameDataManager.Instance.GetDishData("Potato_Food_01");
        DishData steamedPotatoData = GameDataManager.Instance.GetDishData("Potato_Food_02");

        int currentPotatoCount = GameManager.Instance.InventoryModel.GetItemCount("Crop_Potato");

        if(pancakePotatoData != null && Text_PancakePotatoInfo != null)
        {
            Text_PancakePotatoInfo.text = $"{pancakePotatoData.Name}\n(필요 재료: { currentPotatoCount}/{pancakePotatoData.CostCount})";
        }
        if (steamedPotatoData != null && Text_SteamedPotatoInfo != null)
        {
            Text_SteamedPotatoInfo.text = $"{steamedPotatoData.Name}\n(필요 재료: {currentPotatoCount}/{steamedPotatoData.CostCount})";
        }
    }

    private void OnClickDishButton(string dishId)
    {
        Debug.Log($"[RestaurantMenuUI] {dishId} 메뉴 선택됨.");

        RestaurantManager.Instance.ServeDish( dishId );

        UIManager.Instance.CloseRestaurantMenu();
    }
}
