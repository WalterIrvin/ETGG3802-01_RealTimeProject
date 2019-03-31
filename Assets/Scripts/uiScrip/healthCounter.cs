using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthCounter : MonoBehaviour
{
    public BasicBaseScript playerBase;
    public Counter attachedCounter;

    void Update()
    {
        attachedCounter.setCounted(playerBase.health);
    }
}
