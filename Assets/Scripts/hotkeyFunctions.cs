using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotkeyFunctions : MonoBehaviour
{
    uiManager uiMa;
    // Start is called before the first frame update
    void Start()
    {
        uiMa = gameObject.GetComponent<uiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            uiMa.BuyTower();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            uiMa.UpgradeTower();
        }
    }
}
