using System.Collections.Generic;
using UnityEngine;

public class GameplayClient : MonoBehaviour
{
    public static GameplayClient Instance;
    public string PlayerName;

    public List<ControlAction> InputActions = new();

    private Dictionary<KeyCode, ControlAction> _cashInputs = new();
    private Dictionary<EnumControlInputPlayer, ControlAction> _cashActions = new();

    private void Awake()
    {
        Instance = this;
        UpdateInputs();
    }

    private void Update()
    {
        foreach (var i in _cashInputs)
        {
            if (i.Value.ShotKey)
            {
                i.Value.IsPressed = Input.GetKeyDown(i.Key);
            }
            else
            {
                if (!i.Value.IsStop)
                {
                    i.Value.IsPressed = Input.GetKey(i.Key);
                }
                else
                {
                    if (Input.GetKeyUp(i.Key))
                    {
                        i.Value.IsStop = false;
                    }
                }
            }
        }
    }

    private void UpdateInputs()
    {
        _cashInputs.Clear();
        _cashActions.Clear();
        foreach (var ia in InputActions)
        {
            _cashInputs.Add(ia.keyCode, ia);
            _cashActions.Add(ia.controlInputPlayer, ia);
        }
    }

    public ControlAction GetCA(EnumControlInputPlayer controlInputPlayer)
    {
        return _cashActions[controlInputPlayer];
    }

    public bool CheckAction(EnumControlInputPlayer controlInputPlayer, bool isStop = false)
    {
        return _cashActions[controlInputPlayer].GetPressed(isStop);
    }
}

[System.Serializable]
public class ControlAction
{
    public KeyCode keyCode;
    public EnumControlInputPlayer controlInputPlayer;
    public bool IsPressed;
    public bool ShotKey;
    public bool IsStop;

    public bool GetPressed(bool isStop = false)
    {
        IsStop = isStop;

        var result = IsPressed;

        if (ShotKey || IsStop)
        {
            IsPressed = false;
        }        

        return result;
    }
}