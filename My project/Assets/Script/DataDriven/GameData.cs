using System;
using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

[System.Serializable]
public class GroundTypeData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string NextLevelID;
}

[System.Serializable]
public class ItemData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class CropData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string NextLevelID;
    public string CropState; 
}

[System.Serializable]
public class DishData : GameDataBase
{
    public string Name;
    public string Description;
    public string CostItemID; // CostType에서 변경
    public int CostCount;
    public string PriceItemID; 
    public int Price;         
}

[System.Serializable]
public class CustomerData: GameDataBase
{
    public string Name;
    public string Description;
    public string CorrectAnswer;
}