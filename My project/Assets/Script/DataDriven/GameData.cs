using System;
using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

// C# 때와 약간 달라진 점
    // Syste.Text.Json대신 유니티 내장 JsonUtility를 사용
    // 따라서 프로퍼티말고 그냥 일반 public 멤버변수로 변경함
    // [System.Serializable]가 없다면 JsonUtility는 데이터를 무시


[System.Serializable]
public class CropData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string NextLevelID;
}

[System.Serializable]
public class DishData : GameDataBase
{
    public string Name;
    public string Description;
    public string CostType;
    public string CostCount;
    public float Price;
}

[System.Serializable]
public class CustomerData: GameDataBase
{
    public string Name;
    public string Description;
    public string CorrectAnswer;
}