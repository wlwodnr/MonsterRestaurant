using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
 
public static class GameDataTester
{
    public static void StartDataTest()
    {
        GameUtil.LoadFullData();

        var myPotato_Level_0 = GameDataManager.Instance.GetCropData("Potato_Level_0");
        Debug.Log(myPotato_Level_0.Description);


        //foreach(var potato in GameDataManager.Instance.CropDataList)
        //{
        //    string key = potato.Key;
        //    var data = potato.Value;
        //    Debug.Log($"키는 {key}, 데이터의 이름: {data.Name}");
        //    Debug.Log(data.Description);
        //    Debug.Log(data.IconPath);
        //}
        foreach(var Dish in GameDataManager.Instance.DishDataList)
        {
            string key = Dish.Key;
            var data = Dish.Value;
            Debug.Log($"키는 {key}, 데이터 이름: {data.Name}");
        }

    }
}
