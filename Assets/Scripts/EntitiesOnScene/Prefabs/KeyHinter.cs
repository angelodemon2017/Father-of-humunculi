using TMPro;
using UnityEngine;

public class KeyHinter : MonoBehaviour// PrefabByComponentData
{
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private EntityMonobeh _entityMonobeh;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private EnumControlInputPlayer _actionForPickUp;
    [SerializeField] private string _desc;
    [SerializeField] private bool StopInteract;

    private bool _isShow;

    public EntityMonobeh Entity => _entityMonobeh;
    public bool IsPressActionButton => _actionForPickUp.CheckAction(StopInteract);

    private void Awake()
    {
        _sphereCollider.radius = Config.DistanceForShowHeyHint;
        _tip.SetActive(false);
        var act = GameplayClient.Instance.GetCA(_actionForPickUp);
        _tipText.text = $"{act.keyCode} {_desc}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var entInt = other.gameObject.GetComponent<EntityInteractabler>();
            if (entInt == null)
            {
                return;
            }

            entInt.AddKeyHinter(this);
            _isShow = true;
            _tip.SetActive(_isShow);
        }
    }

    public void Disconect()
    {
        _isShow = false;
        _tip.SetActive(_isShow);
    }
}