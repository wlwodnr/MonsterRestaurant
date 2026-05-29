using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaniTechUIManager : MonoBehaviour
{
    // 팝업과 일반 콘텐츠 UI가 배치될 루트 트랜스폼
    [SerializeField] private Transform canvasContentRoot;
    [SerializeField] private Transform canvasPopupRoot;

    public static DaniTechUIManager Instance { get; private set; }

    // 생성된 UI 보관 (메모리 재사용 및 GC 완화)
    private Dictionary<DaniTechUIType, DaniTechUIBase> _createdUIDic = new Dictionary<DaniTechUIType, DaniTechUIBase>();
    // 현재 화면에 열려 있는 UI 관리
    private HashSet<DaniTechUIType> _openedUISet = new HashSet<DaniTechUIType>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 일반 콘텐츠 UI 또는 버튼 연결용 UI를 엽니다.
    /// </summary>
    public DaniTechUIBase OpenContentUI(DaniTechUIType uiType)
    {
        return OpenUI(uiType, canvasContentRoot);
    }

    /// <summary>
    /// 팝업 UI를 엽니다. (팝업 전용 루트에 생성)
    /// </summary>
    public DaniTechUIBase OpenPopupUI(DaniTechUIType uiType)
    {
        return OpenUI(uiType, canvasPopupRoot);
    }

    /// <summary>
    /// 일반 콘텐츠 UI를 닫습니다.
    /// </summary>
    public void CloseContentUI(DaniTechUIType uiType)
    {
        CloseUI(uiType);
    }

    /// <summary>
    /// 팝업 UI를 닫습니다.
    /// </summary>
    public void ClosePopupUI(DaniTechUIType uiType)
    {
        CloseUI(uiType);
    }

    // 내부 공통 UI 오픈 로직
    private DaniTechUIBase OpenUI(DaniTechUIType uiType, Transform rootTransform)
    {
        DaniTechUIBase ui = GetOrCreateUI(uiType, rootTransform);

        if (ui != null && _openedUISet.Contains(uiType) == false)
        {
            ui.gameObject.SetActive(true);
            _openedUISet.Add(uiType);
        }

        return ui;
    }

    // 내부 공통 UI 폐쇄 로직 (오브젝트를 파괴하지 않고 비활성화)
    private void CloseUI(DaniTechUIType uiType)
    {
        if (_openedUISet.Contains(uiType))
        {
            if (_createdUIDic.TryGetValue(uiType, out var ui))
            {
                ui.gameObject.SetActive(false);
            }
            _openedUISet.Remove(uiType);
        }
    }

    // UI가 생성되어 있으면 가져오고, 없으면 Resources에서 로드하여 생성
    private DaniTechUIBase GetOrCreateUI(DaniTechUIType uiType, Transform rootTransform)
    {
        if (_createdUIDic.ContainsKey(uiType) == false)
        {
            // UI 프리팹 경로를 가져오는 규칙 (기존 GetUIPath 활용 혹은 커스텀 필요)
            string path = $"Prefabs/UI/{uiType}";
            GameObject original = Resources.Load<GameObject>(path);

            if (original != null)
            {
                GameObject go = Instantiate(original, rootTransform);
                var uiBase = go.GetComponent<DaniTechUIBase>();
                _createdUIDic.Add(uiType, uiBase);
            }
            else
            {
                Debug.LogError($"[UIManager] 프리팹을 찾을 수 없습니다: {path}");
            }
        }

        return _createdUIDic[uiType];
    }
}