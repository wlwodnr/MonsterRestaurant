using UnityEngine;
using UnityEngine.UI;

public class RestaurantMenuUI : MonoBehaviour
{
    [Header("Menu Button")]
    [SerializeField]
    private Button Button_DishPotatoPancake;
    [SerializeField]
    private Button Button_DishSteamedPotato;

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
    }

    private void OnClickDishButton(string dishId)
    {
        Debug.Log($"[RestaurantMenuUI] {dishId} 메뉴 선택됨.");

        RestaurantManager.Instance.ServeDish( dishId );

    }
}
