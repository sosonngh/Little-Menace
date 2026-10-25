using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private Transform player;
    [SerializeField] private int generateRadius;

    private void Update()
    {
        Vector3Int playerPos = terrainTilemap.WorldToCell(player.position);

        for (int x = -generateRadius; x <= generateRadius; x++)
        {
            for (int y = -generateRadius; y <= generateRadius; y++)
            {
                Vector3Int tilePos = playerPos + new Vector3Int(x, y, 0);

                if (!terrainTilemap.HasTile(tilePos))
                {
                    TileBase tile = tiles[Random.Range(0, tiles.Length)];
                    terrainTilemap.SetTile(tilePos, tile);
                }
            }
        }
    }
}
