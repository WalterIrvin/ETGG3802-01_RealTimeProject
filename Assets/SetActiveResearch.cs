using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveResearch : MonoBehaviour
{
    // Start is called before the first frame update
    public TowerButtonResearchTab activeResearch = null;
    public void setActiveResearch(TowerButtonResearchTab obj)
    {
        activeResearch = obj;
    }
}
