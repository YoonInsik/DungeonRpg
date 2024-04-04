using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_Chest : BaseProp, IInteractable
{
    [SerializeField] protected List<DropData> dropTable;

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public bool IsInteractable()
    {
        throw new System.NotImplementedException();
    }
}
