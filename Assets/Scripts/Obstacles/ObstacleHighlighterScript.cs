using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHighlighterScript : MonoBehaviour
{
    public Material oldMaterial;
    public Material highlightMaterial;

    private void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = highlightMaterial.color;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = oldMaterial.color;
    }
}
