using UnityEngine;

public class MouseInterfaceInteraction : MonoBehaviour
{
    [SerializeField] private EntityMonobeh _linkParent;

    public EntityMonobeh EM => _linkParent;

    public void Click()
    {

    }
}