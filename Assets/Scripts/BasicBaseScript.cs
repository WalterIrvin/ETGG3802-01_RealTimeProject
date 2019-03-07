using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicBaseScript : MonoBehaviour
{

    public int health = 100;
    private float maxHealth;
    private Image healthBar;
    public Text gameover;
    

    // Start is called before the first frame update
    void Start()
    {
        gameover.text = "";
        maxHealth = health;
        healthBar = transform.Find("BaseCanvas").Find("healthBG").Find("health").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0)
        {
            gameover.text = "GAME OVER";
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Driller")
        {
            Destroy(other.gameObject);
            this.health -= 10;
            healthBar.fillAmount = (float)health / (float)maxHealth;
        }

    }
}
