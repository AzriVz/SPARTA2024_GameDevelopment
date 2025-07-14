using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public virtual string DefaultPromptMessage => LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0] ? "(E) Interact" : "(E) Interaksi";
    public string promptMessage;
    public string GetPrompt()
    {        
        return promptMessage.Length != 0 ? promptMessage : DefaultPromptMessage;
    }

    public virtual void IsOverEnter()
    {
        
    }
    
    public virtual void IsOverExit()
    {
        
    }

    public virtual void Interact()
    {

    }
}
