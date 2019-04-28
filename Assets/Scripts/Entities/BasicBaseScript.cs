using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicBaseScript : MonoBehaviour
{
    public int mapX, mapZ;

    public int health, maxHealth;
    [SerializeField] private Image healthBar;
    //public Text gameover;
    private AudioSource source;
    public float AudioFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        //gameover.text = "";
        //maxHealth = health;
        source = GetComponent<AudioSource>();
        //healthBar = transform.Find("BaseCanvas").Find("healthBG").Find("health").GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (this.health <= 0)
        {
            //loading the gameover scene
            Destroy(gameObject);
            SceneManager.LoadScene(2);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Driller")
        {
            source.Play(0);
            Destroy(other.gameObject);
            this.health -= 10;
            RefreshFillAmount();
        }

    }

    public void RefreshFillAmount()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
    }

    public void UpdateVolume(float n_vol)
    {
        source.volume = n_vol * AudioFactor;
    }
}
