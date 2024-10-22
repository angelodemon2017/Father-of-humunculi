using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPanelCraft : MonoBehaviour
{
    [SerializeField] private UIIconPresent _prefabResources;

    [SerializeField] private TextMeshProUGUI _titleRecipe;
    [SerializeField] private UIIconPresent _iconResult;
    [SerializeField] private Transform _parentResources;
    [SerializeField] private Button _buttonCraft;

    private void Awake()
    {
        _buttonCraft.onClick.AddListener(OnClick);
    }

    public void Init(RecipeSO recipe)
    {
        _titleRecipe.text = recipe.Result.ItemConfig.Key;

        _iconResult.InitIcon(new UIIconModel(recipe.Result));

        _parentResources.DestroyChildrens();
        foreach (var r in recipe.Resources)
        {
            var uicp = Instantiate(_prefabResources, _parentResources);
            uicp.InitIcon(new UIIconModel(r));
        }
    }

    private void OnClick()
    {
        //TODO do recipe
    }

    private void OnDestroy()
    {
        _buttonCraft.onClick.RemoveAllListeners();
    }
}