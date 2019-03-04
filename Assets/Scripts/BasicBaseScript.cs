using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaseScript : MonoBehaviour
{

    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Driller")
        {
            Destroy(other.gameObject);
            this.health -= 10;
        }

    }
}
