using UnityEngine;
using Yarn;

public class NPC : InteractableObject
{
    [SerializeField] bool miku;
    [SerializeField] bool nino;
    [SerializeField] bool itsuki;
    [SerializeField] bool ichika;
    [SerializeField] bool yotsuba;

    public Dialogue dialog;
    private bool isInteractable;

    private void Awake()
    {
        var gm = MasterGameManager.Instance;
        if (miku) isInteractable = !gm.MikuWin;
        else if (nino) isInteractable = !gm.NinoWin;
        else if (itsuki) isInteractable = !gm.ItsukiWin;
        else if (ichika) isInteractable = !gm.IchikaWin;
        else if (yotsuba) isInteractable = !gm.YotsubaWin;
    }

    public override void Interact()
    {
        if(!isInteractable) return;

        //play dialog
    }
}