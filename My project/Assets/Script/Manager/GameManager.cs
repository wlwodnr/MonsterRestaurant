using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private TimeManager timeManager;
    
    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log($"중복된 GameManager가 발견되어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        GameDataTester.StartDataTest();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            TimeManager.Instance.TimeSetMorning();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            TimeManager.Instance.TimeSetNight();
        }
    }
}
