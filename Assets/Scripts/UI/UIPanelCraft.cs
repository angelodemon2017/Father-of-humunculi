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
        gameObject.SetActive(false);
    }

    public void Init(RecipeSO recipe)
    {
        Debug.Log($"Init panel craft{recipe.Result.ItemConfig.Key}");

        _recipe = recipe;
        _parentResources.DestroyChildrens();

        _titleRecipe.text = recipe.Result.ItemConfig.Key;

        _iconResult.InitIcon(new UIIconModel(recipe.Result));

        foreach (var r in recipe.Resources)
        {
            var uicp = Instantiate(_prefabResources, _parentResources);
            uicp.InitIcon(new UIIconModel(r, AspectRatioFitter.AspectMode.HeightControlsWidth));
        }
    }

    private void OnClick()
    {
        EntityPlayer ep = GameProcess.Instance.GameWorld.entityDatas[0] as EntityPlayer;
        var ci = ep.Components.GetComponent<ComponentInventory>();
        ci.TryApplyRecipe(_recipe);
        ep.UpdateEntity();
        //TODO do recipe
    }

    private void OnDestroy()
    {
        _buttonCraft.onClick.RemoveAllListeners();
    }
}