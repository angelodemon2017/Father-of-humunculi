using UnityEngine;

/// <summary>
/// Thinking about logic
/// Need think about animation
/// </summary>
public class ModelController : MonoBehaviour
{
    [SerializeField] private int _condition;
    [SerializeField] private GameObject _option;

    public void SomeCheck(int currentValue)
    {
        _option.SetActive(currentValue > _condition);
    }
}