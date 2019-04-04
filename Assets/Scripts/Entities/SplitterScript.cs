using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterScript : MonoBehaviour
{
    public int SplitCount;
    private int mValue;
    // Start is called before the first frame update
    void Start()
    {
        int n = 0;
        double sum = 0f;
        while (n < SplitCount)
        {
            sum += Math.Pow(2, n);
            n += 1;
        }

        if (sum  > 0)
            mValue = GetComponent<EnemyMover>().value / (int)sum;

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyMover>().health <= 0 && SplitCount > 0)
        {
            GameObject child1 = Instantiate(gameObject, transform.position, transform.rotation);
            GameObject child2 = Instantiate(gameObject, transform.position + transform.forward, transform.rotation);
            child1.transform.localScale *= 0.9f;
            child2.transform.localScale *= 0.9f;
            child1.GetComponent<EnemyMover>().health = GetComponent<EnemyMover>().maxHealth / 2;
            child2.GetComponent<EnemyMover>().health = GetComponent<EnemyMover>().maxHealth / 2;
            child1.GetComponent<SplitterScript>().SplitCount = SplitCount - 1;
            child2.GetComponent<SplitterScript>().SplitCount = SplitCount - 1;

            child1.GetComponent<EnemyMover>().value = mValue;
            child2.GetComponent<EnemyMover>().value = mValue;
        }
    }
}
