using System.Collections;
using Mechanic.Itsuki;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMiku : MonoBehaviour
{
    public string SceneName;
    public bool IsInJudge;

    [SerializeField] private float maxHp = 100f;

    private MikuScoreManager _mikuScoreManager;
    private Health _playerHealth;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != SceneName)
        {
            enabled = false;
            return;
        }
    }

    void Start()
    {
        _mikuScoreManager = MikuScoreManager.Instance;
        _playerHealth = PlayerManager.Instance.player.GetComponent<Health>();
        UpdateUI();
        // StartCoroutine(wintest());
    }

    // private void HandleProgress(bool isMiss)
    // {
    //     if (isMiss) score = 0;
    //     else score += 1;
    //     if (!IsInJudge || isMiss)
    //     {
    //         hp = Mathf.Max(0, hp - 2);
    //     }
    //     UpdateUI();
    // }

    private void UpdateUI()
    {
        // ScoreText.text = Mathf.FloorToInt(score).ToString();
        // ScoreSlider.value = hp / maxHp;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JudgeZone")) IsInJudge = true;

        if (collision.gameObject.CompareTag("Platform"))
        {
            _mikuScoreManager.AddScore(1);
            collision.GetComponent<SpriteRenderer>().color = Color.green;
            collision.gameObject.tag = "Untagged";
            collision.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject, 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("JudgeZone")) IsInJudge = false;
    }

}
