using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textQuest;
    [SerializeField] private TextMeshProUGUI tryText;

    void Start()
    {
        textQuest.text = $"Find All Nakano Sisters ({MasterGameManager.Instance.CountSister()} / 5)";
        tryText.text = $"You have {MasterGameManager.Instance.TryLeft} Tries Left";
    }
}
