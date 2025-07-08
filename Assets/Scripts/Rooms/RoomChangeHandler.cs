using System.Collections;
using UnityEngine;

public class RoomChangeHandler2D : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float transitionDuration = 0.5f;

    void Awake()
    {
        if (playerCamera == null)
        {
            var camObj = GameObject.FindGameObjectWithTag("playerCamera");
            if (camObj != null)
                playerCamera = camObj.GetComponent<Camera>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerCamera == null || !other.CompareTag("Room"))
            return;

        Transform roomParent = other.transform.parent;
        if (roomParent == null)
            return;

        StopAllCoroutines();
        StartCoroutine(TransitionCamera(roomParent.position, roomParent.rotation));
    }

    IEnumerator TransitionCamera(Vector3 targetPos, Quaternion targetRot)
    {
        Transform camT = playerCamera.transform;
        Vector3 startPos = camT.position;
        float fixedZ = startPos.z;                                // preserve original Z
        Vector3 endPos2D = new Vector3(targetPos.x, targetPos.y, fixedZ);

        Quaternion startRot = camT.rotation;

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            camT.position = Vector3.Lerp(startPos, endPos2D, t);
            camT.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        camT.position = endPos2D;
        camT.rotation = targetRot;
    }
}
