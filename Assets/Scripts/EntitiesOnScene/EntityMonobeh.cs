using UnityEngine;

public class EntityMonobeh : MonoBehaviour
{
    [SerializeField] private PrefabsByComponent _prefabsByComponent;

    private EntityInProcess _entityInProcess;

    public long Id => _entityInProcess.Id;
    public EntityInProcess EntityInProcess => _entityInProcess;

    public void Init(EntityInProcess entityInProcess)
    {
        _entityInProcess = entityInProcess;
        transform.position = _entityInProcess.Position;

        entityInProcess.UpdateEIP += UpdateEntity;
        InitComponents();
    }

    private void InitComponents()
    {
        _entityInProcess.EntityData.Config.InitOnScene(this);

        _entityInProcess.UpdateEntity();
    }

    private void UpdateEntity()
    {
        if (_entityInProcess.EntityIsDeleted)
        {
            GameProcess.Instance.RemoveEIP(_entityInProcess);
            WorldViewer.Instance.RemoveEntity(this);
        }
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateEntity;
    }

    public void SendCommand(CommandData comand)
    {
        _entityInProcess.SendCommand(comand);
    }
}