using UnityEngine;

// It's the same as uiRelay, but with the GameMaster as the command target. //
// Also, the BuyTower() and UpgradeTower() functions have been combined. //

public class uiRelay_V2 : MonoBehaviour
{
    public string towerType;
    public GameMaster commandTarget;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetCurrentTowerType()
    {
        commandTarget.SetCurrentTowerType(towerType);
    }

    public void BuyOrUpgradeTower()
    {
        commandTarget.BuyOrUpgradeTower();

        if(source != null)
            source.Play(0);
    }
}
