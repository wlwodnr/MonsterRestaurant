using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject TitlePanel;
    [SerializeField]
    private GameObject GameClearPanel;
    [SerializeField]
    private GameObject GameOverPanel;
    [SerializeField]
    private Button Button_GameOverExit;

    [SerializeField]
    private Button Button_GameStart;
    [SerializeField]
    private Button Button_ExitGame;

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
        InitButton();
    }

    private void InitButton()
    {
        if(Button_GameStart != null)
        {
            Button_GameStart.onClick.RemoveAllListeners();
            Button_GameStart.onClick.AddListener(OnClickGameStart);
        }
        if (Button_ExitGame != null)
        {
            Button_ExitGame.onClick.RemoveAllListeners();
            Button_ExitGame.onClick.AddListener(OnClickExitGame);
        }
        if(Button_GameOverExit != null)
        {
            Button_GameOverExit.onClick.RemoveAllListeners();
            Button_GameOverExit.onClick.AddListener(OnClickExitGame);
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

    public void OpenTitleUI()
    {
        if(TitlePanel != null)
        {
            TitlePanel.gameObject.SetActive(true );
            
        }
        if(GameClearPanel != null)
        {
            GameClearPanel.gameObject.SetActive(false);
        }
        if (GameOverPanel != null)
        {
            GameOverPanel.gameObject.SetActive(false);
        }
    }
    public void OpenGameClearUI()
    {
        if(TitlePanel != null)
        {
            TitlePanel.gameObject.SetActive(false );
        }
        if(GameClearPanel != null)
        {
            GameClearPanel.gameObject.SetActive(true );
        }
        if (GameOverPanel != null)
        {
            GameOverPanel.gameObject.SetActive(false);
        }
    }

    public void OpenGameOverUI()
    {
        if(TitlePanel != null)
        {
            TitlePanel.gameObject.SetActive(false);
        }
        if (GameClearPanel != null)
        {
            GameClearPanel.gameObject.SetActive(false);
        }
        if(GameOverPanel != null)
        {
            GameOverPanel.gameObject.SetActive(true);
        }
    }

    private void OnClickGameStart()
    {
        Debug.Log("[UIManager] 게임 시작! 메인 화면을 닫습니다.");
        if (TitlePanel != null) 
        { 
            TitlePanel.SetActive(false); 
        }
        GameManager.Instance.GameStart();
    }
    private void OnClickExitGame()
    {
        Debug.Log("[UIManager] 클리어 화면에서 게임을 종료합니다.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
