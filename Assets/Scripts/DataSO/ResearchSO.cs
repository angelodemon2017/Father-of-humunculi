using UnityEngine;

[CreateAssetMenu(menuName = "Research config", order = 1)]
public class ResearchSO : ScriptableObject
{
    public Sprite IconItem;
    public string Name;
    public int Need;
}