using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInteractabler : PrefabByComponentData
{
    private List<KeyHinter> _otherEnts = new();

    public void AddKeyHinter(KeyHinter keyHinter)
    {
        if (!_otherEnts.Contains(keyHinter))
        {
            _otherEnts.Add(keyHinter);
        }
    }
}