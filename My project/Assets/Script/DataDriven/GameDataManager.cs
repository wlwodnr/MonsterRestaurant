using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        // +++ C# 콘솔때와 다르게 이제 Main()함수가 아닌
        // 모노의 메서드에서 호출될 수 있으므로, 데이터 매니저가 활성화되면 바로 모든 데이터를 한번 받아오자
        // 이처리는 원하는 시점이 있다면 이전해도 된다
        GameUtil.LoadFullData();
        //GameDataTester.StartDataTest();
    }

    // --- JsonUtility의 한계를 극복하기 위한 Wrapper 클래스 ---
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items; // JSON 파일의 루트 키 이름이 "items"여야 함
    }
    // ---------------------------------------------------

    public Dictionary<string, CropData> CropDataList { get; private set; } = new Dictionary<string, CropData>();
    

    //private Dictionary<string, T> LoadData<T>(string jsonPath) where T : GameDataBase
    private Dictionary<string, T> LoadData<T>(string resourcePath) where T : GameDataBase
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(resourcePath);
        if (jsonAsset == null)
        {
            Debug.LogError($"[Error] 파일을 찾을 수 없습니다: Resources/{resourcePath}.json 파일이 존재하는지 확인하세요.");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = jsonAsset.text;
            //string jsonString = File.ReadAllText(jsonPath);

            // JsonUtility는 List<T>를 직접 못 가져오므로 Wrapper를 사용합니다.
            // 만약 JSON이 배열 형태([ {...}, {...} ])라면 아래 방식이 필요합니다.
            // 만약 JSON 구조가 { "items": [...] } 형태가 아니라면 
            // jsonString을 수정하여 강제로 감싸는 트릭을 써야 합니다.
            string wrappedJson = "{\"items\":" + jsonString + "}";
            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper != null && wrapper.items != null)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.items.Count}개 로드했습니다.");
                return wrapper.items.ToDictionary(item => item.Id);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} JSON 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }



    public void LoadCropData(string resourcePath)
    {
        CropDataList = LoadData<CropData>(resourcePath);
    }


    // [아래는 사용을 위한 부분들을 메서드 정의] =========================================================================================
    // Get과 Find이름을 꼭 구별 하자!

    public CropData GetCropData(string id)
    {
        if(CropDataList == null || string.IsNullOrEmpty(id))return null;
        
        return CropDataList.TryGetValue(id,out var data) ? data : null;
    }
}