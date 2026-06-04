using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        GameDataTester.StartDataTest();
    }

    private void InitGameManager()
    {
        InventoryModel = new InventoryModel();
    }


    void Update()
    {
        
    }
}
