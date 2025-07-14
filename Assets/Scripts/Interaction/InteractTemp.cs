using UnityEngine;

public class InteractTemp : InteractableObject
{
    public override void Interact()
    {
        //base.Interact();

        Debug.Log("Hey you're touching me!");
        //Implement interact mechanic here
    }
}