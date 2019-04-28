using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PROJECTILE_TYPE { PROJ_BULLET, PROJ_LASER };

public class TowerDictionary : MonoBehaviour
{
    public List<TowerData> towerData;
    private static Dictionary<string, TowerData> towerDictionary;

    void Start()
    {
        towerDictionary = new Dictionary<string, TowerData>();
        foreach(TowerData TD in towerData)
        {
            towerDictionary.Add(TD.towerType, TD);
        }
    }

    public static bool ContainsTowerType(string whichType)
    {
        return towerDictionary.ContainsKey(whichType);
    }

    public static void AddTowerType(TowerData newType)
    {
        if(towerDictionary.ContainsKey(newType.towerType))
            print("TowerDictionary already contains a tower of type " + newType.towerType);
        else
            towerDictionary.Add(newType.towerType, newType);
    }

    public static TowerData GetTowerData(string whichType)
    {
        TowerData result;
        towerDictionary.TryGetValue(whichType, out result);
        return result;
    }

    public static bool IsValidUpgrade(string currentType, string upgradeType)//bool checkReseach)
    {
        if(currentType == upgradeType)
            return false;

        if(!ContainsTowerType(currentType) || !ContainsTowerType(upgradeType))
            return false;

        TowerData upgradeData = GetTowerData(upgradeType);
        while(upgradeData != null)
        {
            /*
            if(checkResearch)
            {
                if(!upgradeData.isResearched)
                    return false;
            }
            */

            upgradeType = upgradeData.prevTowerType;
            if(upgradeType == currentType)
                return true;

            upgradeData = GetTowerData(upgradeData.prevTowerType);
        }
        
        return false;
    }

    public static bool GetValueTotals(string towerType, out int totalBuyValue, out int totalSellValue)
    {
        totalBuyValue = 0;
        totalSellValue = 0;

        if(!ContainsTowerType(towerType))
            return false;

        TowerData tempData = GetTowerData(towerType);
        while (tempData != null)
        {
            totalBuyValue  += tempData.buildCost;
            totalSellValue += tempData.refundAmount;

            tempData = GetTowerData(tempData.prevTowerType);
        }
       
        return true;
    }
}
