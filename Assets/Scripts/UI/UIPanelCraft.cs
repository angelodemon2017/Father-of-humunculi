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

    private RecipeSO _recipe;

    private void Awake()
    {
        _buttonCraft.onClick.AddListener(OnClick);
    }

    public void Init(RecipeSO recipe)
    {
        _recipe = recipe;
        _parentResources.DestroyChildrens();

        _titleRecipe.text = recipe.Result.ItemConfig.Key;

        _iconResult.InitIcon(new UIIconModel(recipe.Result));

        foreach (var r in recipe.Resources)
        {
            var uicp = Instantiate(_prefabResources, _parentResources);
            uicp.InitIcon(new UIIconModel(r, aspectMode: AspectRatioFitter.AspectMode.HeightControlsWidth));
        }
    }

    private void OnClick()
    {
        UIPlayerManager.Instance.UIPresentInventory.ApplyRecipe(_recipe);

        /*        EntityPlayer ep = GameProcess.Instance.GameWorld.entityDatas.LastOrDefault() as EntityPlayer;
                var ci = ep.Components.GetComponent<ComponentInventory>();
                ci.TryApplyRecipe(_recipe);
                ep.UpdateEntity();/**/
        //TODO do recipe
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _buttonCraft.onClick.RemoveAllListeners();
    }
}