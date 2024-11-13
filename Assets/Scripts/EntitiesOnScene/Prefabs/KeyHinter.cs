using TMPro;
using UnityEngine;

public class KeyHinter : PrefabByComponentData
{
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private EntityMonobeh _entityMonobeh;

    private bool _isShow;

    private void Awake()
    {

    }

    private void Update()
    {

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


        }
    }
}