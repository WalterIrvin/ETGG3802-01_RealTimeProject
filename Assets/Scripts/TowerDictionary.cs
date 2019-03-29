using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//                          Base      Rapid Fire        Laser          Slow           Ice       //
public enum TOWER_TYPE { TOWER_BASE, TOWER_RAPID_1, TOWER_RAPID_2, TOWER_SLOW_1, TOWER_SLOW_2 };
public enum PROJECTILE_TYPE { PROJ_BULLET, PROJ_LASER };

// Not actually a dictionary (Can't add elements to a dictionary through the inspector like lists, so this is an alternative). //
public class TowerDictionary : MonoBehaviour
{
    // Can't add elements through the inspector if the list is static, but having a static list  //
    //    removes the need to add TowerDictionary variables to objects that need the tower data. //
    //    Maybe convert this to a singleton... (Probably not).                                   //

    public List<TowerData> towerData; 
    public static List<TowerData> towerDataGlobal;

    void Start()
    {
        towerDataGlobal = towerData;
    }

    public TowerData GetTowerData(TOWER_TYPE whichType)
    {
        return null;
    }
}
