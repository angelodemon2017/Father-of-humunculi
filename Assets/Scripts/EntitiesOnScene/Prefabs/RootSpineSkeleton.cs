using UnityEngine;
using static OptimazeExtensions;

public class RootSpineSkeleton : PrefabByComponentData
{
    [SerializeField] private GameObject _animatModel;

    public override int KeyComponentData => TypeCache<RootSpineSkeleton>.IdType;

    private void Awake()
    {
        VerticalMirror();
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        VerticalMirror();
    }

    private void VerticalMirror()
    {
        var tempScale = _animatModel.transform.localScale;
        tempScale.x = Random.Range(0, 10) > 5 ? tempScale.x : -tempScale.x;
        _animatModel.transform.localScale = tempScale;
    }
}