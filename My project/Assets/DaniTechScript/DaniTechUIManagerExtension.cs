using UnityEngine;

// 팝업과 콘텐츠 중심으로 루트 타입을 단순화
public enum DaniTechUIRootType
{
    None = 0,
    ContentUI,
    PopupUI
}

public enum DaniTechUIType
{
    DNSimplePopup,
    DNMainUI,
    DNMyProfilePopup,
    DNInventory,
    DNLoadingUI,
    DNDialogueUI,
    DNInfoBookUI,
    DNInventory_Close,

}

public static class DaniTechUIManagerExtension
{
    /// <summary>
    /// UI 타입과 루트 타입에 맞는 Resources 폴더 내의 프리팹 경로를 반환합니다.
    /// </summary>
    public static string GetUIPath(this DaniTechUIManager uiManager, DaniTechUIRootType uiRootType, DaniTechUIType uiType)
    {
        // 예: Prefabs/UI/PopupUI/DNSimplePopup
        return $"Prefabs/UI/{uiRootType}/{uiType}";
    }

    /// <summary>
    /// 게임 시작 시 기본적으로 필요한 UI들을 오픈합니다.
    /// </summary>
    public static void ShowStartupUIOnGameStart(this DaniTechUIManager uiManager)
    {
        uiManager.OpenLoadingUI();
        // 메인 UI도 이제 ContentUI 루트를 사용하도록 변경
        uiManager.OpenContentUI(DaniTechUIType.DNMainUI);
    }

    /// <summary>
    /// 단순 메시지 팝업을 엽니다. (버튼 이벤트 등에서 활용)
    /// </summary>
    //public static void OpenSimplePopup(this DaniTechUIManager uiManager, string msg)
    //{
    //    var uiBase = uiManager.OpenPopupUI(DaniTechUIType.DNSimplePopup);
    //    if (uiBase == null)
    //    {
    //        Debug.LogWarning("[UIManager] DNSimplePopup이 생성되지 않았습니다.");
    //        return;
    //    }

    //    if (uiBase is DaniTech_SimplePopup simplePopup)
    //    {
    //        simplePopup.SetUI(msg);
    //    }
    //}

    /// <summary>
    /// 캐릭터 프로필 팝업을 엽니다.
    /// </summary>
    

    /// <summary>
    /// 인벤토리 UI를 엽니다. (일반 콘텐츠 계층)
    /// </summary>
    public static void OpenInventoryPopup(this DaniTechUIManager uiManager)
    {
        var uiBase = uiManager.OpenContentUI(DaniTechUIType.DNInventory);
        if (uiBase == null)
        {
            Debug.LogWarning("[UIManager] DNInventory가 생성되지 않았습니다.");
            return;
        }
    }

    /// <summary>
    /// 로딩 UI를 엽니다. (기존 VeryFrontUI 대신 가장 상위인 PopupUI나 별도 지정 계층으로 통합 가능)
    /// </summary>
    public static void OpenLoadingUI(this DaniTechUIManager uiManager)
    {
        // 최상단 표시를 위해 PopupUI 루트를 활용하거나 필요에 따라 ContentUI로 조절
        var uiBase = uiManager.OpenPopupUI(DaniTechUIType.DNLoadingUI);
        if (uiBase == null)
        {
            Debug.LogWarning("[UIManager] DNLoadingUI가 생성되지 않았습니다.");
            return;
        }
    }

    /// <summary>
    /// 로딩 UI를 닫습니다.
    /// </summary>
    public static void CloseLoadingUI(this DaniTechUIManager uiManager)
    {
        uiManager.ClosePopupUI(DaniTechUIType.DNLoadingUI);
    }

    /// <summary>
    /// 대화창 UI를 엽니다.
    /// </summary>
    //public static void OpenDialogueUI(this DaniTechUIManager uiManager, string startDialogueId)
    //{
    //    var uiBase = uiManager.OpenContentUI(DaniTechUIType.DNDialogueUI);
    //    if (uiBase == null)
    //    {
    //        Debug.LogWarning("[UIManager] DNDialogueUI가 생성되지 않았습니다.");
    //        return;
    //    }

    //    if (uiBase is DaniTech_DialogueUI dialogueUi)
    //    {
    //        dialogueUi.StartDialogue(startDialogueId);
    //    }
    //}
}