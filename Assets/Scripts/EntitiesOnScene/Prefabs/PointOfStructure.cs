using System.Collections.Generic;
using UnityEngine;

public class PointOfStructure : MonoBehaviour
{
    [SerializeField] private List<VariantEntity> _variantEntities = new();
    [SerializeField] private List<ItemConfig> _givingItems = new();

    public ItemData GivingItem => new ItemData(_givingItems.GetRandom());

    private void Awake()
    {
        Destroy(gameObject);
    }

    public void Init(EntityData entityData)
    {
        var varEnt = _variantEntities.GetRandom().Prefab;
        if (varEnt != null)
        {
            var newEnt = varEnt.CreateEntity(transform.position.x + entityData.Position.x,
                transform.position.z + +entityData.Position.z);

            if (newEnt.TypeKey == Dict.SpecComponents.ItemPresent)
            {
                var compItem = newEnt.GetComponent<ComponentItemPresent>();
                if (compItem != null)
                {
                    compItem.SetItem(GivingItem);
                }
            }
            GameProcess.Instance.GameWorld.AddEntity(newEnt);
        }
    }
}

[System.Serializable]
public class VariantEntity
{
    public int Weight = 1;
    public EntityMonobeh Prefab;
}