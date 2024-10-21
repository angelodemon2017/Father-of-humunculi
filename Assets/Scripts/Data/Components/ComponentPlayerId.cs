using UnityEngine;

public class ComponentPlayerId : ComponentData
{
    public string PlayerName;

    public override void Init(Transform entityME)
    {
        CameraController.Instance.SetTarget(entityME);
        var eip = entityME.GetComponent<EntityMonobeh>().EntityInProcess;
        UIPlayerManager.Instance.InitEntity(eip);
    }
}