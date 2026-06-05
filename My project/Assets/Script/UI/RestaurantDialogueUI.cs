using UnityEngine;
using UnityEngine.UI;
public class RestaurantDialogueUI : MonoBehaviour
{
    [SerializeField]
    private Text Text_DialogueDescription;
    [SerializeField]
    private Text Text_CustomerName;
    [SerializeField]
    private Button Button_Next;

    private void Awake()
    {
        
        if(Button_Next != null)
        {
            Button_Next.onClick.RemoveAllListeners();
            Button_Next.onClick.AddListener(OnClickNextStep);
        }
    }

    public void SetDialogue(CustomerData customerData)
    { 
        if(customerData == null)
        {
            return;
        }
        if(Text_CustomerName != null)
        {
            Text_CustomerName.text = customerData.Name;
        }
        if (Text_DialogueDescription != null)
        {
            Text_DialogueDescription.text = customerData.Description;
        }
    }
     
    private void OnClickNextStep()
    {
        Debug.Log("[RestaurantDialogueUI] 버튼이 성공적으로 인지되어 화면을 전환합니다.");
        UIManager.Instance.CloseRestaurantDialogue();

        UIManager.Instance.OpenRestaurantMenu();
    }

}
