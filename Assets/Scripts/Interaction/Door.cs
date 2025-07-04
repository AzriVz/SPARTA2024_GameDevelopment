using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] bool isLocked;
    [SerializeField] GameObject otherDoor;

    public override void Interact()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerTransform != null)
        {
            if (!isLocked)
                playerTransform.position = otherDoor.transform.position;
            else Debug.Log("Door is Locked");
        }
    }
}