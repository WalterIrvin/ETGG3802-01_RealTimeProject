using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Counter : MonoBehaviour
{
    public string prePendedText;
    public string countedText;
    private Text ourText;
    void Start()
    {
        ourText = this.gameObject.GetComponent<Text>();
        InvokeRepeating("setText", 0f, 0.5f);
    }

    public void setCounted(int input)
    {
        countedText = input.ToString();
    }
    public void setCounted(float input)
    {
        countedText = input.ToString();
    }
    public void setText()
    {
        ourText.text = prePendedText + " " + countedText;
    }
}
