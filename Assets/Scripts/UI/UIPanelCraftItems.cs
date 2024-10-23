using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelCraftItems : MonoBehaviour
{
    [SerializeField] private UIPanelCraft _uIPanelCraftPrefab;
    [SerializeField] private PanelHider _panelHider;
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentIconRecipes;
    [SerializeField] private Transform _parentCraftPanel;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private List<RecipeSO> recipes = new();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(string groupName)
    {
        _parentCraftPanel.DestroyChildrens();
        _parentIconRecipes.DestroyChildrens();
        recipes.Clear();

        var tempRecipes = RecipesController.GetRecipes(groupName);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentIconRecipes);

            uicp.InitIcon(new UIIconModel(tempRecipes[r].Result, r));
            uicp.OnPointerEnter += SelectRecipe;

            recipes.Add(tempRecipes[r]);
        }

        StartCoroutine(Crunch());
    }

    private IEnumerator Crunch()
    {
        yield return new WaitForSeconds(0.1f);
        _parentIconRecipes.gameObject.SetActive(false);
        _parentIconRecipes.gameObject.SetActive(true);
    }

    private void SelectRecipe(int index)
    {
        _parentCraftPanel.DestroyChildrens();
        var uipc = Instantiate(_uIPanelCraftPrefab, _parentCraftPanel);

        uipc.Init(recipes[index]);
    }

    private void OnDisable()
    {
        _parentCraftPanel.DestroyChildrens();
    }
}