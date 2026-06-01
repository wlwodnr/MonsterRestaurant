using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TimeManager : MonoBehaviour
{
    private bool _isMorning;
    private Light2D globalLite2D;

    [Header("Light Setting")]
    [SerializeField]
    private float _MorningLightValue = 1.0f;
    [SerializeField]
    private float _NightLightValue = 0.4f;

    [SerializeField] 
    private Light2D Light_Global2D;

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
            return;
        }

        if (globalLite2D == null)
        {
            GameObject lightObj = GameObject.Find("Global Light 2D");
            if (lightObj != null)
            {
                globalLite2D = lightObj.GetComponent<Light2D>();
                globalLite2D.lightType = Light2D.LightType.Global;
            }
            if (globalLite2D == null)
            {
                Debug.LogError("[TimeManager] 씬에서 globalLite2D 오브젝트 또는 컴포넌트를 찾을 수 없습니다");
            }
        }
    }


    void Start()
    {
        if(Light_Global2D != null)
        {
            _isMorning = GetCurrentTimeState();
        }
        else
        {
            _isMorning = true;
        }
    }

    public void TimeSetNight()
    {
        if(globalLite2D == null)
        {
            Debug.Log("인스팩터 창에 빛을 지정하지 않았습니다. 지정해주세요.");
            return;
        }
        if(_isMorning == false)
        {
            Debug.Log("이미 밤입니다");
            return;
        }
        _isMorning = false;
        Debug.Log("밤으로 변경됩니다.");
        globalLite2D.intensity = _NightLightValue;

    }
    public void TimeSetMorning()
    {
        if (globalLite2D == null)
        {
            Debug.Log("인스팩터 창에 빛을 지정하지 않았습니다. 지정해주세요.");
            return;
        }
        if(_isMorning == true)
        {
            Debug.Log("이미 낮입니다");
            return;
        }
        _isMorning = true;
        Debug.Log("낮으로 변경됩니다.");
        globalLite2D.intensity = _MorningLightValue;
        if(TileManager.Instance != null)
        {
            TileManager.Instance.DryingGround();
        }
    }

    private bool GetCurrentTimeState()
    {
        if (Light_Global2D == null)
        {
            return true;
        }
        return Light_Global2D.intensity > _NightLightValue;
    }
}
