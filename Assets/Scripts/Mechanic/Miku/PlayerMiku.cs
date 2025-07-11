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
            Debug.Log("yay");
            // HandleProgress(false);
            _mikuScoreManager.AddScore(1);
            collision.GetComponent<SpriteRenderer>().color = Color.green;
            collision.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject, 3f);
        }
    }

    IEnumerator wintest()
    {
        yield return new WaitForSeconds(3f);
        LevelManager.Instance.ChangeLevel(LevelManager.SceneID.LevelMiku, LevelManager.SceneID.MapRoom, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("JudgeZone")) IsInJudge = false;
    }

}
