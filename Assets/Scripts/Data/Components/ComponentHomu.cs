using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComponentHomu : ComponentData
{
    private Dictionary<EnumHomuType, Color> colors = new()
    {
        { EnumHomuType.None, Color.white },
        { EnumHomuType.Dummy, Color.white },
        { EnumHomuType.Stone, Color.gray },
        { EnumHomuType.Wood, Color.yellow },
        { EnumHomuType.Leaf, Color.green },
    };

    private Dictionary<string, EnumHomuType> mutats = new()
    {
        { "Stone", EnumHomuType.Stone },
        { "Wood", EnumHomuType.Wood },
        { "Leaf", EnumHomuType.Leaf },
    };

    private EnumHomuType _homuType = EnumHomuType.Dummy;
    public Color _colorModelDemo => colors[_homuType];
    public string _titleDemo => $"Homu {_homuType}";

    public ComponentHomu()
    {

    }

    public ComponentHomu(EnumHomuType homuType)
    {
        SetHomuType(homuType);
    }

    public void ApplyRecipe(RecipeSO recipe)
    {
        SetHomuType(recipe.homuType);
    }

    private void SetHomuType(EnumHomuType homuType)
    {
        _homuType = homuType;
    }

    internal override void UpdateAfterEntityUpdate(EntityData entity)
    {
        if (_homuType == EnumHomuType.None || _homuType == EnumHomuType.Dummy)
        {
            var invs = entity.Components.GetComponents(typeof(ComponentInventory).Name);
            foreach (ComponentInventory i in invs)
            {
                var targetI = i.Items.FirstOrDefault(x => mutats.ContainsKey(x.Id));
                if (targetI != null)
                {
                    ChangeType(entity, i, targetI);
                }
            }
        }
    }

    private void ChangeType(EntityData entity, ComponentInventory i, ItemData targetI)
    {
        SetHomuType(mutats[targetI.Id]);
        i.SubtrackItems(targetI.Id, 1);

        ComponentFSM compFSM = entity.Components.GetComponent<ComponentFSM>();
        if (compFSM != null)
        {
            compFSM.EntityTarget = UIPlayerManager.Instance.EntityMonobeh.Id;
        }
    }
}