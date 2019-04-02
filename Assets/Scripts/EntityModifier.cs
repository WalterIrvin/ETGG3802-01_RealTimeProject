using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum MODIFIER_TYPE { MOD_T_CONST, MOD_T_TIMED, MOD_T_DECAY };
public enum MODIFIER_EFFECT { MOD_NONE, MOD_SLOW, MOD_SLOW_HEAVY, MOD_FREEZE, MOD_ANTIFREEZE };

/*
public struct EntityModifierRelay
{
    public EntityModifier entMod;
    public float currentTimeLeft;
};
*/

public class EntityModifier : MonoBehaviour
{
    public string modName;
    public MODIFIER_EFFECT modEffect;
    public float modValue;
    public float timeStart, timeMax, timeCur;

    /*
    public string modName;
    public int overrideValue;
    public MODIFIER_TYPE modType;
    public MODIFIER_EFFECT modEffect;
    public float modValue;                   
    public float maxTimeLeft;               
    public float startTime;
    public List<EntityModifier> timeEndEffects; // Effects to add when this one wears off. (i.e. Freeze -> Slow & Anti-Freeze) //
    */


    // May need this (or something similar) later. //
    /*
    public static float GetCurrentModifierValue(EntityModifierRelay emr)
    {
        float result = emr.entMod.modValue;

        switch(emr.entMod.modType)
        {
            case MODIFIER_TYPE.MOD_T_CONST:
                break;

            case MODIFIER_TYPE.MOD_T_TIMED:
                break;

            case MODIFIER_TYPE.MOD_T_DECAY:
                result *= (emr.currentTimeLeft / emr.entMod.maxTimeLeft);
                break;

            default:
                result = 0.0f;
                print("Tried to get the value of an EntityModifier type that doesn't exist!");
                break;
        }

        return result;
    }
    */
}
