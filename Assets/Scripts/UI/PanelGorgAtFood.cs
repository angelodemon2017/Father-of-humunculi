using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelGorgAtFood : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textLabel;
    [SerializeField] private Image _backImage;

    internal void UpdateUI(string name, int count)
    {
        _textLabel.text = $"{name}:{count}";
        _backImage.fillAmount = count / 100f;

        _backImage.color = 
            count >= ComponentHunger.Gorgfatal ? Color.red :
            count >= ComponentHunger.Gorgbad ? Color.yellow :
            Color.green;
    }
}