using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TILE_TYPE { TILE_PATH, TILE_TILE, TILE_BREAKABLE, TILE_SPAWNER, TILE_BASE }

public struct TileObject
{
    public TILE_TYPE Type;
    public GameObject Tile;
    public TowerScript Tower;
}

public class GameMaster : MonoBehaviour
{
    public MoneyScript playerMoney;

    public bool loadSave;
    public string mapFileName, saveFileName;

    public BasicBaseScript playerBase;
    public List<EnemySpawnerScript> enemySpawners;

    public GameObject mapTilePrefab;
    private Material tileMaterial;
    public Material highlightMaterial;
    public Material selectMaterial;
    public GameObject breakableTilePrefab;
    public GameObject mapTileParent;
    public Camera mapCamera;
    public GameObject mapGround;
    public TowerScript towerPrefab;
    public GameObject towerRangeIndicator;

    private TileObject[,] tileObjects;
    private TileObject prevTile, curTile, selTile, dummyTile;
    private bool tileIsSelected = false;
    private int[] curCoords = new int[2];
    private int[] selCoords = new int[2];
    private Material currentSelectMaterial;

    //===========//
    // uiManager //
    //===========//
    public string baseTowerType;
    private string currentTowerType;

    public Text moneyText;
    public Text towerCostText;
    public uiTowerInfo towerInfoText;
    //===============//
    // End uiManager //
    //===============//

    [SerializeField] private ResearchScript researchMaster;
    [SerializeField] private WaveHandler waveMaster;

    void Start()
    {
        currentTowerType = baseTowerType;
        currentSelectMaterial = selectMaterial;
        towerRangeIndicator.SetActive(false);

        tileMaterial = mapTilePrefab.GetComponent<MeshRenderer>().sharedMaterial;
        dummyTile = new TileObject();
        prevTile = curTile = selTile = dummyTile;

        LoadLevel();
    }

    void Update()
    {
        if(RaycastToTile())
        {
            HighlightTile();

            if(Input.GetMouseButton(0))
                SelectTile();

            if(selTile.Tile != null)
                selTile.Tile.GetComponent<MeshRenderer>().material = currentSelectMaterial;
        }
        else
        {   
            if(prevTile.Tile != null && prevTile.Tile != selTile.Tile)
                prevTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;
        }
        
        if(tileIsSelected)
        {
            if(Input.GetMouseButtonDown(1))
                DeselectTile();
        }

        // TowerController //
        // Centered camera rotation ||| Middle Mouse Wheel ||| Alt + Mouse 1 //
        if ((Input.GetMouseButton(2) && !Input.GetButton("Camera Control Modifier")) || (Input.GetButton("Camera Control Modifier") && Input.GetMouseButton(0)))
        {
            RotateCamera_YAxis(Input.GetAxis("Horizontal") * 10);
            RotateCamera_XAxis(Input.GetAxis("Vertical") * 10);
        }

        // Zooming in and out ||| Mouse wheel up and down //
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomCamera(Input.mouseScrollDelta.y);
        }

        mapCamera.transform.LookAt(new Vector3(0, 1, 0));
        // End TowerController //
    }

    private void RefreshTotalCostText()
    {
        int buyAmount, sellAmount;
        TowerDictionary.GetValueTotals(currentTowerType, out buyAmount, out sellAmount);

        if (selTile.Tower == null)
            towerCostText.text = "Cost: " + buyAmount.ToString();
        else
        {
            if(TowerDictionary.IsValidUpgrade(selTile.Tower.GetTowerType(), currentTowerType, true))
            {
                int buyAmount2, sellAmount2;
                TowerDictionary.GetValueTotals(selTile.Tower.GetTowerType(), out buyAmount2, out sellAmount2);
                towerCostText.text = "Cost: " + (buyAmount - buyAmount2).ToString();
            }
            else
                towerCostText.text = "";
        }
    }

    private void RefreshRangeIndicator()
    {
        if(selTile.Tower != null)
        {
            towerRangeIndicator.SetActive(true);
            towerRangeIndicator.transform.position = new Vector3(selCoords[0] - 8, 0.201f, 7 - selCoords[1]);
            towerRangeIndicator.GetComponent<MeshRenderer>().material = selTile.Tower.GetMaterial();

            float range = selTile.Tower.GetRange();
            towerRangeIndicator.transform.localScale = new Vector3(range * 2, 0.01f, range * 2);

            currentSelectMaterial = selTile.Tower.GetSelectMaterial();
        }
        else
        {
            towerRangeIndicator.SetActive(false);
            currentSelectMaterial = selectMaterial;
        }
    }

    private void HighlightTile()
    {
        if(prevTile.Tile != null)
        {
            prevTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;

            if(curTile.Tower != null)
                curTile.Tile.GetComponent<MeshRenderer>().material = curTile.Tower.GetHighlightMaterial();
            else
                curTile.Tile.GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }

    private void SelectTile()
    {
        if(selTile.Tile != null)
            selTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;
       
        selTile = curTile;
        selCoords[0] = curCoords[0];
        selCoords[1] = curCoords[1];
        tileIsSelected = true;

        if(selTile.Tower != null)
            towerInfoText.UpdateUIText(selTile.Tower.GetTowerType());
        //else
        //    towerInfoText.UpdateUIText("NONE");

        RefreshRangeIndicator();
    }

    private void DeselectTile()
    {
        selTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;
        selTile = dummyTile;
        tileIsSelected = false;
        towerRangeIndicator.SetActive(false);
    }

    private bool RaycastToTile()
    {
        if(curTile.Tile != null)
            prevTile = curTile;

        RaycastHit rayHit;
        Ray cameraRay = mapCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out rayHit, mapCamera.transform.position.y * 10f, 1 << 10))
        {
            int newX, newZ;
            newX = (int)(rayHit.point.x + 8.5f);
            newZ = (int)(7.5f - rayHit.point.z);

            if(0 <= newX && newX <= 15 && 0 <= newZ && newZ <= 15)
            {
                if(tileObjects[newZ, newX].Type == TILE_TYPE.TILE_TILE)
                {
                    curTile = tileObjects[newZ, newX];
                    curCoords[0] = newX;
                    curCoords[1] = newZ;
                    return true;
                }
            }
        }

        return false;
    }

    private void LoadLevel()
    {
        TileObject newTileObject;
        tileObjects = new TileObject[16, 16];

        StreamReader mapFile = new StreamReader("Assets/_SaveTest_/TextFiles/MapFiles/" + mapFileName + ".txt");
        string mapLine;
        
        mapFile.ReadLine(); // "//LAYOUT//"
        mapFile.ReadLine(); // "//Top Left is (0,0)// '+' for path ' ' for tile"
        mapFile.ReadLine(); // "0----------------0"
        for (int z = 0; z < 16; z++)
        {
            mapLine = mapFile.ReadLine();
            for(int x = 0; x < 16; x++)
            {
                newTileObject = new TileObject();

                switch (mapLine[x + 1])
                {
                    case '+':
                        newTileObject.Type = TILE_TYPE.TILE_PATH;
                        break;

                    case ' ':
                        newTileObject.Type = TILE_TYPE.TILE_TILE;

                        newTileObject.Tile = Instantiate(mapTilePrefab);
                        newTileObject.Tile.transform.position = new Vector3(x - 8, newTileObject.Tile.transform.localScale.y / 2, 7 - z);
                        newTileObject.Tile.transform.SetParent(mapTileParent.transform);

                        newTileObject.Tower = null;
                        break;

                    default:
                        print("Unknown tile type!");
                        break;
                }

                tileObjects[z, x] = newTileObject;
            }
        }
        mapFile.ReadLine(); // "0----------------0"
        mapFile.ReadLine(); // " 0123456789ABCDEF "

        if(loadSave)
        {
            mapFile.Close();
            mapFile = new StreamReader("Assets/_SaveTest_/TextFiles/SaveFiles/" + saveFileName + ".txt");
        }

        mapFile.ReadLine(); // "//BREAKABLES//"

        int newX, newZ;
        mapLine = mapFile.ReadLine().Trim();
        while(mapLine != "//END//")
        {
            newX = ((mapLine[1] - 48) * 10) + (mapLine[2] - 48);
            newZ = ((mapLine[4] - 48) * 10) + (mapLine[5] - 48);

            tileObjects[newZ, newX].Type = TILE_TYPE.TILE_BREAKABLE;
        
            tileObjects[newZ, newX].Tile = Instantiate(breakableTilePrefab);
            tileObjects[newZ, newX].Tile.transform.position = new Vector3(newX - 8, tileObjects[newZ, newX].Tile.transform.localScale.y / 2, 7 - newZ);
            tileObjects[newZ, newX].Tile.transform.SetParent(mapTileParent.transform);

            mapLine = mapFile.ReadLine();
            mapLine = mapLine.Trim();
        }

        mapFile.ReadLine(); // "//TOWERS//"

        mapLine = mapFile.ReadLine().Trim();
        while (mapLine != "//END//")
        {
            newX = ((mapLine[1] - 48) * 10) + (mapLine[2] - 48);
            newZ = ((mapLine[4] - 48) * 10) + (mapLine[5] - 48);
            mapLine = mapLine.Remove(0, 8);

            tileObjects[newZ, newX].Tower = Instantiate(towerPrefab);

            tileObjects[newZ, newX].Tower.SetTowerData(TowerDictionary.GetTowerData(mapLine));
            tileObjects[newZ, newX].Tower.transform.position = new Vector3(tileObjects[newZ, newX].Tile.transform.position.x, 
                                                                           tileObjects[newZ, newX].Tile.transform.localScale.y + (tileObjects[newZ, newX].Tower.transform.localScale.y), 
                                                                           tileObjects[newZ, newX].Tile.transform.position.z);

            mapLine = mapFile.ReadLine().Trim();
        }

        mapFile.ReadLine(); // "//PLAYER_NUMBERS// HEALTH--MONEY--WAVE_NUM"

        playerBase.health      = int.Parse(mapFile.ReadLine().Trim());
        playerMoney.Money      = int.Parse(mapFile.ReadLine().Trim());
        waveMaster.currentWave = int.Parse(mapFile.ReadLine().Trim());

        mapFile.ReadLine(); // "//END//"
        mapFile.ReadLine(); // "//RESEARCH_LIST//"

        string tempString;
        mapLine = mapFile.ReadLine().Trim();
        while(mapLine != "//END//")
        {
            TowerDictionary.SetResearch(mapLine.Remove(0, 2).Trim(), int.Parse(mapLine[0].ToString()) == 1);
            mapLine = mapFile.ReadLine().Trim();
        }

        mapLine = mapFile.ReadLine(); // "//RESEARCH_TIME//"

        float tempFloat;
        tempString = mapFile.ReadLine().Trim();
        tempFloat = float.Parse(mapFile.ReadLine().Trim());

        researchMaster.SetValuesWithSave(tempString, tempFloat);

        mapFile.ReadLine(); // "//END//"

        mapFile.Close();

        waveMaster.StartUP();
        playerBase.RefreshFillAmount();
        tileObjects[playerBase.mapZ, playerBase.mapX].Type = TILE_TYPE.TILE_BASE;
        playerBase.transform.position = new Vector3(playerBase.mapX - 8, playerBase.transform.localScale.y / 2, 7 - playerBase.mapZ);

        EnemySpawnerScript ESS;
        foreach (EnemySpawnerScript E in enemySpawners)
        {
            ESS = E;
            tileObjects[ESS.mapZ, ESS.mapX].Type = TILE_TYPE.TILE_SPAWNER;
            ESS.transform.position = new Vector3(ESS.mapX - 8, ESS.transform.localScale.y / 2, 7 - ESS.mapZ);
        }
    }

    public void SaveLevel()
    {
        StreamWriter saveFile = new StreamWriter("Assets/_SaveTest_/TextFiles/SaveFiles/" + saveFileName + ".txt", false);

        List<string> breakables = new List<string>();
        List<string> towers     = new List<string>();
        string coordString = "";
        TileObject curObject;

        for(int z = 0; z < 16; z++)
        {
            for(int x = 0; x < 16; x++)
            {
                curObject = tileObjects[z, x];
                switch(curObject.Type)
                {
                    case TILE_TYPE.TILE_TILE:
                        if(curObject.Tower != null)
                        {
                            coordString = CoordsToString(x, z);
                            coordString += "?" + curObject.Tower.GetTowerType();
                            towers.Add(coordString);
                        }
                        break;

                    case TILE_TYPE.TILE_BREAKABLE:
                        if(curObject.Tile != null)
                        {
                            coordString = CoordsToString(x, z);
                            breakables.Add(coordString);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        saveFile.WriteLine("//BREAKABLES//");
        foreach(string S in breakables)
        {
            saveFile.WriteLine(S);
        }
        saveFile.WriteLine("//END//");

        saveFile.WriteLine("//TOWERS//");
        foreach (string S in towers)
        {
            saveFile.WriteLine(S);
        }
        saveFile.WriteLine("//END//");

        saveFile.WriteLine("//PLAYER_NUMBERS// HEALTH--MONEY--WAVE_NUM");
        saveFile.WriteLine(playerBase.health.ToString());
        saveFile.WriteLine(playerMoney.Money.ToString());
        saveFile.WriteLine(waveMaster.currentWave.ToString());
        saveFile.WriteLine("//END//");

        saveFile.WriteLine("//RESEARCH_LIST//\n" + TowerDictionary.CreateResearchSaveString() + "//END//");

        string tempString; float tempFloat;
        researchMaster.GetValuesForSave(out tempString, out tempFloat);

        saveFile.WriteLine("//RESEARCH_TIME//");
        saveFile.WriteLine(tempString);
        saveFile.WriteLine(tempFloat.ToString());
        saveFile.WriteLine("//END//");

        saveFile.Close();
    }

    private string CoordsToString(int x, int z)
    {
        string result = "(";

        if(x < 10)
            result += "0";
        result += x.ToString();

        result += "|";

        if(z < 10)
            result += "0";
        result += z.ToString();

        result += ")";

        return result;
    }

    //=================//
    // TowerController //
    //=================//
    private void RotateCamera_YAxis(float amt)
    {
        mapCamera.transform.RotateAround(mapGround.transform.position + new Vector3(0, 1, 0), new Vector3(0, 1, 0), amt * Time.deltaTime);
    }

    private void RotateCamera_XAxis(float amt)
    {
        mapCamera.transform.RotateAround(mapGround.transform.position + new Vector3(0, 1, 0), mapCamera.transform.right, amt * Time.deltaTime);
        //In the future it would be a good idea to make it so that the viewing angles are restricted, but I don't think it's that important right now.
    }

    private void ZoomCamera(float dir)
    {
        Vector3 cam_to_center = mapCamera.transform.position - new Vector3(0, 1, 0);
        if ((cam_to_center - cam_to_center.normalized).magnitude >= 3 || (dir * -1) > 0)
        {
            mapCamera.transform.Translate(cam_to_center.normalized * dir * -1, Space.World);
        }
    }
    //=====================//
    // End TowerController //
    //=====================//

    //===========//
    // uiManager //
    //===========//
    public void UpdateMoneyText()
    {
        moneyText.text = "Money: " + playerMoney.Money.ToString();
    }

    public void SetCurrentTowerType(string newType)
    {
        if(!TowerDictionary.GetResearchStatus(newType))
            return;

        currentTowerType = newType;

        TowerData towerData = TowerDictionary.GetTowerData(newType);
        if(towerData != null)
        {
            towerInfoText.UpdateUIText(towerData.towerType);
            RefreshTotalCostText();
        }
        else
            towerCostText.text = "";
    }

    public void BuyOrUpgradeTower()//string whichType)
    {
        if(!tileIsSelected)
            return;

        string whichType = currentTowerType;

        int buyAmount, sellAmount;
        if(selTile.Tower == null) 
        {
            // Buy //
            if(TowerDictionary.GetValueTotals(whichType, out buyAmount, out sellAmount))
            {
                if(playerMoney.Money >= buyAmount)
                {
                    selTile.Tower = Instantiate(towerPrefab);
                    selTile.Tower.SetTowerData(TowerDictionary.GetTowerData(whichType));
                    selTile.Tower.transform.position = selTile.Tile.transform.position + new Vector3(0, selTile.Tile.transform.localScale.y * 2, 0);
                    selTile.Tile.GetComponent<MeshRenderer>().material = selTile.Tower.GetSelectMaterial();
                    tileObjects[selCoords[1], selCoords[0]] = selTile;
                    RefreshRangeIndicator();

                    playerMoney.Money -= buyAmount;
                    UpdateMoneyText();
                    towerInfoText.UpdateUIText(whichType);
                }
            }
        }
        else 
        {
            // Upgrade //
            if(whichType == baseTowerType)
                return;

            if(TowerDictionary.IsValidUpgrade(selTile.Tower.GetTowerType(), whichType, true))
            {
                int buy2, sell2;
                TowerDictionary.GetValueTotals(selTile.Tower.GetTowerType(), out buy2, out sell2);
                TowerDictionary.GetValueTotals(whichType, out buyAmount, out sellAmount);

                if(playerMoney.Money >= (buyAmount - buy2))
                {
                    selTile.Tower.SetTowerData(TowerDictionary.GetTowerData(whichType));
                    selTile.Tile.GetComponent<MeshRenderer>().material = selTile.Tower.GetSelectMaterial();
                    tileObjects[selCoords[1], selCoords[0]] = selTile;
                    RefreshRangeIndicator();

                    playerMoney.Money -= (buyAmount - buy2);
                    UpdateMoneyText();
                    towerInfoText.UpdateUIText(whichType);
                }
            }
        }
    }
    //===============//
    // End uiManager //
    //===============//
}
