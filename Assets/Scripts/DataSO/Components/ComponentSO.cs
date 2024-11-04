using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/BaseComponent", order = 1)]
public class ComponentSO : ScriptableObject
{
    internal virtual ComponentData GetComponentData { get; }

    public virtual void InitOnScene(EntityMonobeh entityMonobeh)
    {

    }
}