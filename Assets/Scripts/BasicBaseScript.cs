using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaseScript : MonoBehaviour
{

    public int health;

    // Start is called before the first frame update
    void Start()
    {
        this.health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.health is 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Driller")
        {
            Destroy(other.gameObject);
            this.health -= 50;
        }

    }
}
