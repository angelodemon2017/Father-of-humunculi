using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private RectTransform _element;

    private void Update()
    {
        _element.position = Input.mousePosition;
    }
}