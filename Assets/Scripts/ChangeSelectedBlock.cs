using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSelectedBlock : MonoBehaviour
{
    public GameObject currentlySelectedBlock;
    public Material selectedMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentlySelectedBlock.GetComponent<Renderer>().material.color = selectedMat.color;
    }
}
