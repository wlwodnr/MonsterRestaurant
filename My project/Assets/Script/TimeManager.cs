using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    private bool isMoring;
    [SerializeField] private Light2D globalLite2D;
    [SerializeField] private TileManager timeManager;

    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log($"중복된 TimeManager가 발견되어 파괴합니다: {gameObject.name}");
            Destroy(gameObject);
        }
    }


    void Start()
    {
        isMoring = GetCurrentTimeState();
    }

    public void TimeSetNight()
    {
        if(globalLite2D == null)
        {
            Debug.Log("인스팩터 창에 빛을 지정하지 않았습니다. 지정해주세요.");
            return;
        }
        if(isMoring == false)
        {
            Debug.Log("이미 밤입니다");
            return;
        }
        isMoring = false;
        Debug.Log("밤으로 변경됩니다.");
        globalLite2D.intensity = 0.4f;

    }
    public void TimeSetMoring()
    {
        if (globalLite2D == null)
        {
            Debug.Log("인스팩터 창에 빛을 지정하지 않았습니다. 지정해주세요.");
            return;
        }
        if(isMoring == true)
        {
            Debug.Log("이미 낮입니다");
            return;
        }
        isMoring = true;
        Debug.Log("낮으로 변경됩니다.");
        globalLite2D.intensity = 1f;
        timeManager.DryingGround();
    }

    private bool GetCurrentTimeState()
    {
        if(globalLite2D.intensity == 0.4f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
