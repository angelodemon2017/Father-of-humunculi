using UnityEngine;

[CreateAssetMenu(menuName = "States/Craft Recipe State", order = 1)]
public class CraftRecipeState : State
{
    [Range(0.01f, 10)]
    [SerializeField] private float _craftSpeed = 1f;
    [SerializeField] private IdleState _stateAfterDone;

    private int _idRecipe;
    private float _timeAnimate;
    private Vector3 _target;
    private RecipeSO _recipeSO;

    public override string DebugField => $"crafting:{_timeAnimate.ToString("#.#")}";

    protected override void Init()
    {
        var fSMController = Character.GetEntityMonobeh().GetMyComponent<FSMController>();

        _idRecipe = fSMController.IdRecipe;
        fSMController.IdRecipe = -1;
        _recipeSO = RecipesController.GetRecipe(_idRecipe);
        _target = fSMController.TargetRec;
        _timeAnimate = _recipeSO.SecondUsing;

        IsFinished = true;
    }

    protected override void Run()
    {
        _timeAnimate -= Time.deltaTime * _craftSpeed;
        //TODO animate
        if (_timeAnimate <= 0f)
        {
            _recipeSO.EntityDoRecipe(_target);
            Character.SetState(_stateAfterDone);
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return IsReadyCraft(character);
    }

    private bool IsReadyCraft(IStatesCharacter character)
    {
        var fsm = character.GetEntityMonobeh().GetMyComponent<FSMController>();

        return fsm.IdRecipe >= 0;
    }
}