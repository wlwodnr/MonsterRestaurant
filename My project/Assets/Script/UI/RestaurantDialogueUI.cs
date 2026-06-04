using UnityEngine;
using UnityEngine.UI;
public class RestaurantDialogueUI : MonoBehaviour
{
    private Text Text_DialogueDescription;
    private Text Text_CustomerName;
    private Button Button_Next;

    private void Awake()
    {
        if(Button_Next != null)
        {
            Button_Next.onClick.RemoveAllListeners();
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

}
