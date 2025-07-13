using Mechanic.Itsuki;
using UnityEngine;

public class Curtain : InteractableObject
{
    [SerializeField] Sprite hiddenIchika;
    [SerializeField] Sprite openedCurtain;
    [SerializeField] Sprite openedIchikaCurtain;
    [SerializeField] public bool ichikaCurtain;

    public CurtainRandomizer curtainRandomizer;

    public override void Interact()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerTransform != null)
        {
            if (!ichikaCurtain)
            {
                GetComponent<SpriteRenderer>().sprite = openedCurtain;
                curtainRandomizer.StartCoroutine(curtainRandomizer.GameSequence());
            } else
            {
                GetComponent<SpriteRenderer>().sprite = openedIchikaCurtain;
                StageManager.Instance.Win();
            }
        }
    }
}
