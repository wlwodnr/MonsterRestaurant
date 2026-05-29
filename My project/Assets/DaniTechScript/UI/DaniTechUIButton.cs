using System;
using UnityEngine;
using UnityEngine.UI;

public class DaniTechUIButton : MonoBehaviour
{
    [SerializeField] private Button Button_Base;
    [SerializeField] private Text Text_Base;
    [SerializeField] private Image Image_Base;
    [SerializeField] private Image Image_Select;

    // ===================================================================
    // ① [추가] 이 버튼이 클릭되었을 때 어떤 UI를 열지 인스펙터에서 지정하는 변수
    // ===================================================================
    [SerializeField] private DaniTechUIType targetUIType;

    private void Awake()
    {
        InitUIButton();
        SetDefaultUI();
    }

    private void OnEnable()
    {
        // ② [기존 코드 유지] 선택 연출 기능 바인딩
        BindOnClickButtonEvent(OnClickSetSelectUI);

        // ===================================================================
        // ③ [추가] 선택 연출과 동시에, 버튼 고유의 UI 오픈 기능도 함께 바인딩!
        // ===================================================================
        BindOnClickButtonEvent(OnClickOpenTargetUI);
    }

    private void OnDisable()
    {
        Button_Base.onClick.RemoveAllListeners();
    }

    private void SetDefaultUI()
    {
        if (Image_Select != null)
        {
            Image_Select.gameObject.SetActive(false);
        }
    }

    private void InitUIButton()
    {
        if (Button_Base != null) return;

        var button = this.gameObject.GetComponentInChildren<Button>();
        if (button != null)
        {
            this.Button_Base = button;
        }
    }

    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Base == null) return;
        Button_Base.onClick.AddListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }

    public void UnBindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Base == null) return;
        Button_Base.onClick.RemoveListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }

    public void ChangeButtonText(string buttonStr)
    {
        if (Text_Base == null) return;
        Text_Base.text = buttonStr;
    }

    private void OnClickSetSelectUI()
    {
        if (Image_Select != null)
        {
            bool currentActive = Image_Select.gameObject.activeSelf;
            Image_Select.gameObject.SetActive(!currentActive);
        }
    }

    // ===================================================================
    // ④ [추가] 지정된 targetUIType을 판단해서 매니저에게 오픈을 요청하는 공통 함수
    // ===================================================================
    private void OnClickOpenTargetUI()
    {
        if (DaniTechUIManager.Instance == null) return;

        // 인스펙터에서 설정한 UI 타입에 따라 분기 처리
        switch (targetUIType)
        {
            case DaniTechUIType.DNInventory:
                // 매니저 확장 메서드(Extension)인 인벤토리 오픈 호출
                DaniTechUIManager.Instance.OpenInventoryPopup();
                break;

            case DaniTechUIType.DNInventory_Close:
                DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.DNInventory);
                Debug.Log("닫기 시도");
                break;

            //case DaniTechUIType.DNSimplePopup:
            //    // 예시: 팝업은 메시지가 필요하므로 기본 메시지 전달하며 오픈
            //    DaniTechUIManager.Instance.OpenSimplePopup("버튼을 통해 열린 팝업입니다.");
            //    break;

            case DaniTechUIType.DNMainUI:
                DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.DNMainUI);
                break;

            // 앞으로 새로운 UI 버튼이 필요하면 여기에 case만 추가하면 끝!
            default:
                Debug.LogWarning($"[UIButton] {targetUIType}에 대한 오픈 로직이 정의되지 않았습니다.");
                break;
        }
    }

    // 기존에 테스트로 만드셨던 함수는 지우거나 비워두셔도 됩니다.
    private void OnClickInventoryButton() { }
}