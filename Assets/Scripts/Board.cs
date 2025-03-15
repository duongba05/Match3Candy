using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Row[] rows;
    public Tile[,] tiles { get; private set; }
    public int width => tiles.GetLength(0); 
    public int height => tiles.GetLength(1);

    private readonly List<Tile> _selection = new List<Tile>();

    private const float TweenDuration = 0.25f;
    private void Awake()=>Instance = this;

    private void Start()
    {
        tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
        for(var y=0; y < height; y++)
        {
            for(var x=0; x < width; x++)
            {
                var tile = rows[y].tiles[x];
                tile.x = x;
                tile.y = y;
                tile.item = itemDatabase.Items[Random.Range(0, itemDatabase.Items.Length)];
                tiles[x,y] = tile;
            }
        }
    }
    public async void Select(Tile tile)
    {
        if (!_selection.Contains(tile)) _selection.Add(tile); 
        if (_selection.Count < 2) return;
        Debug.Log($"Selected tiles at({_selection[0].x},{_selection[0].y} and {_selection[1].x},{_selection[1].y})");
        await Swap(_selection[0], _selection[1]);
    }
    public async Task Swap(Tile tile1, Tile tile2)
    {
        var icon1 = tile1.icon; 
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play().AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);
        
        tile1.icon = icon2;
        tile2.icon = icon1;

        var tileItem = tile1.item;

        tile1.item = tile2.item;
        tile2.item = tile1.item;
    }

}
