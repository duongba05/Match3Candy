using System.Linq;
using UnityEngine;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Row[] rows;
    public Tile[,] tiles { get; private set; }
    public int width => tiles.GetLength(0); 
    public int height => tiles.GetLength(1);
    private void Awake()=>Instance = this;

    private void Start()
    {
        tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
        for(var y=0; y < height; y++)
        {
            for(var x=0; x < width; x++)
            {
                var tile = rows[y].tiles[x];
                tiles[x,y] = tile;
                tile.item = itemDatabase.Items[Random.Range(0, itemDatabase.Items.Length)];
            }
        }
    }
}
