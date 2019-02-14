using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmp_EnemyMover : MonoBehaviour
{
    public GameObject tmp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmp.transform.Translate(new Vector3(0, 0, 0.01f));
    }
}
