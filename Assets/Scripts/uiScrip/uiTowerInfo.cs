using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiTowerInfo : MonoBehaviour
{
    public Text towerName, towerCost, towerDamage, towerDelay, towerSpecial;

    void Start()
    {
        
    }

    public void UpdateUIText(string towerType)
    {
        TowerData towerData = TowerDictionary.GetTowerData(towerType);
        if(towerData != null)
        {
            towerName.text     = towerData.towerName;
            towerCost.text     = "";
            towerDamage.text   = "";
            towerDelay.text    = "";

            switch(towerData.bulletEffect)
            {
                case MODIFIER_EFFECT.MOD_NONE:
                    break;

                case MODIFIER_EFFECT.MOD_SLOW:
                    break;

                case MODIFIER_EFFECT.MOD_SLOW_HEAVY:
                    break;

                default:
                    break;
            }
        }
    }

}
