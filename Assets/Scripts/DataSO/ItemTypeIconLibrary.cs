using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Icon Item Type Library", order = 1)]
public class ItemTypeIconLibrary : ScriptableObject
{
    private static ItemTypeIconLibrary _instance;
    public static ItemTypeIconLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.LoadAll<ItemTypeIconLibrary>(string.Empty)[0];
            }

            return _instance;
        }
    }

    [SerializeField] private List<CategoryToIcon> _icons;
    private Dictionary<EnumItemCategory, Sprite> _cashIcons = new();

    public Sprite GetIcon(EnumItemCategory key)
    {
        Init();

        if (_cashIcons.TryGetValue(key, out Sprite result))
        {
            return result;
        }

        return null;
    }

    private void Init()
    {
        if (_cashIcons.Count == 0)
        {
            foreach (var ent in _icons)
            {                
                _cashIcons.Add(ent.KeyItem, ent.Icon);
            }
        }
    }

    [System.Serializable]
    public class CategoryToIcon
    {
        public EnumItemCategory KeyItem;
        public Sprite Icon;
    }
}