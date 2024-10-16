using UnityEngine;

public class EntityMonobeh : MonoBehaviour
{
    [SerializeField] private PrefabsByComponent _prefabsByComponent;

    private EntityInProcess _entityInProcess;

    public long Id => _entityInProcess.Id;

    public void Init(EntityInProcess entityInProcess)
    {
        _entityInProcess = entityInProcess;
        transform.position = _entityInProcess.Position;

        entityInProcess.UpdateEIP += UpdateUI;
        InitComponents();
    }

    private void InitComponents()
    {
        foreach (var cd in _entityInProcess.EntityData.Components)
        {
            var pbc = _prefabsByComponent.GetPrefab(cd.KeyName);
            if (pbc != null)
            {
                var newpbc = Instantiate(pbc, transform);
                newpbc.Init(cd, _entityInProcess);
            }
            cd.Init(transform);
        }
        _entityInProcess.UpdateEntity();
    }

    public void Touching(string paramTouch = "")
    {
        _entityInProcess.SendCommand(_entityInProcess.GetCommand(paramTouch));
    }

    private void UpdateUI()
    {

    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateUI;
    }
}