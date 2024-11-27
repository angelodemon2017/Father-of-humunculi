using UnityEngine;

public class TestColliderChecker : MonoBehaviour
{
    [SerializeField] private HomuPresentPBCD _homuPresentPBCD;

    private void OnTriggerEnter(Collider other)
    {
        _homuPresentPBCD.CheckCollider(other);
    }
}