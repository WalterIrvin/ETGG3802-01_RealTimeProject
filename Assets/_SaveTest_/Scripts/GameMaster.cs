using System.IO;
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
    public string mapFileName;
    public TextAsset saveData;

    public BasicBaseScript playerBase;
    public List<EnemySpawnerScript> enemySpawners;

    public GameObject mapTilePrefab;
    private Material tileMaterial;
    public Material highlightMaterial;
    public GameObject breakableTilePrefab;
    public GameObject mapTileParent;
    public Camera mapCamera;

    private TileObject[,] tileObjects;
    private int[] currentTile = new int[2]{0, 0};

    void Start()
    {
        tileMaterial = mapTilePrefab.GetComponent<MeshRenderer>().sharedMaterial;

        LoadLevel();
    }

    void Update()
    {

    }

    public void LoadLevel()
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

        tileObjects[playerBase.mapZ, playerBase.mapX].Type = TILE_TYPE.TILE_BASE;
        playerBase.transform.position = new Vector3(playerBase.mapX - 8, playerBase.transform.localScale.y / 2, 7 - playerBase.mapZ);

        EnemySpawnerScript ESS;
        foreach(EnemySpawnerScript E in enemySpawners)
        {
            ESS = E;
            tileObjects[ESS.mapZ, ESS.mapX].Type = TILE_TYPE.TILE_SPAWNER;
            ESS.transform.position = new Vector3(ESS.mapX - 8, ESS.transform.localScale.y / 2, 7 - ESS.mapZ);
        }

        mapFile.Close();
    }

    public void SaveLevel()
    {

    }
}
