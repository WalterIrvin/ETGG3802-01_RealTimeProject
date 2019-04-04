using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiTowerInfo : MonoBehaviour
{
    public Text towerName, towerCost, towerDamage, towerRange, towerSpecial;
   
    void Start()
    {
        towerName.text = towerCost.text = towerDamage.text = towerRange.text = towerSpecial.text = "";
    }

    public void UpdateUIText(string towerType)
    {
        TowerData towerData = TowerDictionary.GetTowerData(towerType);
        if (towerData != null)
        {
            towerName.text   = "[ " + towerData.towerName + " ]";
            towerCost.text   = "Cost: " + towerData.buildCost.ToString();
            towerDamage.text = "Damage: " + (((float)towerData.bulletDamage) / towerData.fireDelay).ToString() + "/s"; 
            towerRange.text  = "Range: " + towerData.detectRange.ToString();

            switch (towerData.bulletEffect)
            {
                case MODIFIER_EFFECT.MOD_SLOW:
                    towerSpecial.text = "Special: Slow";
                    break;

                case MODIFIER_EFFECT.MOD_SLOW_HEAVY:
                    towerSpecial.text = "Special: Greater Slow";
                    break;

                default:
                    towerSpecial.text = "";
                    break;
            }
        }
        else
            towerName.text = towerCost.text = towerDamage.text = towerRange.text = towerSpecial.text = "";
    }

}
