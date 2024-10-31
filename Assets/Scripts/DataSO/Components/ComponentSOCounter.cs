using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Counter Component", order = 1)]
public class ComponentSOCounter : ComponentSO, ISeconder
{
    [SerializeField] int _chanceToAction;

    public void DoSecond()
    {
        if (_chanceToAction.GetChance())
        {
            //??
        }
    }
}