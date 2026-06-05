using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
public class DokkaebiCoinCounter : MonoBehaviour
{
    [SerializeField] private Text Text_CoinAmount;

    private void Start()
    {
        if (Text_CoinAmount == null)
        {
            Text_CoinAmount = GetComponent<Text>();
        }

    }

    private void Update()
    {
        RefreshCoinCounterUI();
    }

    private void RefreshCoinCounterUI()
    {
        if (Text_CoinAmount == null)
        {
            return;
        }
        if(GameManager.Instance != null && GameManager.Instance.InventoryModel !=null)
        {
            int currentCoinCount = GameManager.Instance.InventoryModel.GetItemCount("Dokkeabi_Coin");

            Text_CoinAmount.text = currentCoinCount.ToString();
        }
    }
}
