using TMPro;
using UnityEngine;

public class EntityMonobeh : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private Transform _model;

    private EntityInProcess _entityInProcess;
    private Transform _uicanvas;

    public long Id => _entityInProcess.Id;

    public void Init(EntityInProcess entityInProcess)
    {
        _uicanvas = _testText.transform.parent;
        _entityInProcess = entityInProcess;
        transform.position = _entityInProcess.Position;

        entityInProcess.UpdateEIP += UpdateUI;

        UpdateUI();

        //this calc components...
    }

    public void Touching(int paramTouch = 0)
    {
        _entityInProcess.Touch(paramTouch);
    }

    private void UpdateUI()
    {
        _testText.text = _entityInProcess.TestDebugProp;
        _uicanvas.rotation = Camera.main.transform.rotation;
        _model.rotation = Camera.main.transform.rotation;
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateUI;
    }
}