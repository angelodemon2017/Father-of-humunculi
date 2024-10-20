using UnityEngine;

[CreateAssetMenu(menuName = "ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
    public EnumItem EnumKey;
    public string Key;//localize key, dictionary key, etc...
    public int AmountStack;
    public int BaseDurability;//max durability
    public Sprite IconItem;
    public int BaseQuality;
    public string BaseMeta;
    public int MinSpawnItem;
    public int MaxSpawnItem;
}