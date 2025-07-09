using UnityEngine;
using TMPro;

public class PlayerInteract2D : MonoBehaviour
{
    [SerializeField] Transform playerPivot;
    [SerializeField] TextMeshProUGUI promptText;
    public static bool ShouldShowPrompt = true;

    IInteractable currentTarget;
    bool ready = true;
    void Start()
    {
        if (playerPivot == null || promptText == null)
            ready = false;
    }

    public void Initialize(TextMeshProUGUI promptText)
    {
        this.promptText = promptText;
        Start();
    }

    void Update()
    {
        if (!ready) return;
        CheckForInteractable2D();
        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
            currentTarget.Interact();
    }

    void CheckForInteractable2D()
    {
        Vector2 origin = playerPivot.position;
        float radius = 2f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius);

        Collider2D closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Interactable")) continue;

            float dist = Vector2.Distance(origin, hit.transform.position);
            if (dist < minDist)
            {
                closest = hit;
                minDist = dist;
            }
        }

        if (closest != null && ShouldShowPrompt)
        {
            currentTarget = closest.GetComponent<IInteractable>();
            promptText.transform.parent.gameObject.SetActive(true);
            promptText.text = currentTarget.GetPrompt();
        }
        else
        {
            currentTarget = null;
            promptText.transform.parent.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (playerPivot != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerPivot.position, 2f);
        }
    }
}
