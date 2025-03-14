using UnityEngine;

public static class itemDatabase
{
    public static Item[] Items { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Initiialize() => Items = Resources.LoadAll<Item>("Items/");
}
