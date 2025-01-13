using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DebugPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugText;
    [SerializeField] private Image _backGround;

//    private bool _isEnable;
    private Dictionary<string, string> _params = new();

    private void Awake()
    {
        InitEvents();
    }

    private void InitEvents()
    {
        WorldViewer.FocusChunkChanged += FocusChunk;
        WorldViewer.CashEntitiesCount += CountCashEnts;
        GameProcess.CountCashEIP += CountEIPs;
        CameraController.ChangePosition += CameraFocus;
    }

    private void Update()
    {
        CalcFPS();
        CalcWorldData();
        UpdatePanel();
    }

    private void UpdatePanel()
    {
        string totalText = string.Empty;
        foreach (var par in _params)
        {
            totalText += par.Key + ": " + par.Value + "\r\n";
        }
        _debugText.text = totalText;
    }

    private void UpdateField(string key, string val)
    {
        if (_params.ContainsKey(key))
        {
            _params[key] = val;
        }
        else
        {
            _params.Add(key, val);
        }
    }

    private void CameraFocus(Vector3 posCamera)
    {
        UpdateField("Camera Focus", $"{posCamera.x.SimpleFormat()}, {posCamera.z.SimpleFormat()}");
    }

    private void FocusChunk(Vector3 posChunk)
    {
        UpdateField("FocusChunk", $"{posChunk.x}, {posChunk.z}");
    }

    private void CountCashEnts(int count)
    {
        UpdateField("Count Cash MonoEntity", $"{count}");
    }

    private void CountEIPs(int count)
    {
        UpdateField("Count Cash EIPs", $"{count}");
    }

    private void CalcWorldData()
    {
        var gw = GameProcess.Instance.GameWorld;
        UpdateField("Data Entities", $"{gw.CountEntityData}");
    }

    private float deltaTime;
    private void CalcFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        UpdateField("FPS", fps.ToString("0:0."));
//        fpsText.text = string.Format("FPS: {0:0.} ", fps);
    }
}