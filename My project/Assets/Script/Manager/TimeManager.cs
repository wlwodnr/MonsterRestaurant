using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public enum _mTimeState { Morning, Night, Restaurant }

public class TimeManager : MonoBehaviour
{
    private Light2D globalLite2D;

    public event System.Action<_mTimeState> OnTimeStateChanged;

    public _mTimeState CurrentState { get; private set; } = _mTimeState.Morning;



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

    public void ToggleTimeState()
    {
        switch(CurrentState)
        {
            case _mTimeState.Morning:
                SetState(_mTimeState.Night);
                break;
            case _mTimeState.Night:
                SetState(_mTimeState.Restaurant);
                break;
            case _mTimeState.Restaurant:
                SetState(_mTimeState.Morning);
                break;
        }
    }

    private void SetState(_mTimeState newState)
    {
        CurrentState = newState;
        UpdateLighting();

        OnTimeStateChanged?.Invoke(CurrentState);
        Debug.Log($"[TimeManager] 상태가 {CurrentState}로 변경되었습니다.");
    }

    private void UpdateLighting()
    {
        if(globalLite2D == null)
        {
            return;
        }

        globalLite2D.intensity = (CurrentState == _mTimeState.Morning) ? _MorningLightValue : _NightLightValue;

        if(CurrentState == _mTimeState.Morning && TileManager.Instance != null)
        {
            TileManager.Instance.DryingGround();
        }
    
    }

}
