using UnityEngine;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [SerializeField] private UIPresentInventory uIPresentInventory;

    private EntityInProcess _entityInProcess;

    private void Awake()
    {
        Instance = this;
        uIPresentInventory.ComponentUpdated += UpdateModules;
    }

    public void InitEntity(EntityInProcess entity)
    {
        _entityInProcess = entity;

        _entityInProcess.UpdateEIP += UpdateModules;

        //TODO cycle init all components
        var ci = entity.EntityData.Components.GetComponent<ComponentInventory>();
        uIPresentInventory.Init(ci);

        UpdateModules();
    }

    private void UpdateModules()
    {
        uIPresentInventory.UpdateSlots();
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateModules;
    }
}