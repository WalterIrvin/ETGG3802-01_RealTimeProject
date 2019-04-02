using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScript : MonoBehaviour
{
    //public Text MoneyText;
    public int Money = 1000;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //MoneyText.text = "Money: " + Money.ToString();
    }

    void ChangeMoney(int amt)
    {
        Money += amt;
    }
}
