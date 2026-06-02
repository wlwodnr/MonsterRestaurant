using UnityEngine;
using UnityEngine.UI;

public class TimeChangeButton : MonoBehaviour
{
    [SerializeField] private Button _buttonBase;
    [SerializeField] private Image _buttonImage; // 이미지가 바뀌어야 할 컴포넌트

    private void Start()
    {
        if (_buttonBase == null) _buttonBase = GetComponent<Button>();
        if (_buttonImage == null) _buttonImage = GetComponent<Image>();

        _buttonBase.onClick.AddListener(OnButtonClick);

        // [수정] 매니저 상태와 상관없이 시작 시 무조건 '아침' 이미지로 강제 설정합니다.
        Sprite initialMorningSprite = Resources.Load<Sprite>("Image/Morning");
        if (initialMorningSprite != null)
        {
            _buttonImage.sprite = initialMorningSprite;
        }
        else
        {
            Debug.LogError("[TimeChangeButton] 초기 아침 이미지를 로드할 수 없습니다!");
        }
    }

    private void OnButtonClick()
    {
        TimeManager.Instance.ToggleTimeState();

        UpdateImage();
    }

    private void UpdateImage()
    {
        bool isMorning = TimeManager.Instance.IsMorning;

        string path = isMorning ? "Image/Morning" : "Image/Night";
        Sprite newSprite = Resources.Load<Sprite>(path);

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