using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Win/Lose Conditions")]
    [SerializeField] private int MaxGameDays = 10;          //제한 시간
    [SerializeField] private int TargetClearGold = 1500;    //클리어 조건

    [SerializeField]
    private int _currentDay = 1;

    public static GameManager Instance {  get; private set; }
    public InventoryModel InventoryModel { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            InitGameManager();
        }
        else
        {
            Debug.Log($"중복된 GameManager가 발견되어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
            return;
        }
    }
    public void Start()
    {
        //GameDataTester.StartDataTest();
    }

    private void InitGameManager()
    {
        InventoryModel = new InventoryModel();
    }

    public void AdvanceDay()
    {
        _currentDay++;
        Debug.Log($"[GameManager] 날짜 경과. 현재 일수: {_currentDay}일차");

        CheckGameEndConditions();
    }

    public void CheckGameEndConditions()
    {
        int currentGold = InventoryModel.GetItemCount("Dokkaebi_Coin");

        if( currentGold >= TargetClearGold )
        {
            Debug.Log($"[게임 클리어] {MaxGameDays}일 내에 {TargetClearGold} 코인을 모았습니다! 최종 코인: {currentGold}");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OpenGameClearUI();
            }
            return;
        }
        if (_currentDay > MaxGameDays)
        {
            Debug.Log($"[게임 오버] {MaxGameDays}일이 지났으나 목표 금액에 도달하지 못했습니다. 최종 코인: {currentGold}");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OpenGameOverUI();
            }
            return;
        }
    }

    void Update()
    {
        
    }

    public void GameStart()
    {
        Debug.Log("[GameManager] 게임 시작! 데이터를 초기화합니다.");
        _currentDay = 1;
    }

}
