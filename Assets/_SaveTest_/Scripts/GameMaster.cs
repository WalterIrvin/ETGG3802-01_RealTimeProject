using UnityEngine;

public enum TILE_TYPE { TILE_PATH, TILE_TILE, TILE_BREAKABLE, TILE_START, TILE_END }

public struct TileObject
{
    public TILE_TYPE Type;
    public GameObject Tile;
    public TowerScript Tower;
}

public class GameMaster : MonoBehaviour
{
    public TextAsset mapFile;
    public TextAsset saveData;
    public GameObject mapTilePrefab;
    public GameObject mapTileParent;

    private TileObject[,] tileObjects;

    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        TileObject newTileObject;
        tileObjects = new TileObject[16, 16];
    
        for(int z = 0; z < 16; z++)
        {
            for(int x = 0; x < 16; x++)
            {
                newTileObject = new TileObject();

                switch(mapFile.text[(z * 18) + x]) 
                {
                    case '0':
                        newTileObject.Type = TILE_TYPE.TILE_PATH;
                        break;

                    case '1':
                        newTileObject.Type = TILE_TYPE.TILE_TILE;

                        newTileObject.Tile = Instantiate(mapTilePrefab);
                        newTileObject.Tile.transform.position = new Vector3(x - 8, newTileObject.Tile.transform.localScale.y / 2, z - 8);
                        newTileObject.Tile.transform.SetParent(mapTileParent.transform);
                        break;

                    default:
                        break;
                }

                tileObjects[z,x] = newTileObject;
            }
        }

    }

    public void SaveLevel()
    {

    }
}
