using UnityEngine;

[CreateAssetMenu(menuName = "Group", order = 1)]
public class GroupSO : ScriptableObject
{
    public int Order;
    public string GroupName;
    public Sprite IconGroup;
}