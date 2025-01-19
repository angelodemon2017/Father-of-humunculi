using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIPresentHunger : MonoBehaviour
{
    [SerializeField] private PanelGorgAtFood _prefabPanelGorgAtFood;
    [SerializeField] private Transform _parentGorging;

    [SerializeField] private TextMeshProUGUI _textHunger;
    [SerializeField] private TextMeshProUGUI _textHP;

    private ComponentHPData _componentHPData;
    private ComponentHunger _componentHunger;
    private EntityInProcess _eip;
    private List<PanelGorgAtFood> _cashPGAF = new();

    internal void Init(ComponentHunger componentHunger, EntityInProcess entityInProcess)
    {
        _componentHunger = componentHunger;
        _eip = entityInProcess;
        _eip.UpdateEIP += UpdateUI;
    }

    internal void InitHP(ComponentHPData componentHP, EntityInProcess entityInProcess)
    {
        _componentHPData = componentHP;
    }

    private void UpdateUI()
    {
        _textHunger.text = $"{_componentHunger.Starvation}/{_componentHunger.MaxStarvation}";
//        if (_componentHPData != null)
//        {
            _textHP.text = $"HP:{_componentHPData.CurrentHP}/{_componentHPData.MaxHP}";
        //        }
        UpdateGorging();
    }

    private void UpdateGorging()
    {
        var tempList = _componentHunger.Gorging;
        for (int i = 0; i < tempList.Count; i++)
        {
            if (_cashPGAF.Count < i + 1)
            {
                _cashPGAF.Add(Instantiate(_prefabPanelGorgAtFood, _parentGorging));
            }
            _cashPGAF[i].UpdateUI(tempList[i].Key, tempList[i].Value);
        }
        for (int i = _cashPGAF.Count - 1; i >= tempList.Count; i--)
        {
            Destroy(_cashPGAF[i].gameObject);
            _cashPGAF.RemoveAt(i);
        }
    }

    private void OnDestroy()
    {
        _eip.UpdateEIP -= UpdateUI;
    }
}