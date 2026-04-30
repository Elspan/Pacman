using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;  // ← cette ligne manquait !
    public TileBase wallTile; 
    public TileBase floorTile;

    public GameObject dotPrefab;
    public GameObject playerPrefab;
    public GameObject ghostPrefab;

    void Start()
    {
        LoadRandomMap();
    }

    public void LoadRandomMap()
    {
        string[][] validMaps = System.Array.FindAll(MapData.maps, m => m != null && m.Length > 0);
        if (validMaps.Length == 0)
        {
            Debug.LogError("MapGenerator: aucune map valide dans MapData.maps");
            return;
        }
        int index = Random.Range(0, validMaps.Length);
        LoadMap(validMaps[index]);
    }

    void LoadMap(string[] map)
    {
        tilemap.ClearAllTiles();

        foreach (var tag in new[] { "Dot", "Player", "Ghost" })
            foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
                Destroy(obj);

        for (int y = 0; y < map.Length; y++)
        {
            string row = map[y];
            for (int x = 0; x < row.Length; x++)
            {
                char cell = row[x];
                Vector3Int pos = new Vector3Int(x, -y, 0);
                Vector3 worldPos = tilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);

                switch (cell)
                {
                    case 'X':
                        tilemap.SetTile(pos, wallTile);
                        break;
                    case '.':
                        tilemap.SetTile(pos, floorTile);
                        Instantiate(dotPrefab, worldPos, Quaternion.identity);
                        break;
                    case 'P':
                        tilemap.SetTile(pos, floorTile);
                        Instantiate(playerPrefab, worldPos, Quaternion.identity);
                        break;
                    case 'G':
                        tilemap.SetTile(pos, floorTile);
                        Instantiate(ghostPrefab, worldPos, Quaternion.identity);
                        break;
                    default:
                        tilemap.SetTile(pos, floorTile);
                        break;
                }
            }
        }
    }
}