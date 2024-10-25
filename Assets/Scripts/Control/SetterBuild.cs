using UnityEngine;

public class SetterBuild : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _mask;

    private RaycastHit hit;

    public void Init(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private void Update()
    {
        UpdatePosition();
        CheckColliders();
    }

    private void UpdatePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, _mask))
        {
            transform.position = hit.point;
        }
    }

    private void CheckColliders()
    {
        _spriteRenderer.color = true ? Color.green : Color.red;
    }
}