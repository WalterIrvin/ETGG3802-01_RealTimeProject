using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyCounter : MonoBehaviour
{
    public MoneyScript moneyHandler;
    public Counter attachedCounter;

    void Update()
    {
        attachedCounter.setCounted(moneyHandler.Money);
    }
}
