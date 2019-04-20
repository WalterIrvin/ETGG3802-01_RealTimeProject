using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE { TILE_PATH, TILE_TILE, TILE_BREAKABLE, TILE_SPAWNER, TILE_BASE }

public struct TileObject
{
    public TILE_TYPE Type;
    public GameObject Tile;
    public TowerScript Tower;
}

public class GameMaster : MonoBehaviour
{
    private int playerHealth, playerMoney, currentWave;

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
    public TowerScript towerPrefab;
    public GameObject towerRangeIndicator;

    private TileObject[,] tileObjects;
    private TileObject prevTile, curTile, selTile, dummyTile;
    private bool tileIsSelected = false;
    private int[] curCoords = new int[2];
    private int[] selCoords = new int[2];

    void Start()
    {
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
                selTile.Tile.GetComponent<MeshRenderer>().material = selectMaterial;
        }
        else
        {   
            if(prevTile.Tile != null && prevTile.Tile != selTile.Tile)
                prevTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;
        }
        
        if(tileIsSelected)
        {
            if(Input.GetKeyDown(KeyCode.B))
                BuildTower("BASE");

            if(Input.GetMouseButtonDown(1))
                DeselectTile();
        }

        if(Input.GetKeyDown(KeyCode.S))
            SaveLevel();
    }

    private void HighlightTile()
    {
        if(prevTile.Tile != null)
        {
            prevTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;
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
        {
            towerRangeIndicator.SetActive(true);
            towerRangeIndicator.transform.position = new Vector3(selCoords[0] - 8, 0.166f, 7 - selCoords[1]);
            towerRangeIndicator.GetComponent<MeshRenderer>().material = selTile.Tower.GetMaterial();

            float range = selTile.Tower.GetRange();
            towerRangeIndicator.transform.localScale = new Vector3(range * 2, 0.01f, range * 2);
        }
        else
            towerRangeIndicator.SetActive(false);

    }

    private void DeselectTile()
    {
        selTile.Tile.GetComponent<MeshRenderer>().material = tileMaterial;
        selTile = dummyTile;
        tileIsSelected = false;
    }

    private bool RaycastToTile()
    {
        if(curTile.Tile != null)
            prevTile = curTile;

        RaycastHit rayHit;
        Ray cameraRay = mapCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out rayHit, mapCamera.transform.position.y * 2f, 1 << 10))
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

    private void BuildTower(string towerType)
    {
        if(selTile.Tower == null)
        {
            selTile.Tower = Instantiate(towerPrefab);

            selTile.Tower.SetTowerData(TowerDictionary.GetTowerData(towerType));
            selTile.Tower.transform.position = selTile.Tile.transform.position;

            tileObjects[selCoords[1], selCoords[0]] = selTile;
        }
    }

    private void LoadLevel()
    {
        TileObject newTileObject;
        tileObjects = new TileObject[16, 16];

        StreamReader mapFile = new StreamReader("Assets/_SaveTest_/TextFiles/MapFiles/" + mapFileName + ".txt");
        string mapLine;
        
        mapLine = mapFile.ReadLine(); // "//LAYOUT//"
        mapLine = mapFile.ReadLine(); // "//Top Left is (0,0)// '+' for path ' ' for tile"
        mapLine = mapFile.ReadLine(); // "0----------------0"
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
        mapLine = mapFile.ReadLine(); // "0----------------0"
        mapLine = mapFile.ReadLine(); // " 0123456789ABCDEF "

        if(loadSave)
        {
            mapFile.Close();
            mapFile = new StreamReader("Assets/_SaveTest_/TextFiles/SaveFiles/" + saveFileName + ".txt");
        }

        mapLine = mapFile.ReadLine(); // "//BREAKABLES//"

        int newX, newZ;
        mapLine = mapFile.ReadLine();
        mapLine = mapLine.Trim();
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

        mapLine = mapFile.ReadLine(); // "//TOWERS//"

        mapLine = mapFile.ReadLine();
        mapLine = mapLine.Trim();
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

            mapLine = mapFile.ReadLine();
            mapLine = mapLine.Trim();
        }

        mapLine = mapFile.ReadLine(); // "//PLAYER_NUMBERS// HEALTH--MONEY--WAVE_NUM"

        playerHealth = int.Parse(mapFile.ReadLine().Trim());
        playerMoney  = int.Parse(mapFile.ReadLine().Trim());
        currentWave  = int.Parse(mapFile.ReadLine().Trim());

        mapFile.Close();

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
        saveFile.WriteLine(playerHealth.ToString());
        saveFile.WriteLine(playerMoney.ToString());
        saveFile.WriteLine(currentWave.ToString());

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
}
