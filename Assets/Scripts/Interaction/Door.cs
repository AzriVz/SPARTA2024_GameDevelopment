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
            {
                Debug.Log("open sasame");
                playerTransform.position = otherDoor.transform.position;
            }
            else Debug.Log("Door is Locked");
        }
    }
    private void OnDrawGizmos()
    {
        if (otherDoor == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, otherDoor.transform.position);
    }
}