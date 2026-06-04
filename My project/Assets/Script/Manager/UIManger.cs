using UnityEngine;

public class UIManger : MonoBehaviour
{
    public static UIManger Instance { get; private set; }

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

    }

    public void CloseRestaurantDialogue()
    {

    }

    public void OpenRestaurantMenu()
    {

    }
}
