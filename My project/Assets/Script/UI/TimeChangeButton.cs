using UnityEngine;
using UnityEngine.UI;

public class TimeChangeButton : MonoBehaviour
{

    private bool _isCoolDown = false;

    [SerializeField]
    private float _mCoolTime = 1.0f;

    [SerializeField] private Button _buttonBase;
    [SerializeField] private Image _buttonImage;

    private void Awake()
    {
        if(_buttonBase == null)
        {
            _buttonBase = GetComponent<Button>();
        }
        if( _buttonImage == null)
        {
            _buttonImage = GetComponent<Image>();
        }
    }


    private void Start()
    {
        _buttonBase.onClick.RemoveAllListeners();
        _buttonBase.onClick.AddListener(OnButtonClick);

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeStateChanged -= UpdateButtonImage;
            TimeManager.Instance.OnTimeStateChanged += UpdateButtonImage;
            UpdateButtonImage(TimeManager.Instance.CurrentState);
        }
        else
        {
            Debug.LogError("[TimeChangeButton] Start 시점에도 TimeManager를 찾을 수 없습니다!");
        }
    }

    private void OnButtonClick()
    {
        if (_isCoolDown) return; 

        StartCoroutine(CoolDownRoutine()); 
        TimeManager.Instance.ToggleTimeState();
    }

    private System.Collections.IEnumerator CoolDownRoutine()    //쿨타임
    {
        _isCoolDown = true;
        yield return new WaitForSeconds(_mCoolTime); 
        _isCoolDown = false;
    }


    private void UpdateButtonImage(_mTimeState state)
    {
        string path = state switch
        {
            _mTimeState.Morning => "Image/Morning",
            _mTimeState.Night => "Image/Night",
            _mTimeState.Restaurant => "Image/Restaurant",
            _ => "Image/Morning"
        };

        Sprite newSprite = Resources.Load<Sprite>(path);

        Debug.Log($"[Debug] 로드 시도 경로: {path}, 결과: {(newSprite == null ? "NULL!" : "성공")}");

        if (newSprite != null)
        {
            _buttonImage.sprite = newSprite;
        }
        else
        {
            Debug.LogError($"[TimeChangeButton] 이미지를 찾을 수 없습니다: {path}");
        }
    }
}