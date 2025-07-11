using UnityEngine;
using Yarn;

public class NPC : InteractableObject
{
    [SerializeField] bool miku;
    [SerializeField] bool nino;
    [SerializeField] bool itsuki;
    [SerializeField] bool ichika;
    [SerializeField] bool yotsuba;
    [SerializeField] LevelManager.SceneID sceneID;

    public Dialogue dialog;
    public bool isInteractable;

    private void Start()
    {
        var gm = MasterGameManager.Instance;
        if (miku) isInteractable = !gm.MikuWin;
        else if (nino) isInteractable = !gm.NinoWin;
        else if (itsuki) isInteractable = !gm.ItsukiWin;
        else if (ichika) isInteractable = !gm.IchikaWin;
        else if (yotsuba) isInteractable = !gm.YotsubaWin;

        if(!isInteractable) gameObject.SetActive(false);
    }

    public override void Interact()
    {
        if(!isInteractable) return;

        // Run dialog here

        LevelManager.Instance.ChangeLevel(LevelManager.SceneID.MapRoom, sceneID, false);
    }
}