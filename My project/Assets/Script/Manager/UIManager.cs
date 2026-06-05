using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] 
    private RestaurantDialogueUI UI_RestaurantDialogue;
    [SerializeField] 
    private RestaurantMenuUI UI_RestaurantMenu;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"중복된 UIManager가 있어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    public void OpenRestaurantDialogue(CustomerData customerData)
    {
        if(UI_RestaurantDialogue ==null)
        {
            Debug.Log("[UIManager] UI_RestaurantDialogue가 인스펙터에 연결되지 않았습니다.");

            return;
        }
        UI_RestaurantDialogue.gameObject.SetActive(true);
        UI_RestaurantDialogue.SetDialogue(customerData);
    }

    public void CloseRestaurantDialogue()
    {
        if(UI_RestaurantDialogue != null)
        {
            UI_RestaurantDialogue.gameObject.SetActive(false);
        }
    }

    public void OpenRestaurantMenu()
    {
        if( UI_RestaurantMenu == null)
        {
            Debug.Log("[UIManager] UI_RestaurantMenu가 인스펙터에 연결되지 않았습니다.");
            return;
        }
        UI_RestaurantMenu.gameObject.SetActive(true );
        UI_RestaurantMenu.SetupMenuPanel();
    }

    public void CloseRestaurantMenu()
    {
        if(UI_RestaurantMenu != null)
        {
            UI_RestaurantMenu.gameObject.SetActive(false);
        }
    }
}
