using System.Collections.Generic;
using UnityEngine;
using static OptimazeExtensions;

public class StructureComponent : PrefabByComponentData
{
    public override int KeyType => TypeCache<StructureComponent>.IdType;

    [SerializeField] private List<PointOfStructure> _pointOfStructures = new();

    private void Awake()
    {
        _pointOfStructures.Clear();
    }

    internal override void PrepareEntityBeforeCreate(EntityData entityData)
    {
        _pointOfStructures.ForEach(pos => pos.Init(entityData));
    }
}