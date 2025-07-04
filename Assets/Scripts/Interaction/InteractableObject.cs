using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public virtual string DefaultPromptMessage => "(E) Interact";
    public string promptMessage;
    public string GetPrompt()
    {        
        return promptMessage.Length != 0 ? promptMessage : DefaultPromptMessage;
    }

    public virtual void Interact()
    {

    }
}
