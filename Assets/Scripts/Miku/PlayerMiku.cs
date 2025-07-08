using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMiku : MonoBehaviour
{
    public string SceneName;
    public bool IsInJudge;

    public TextMeshProUGUI ScoreText;
    public Slider ScoreSlider;

    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 5f;

    private float score;
    private float hp;
    private Vector3 scoreTextOriginalPos;

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
        score = 0;
        hp = maxHp;
        scoreTextOriginalPos = ScoreText.rectTransform.localPosition;
        UpdateUI();
    }

    private void HandleProgress(bool isMiss)
    {
        if (isMiss) score = 0;
        else score += 1;
        if (!IsInJudge || isMiss)
        {
            hp = Mathf.Max(0, hp - 2);
        }
        UpdateUI();
        StartCoroutine(ShakeScoreText());
    }

    private void UpdateUI()
    {
        ScoreText.text = Mathf.FloorToInt(score).ToString();
        ScoreSlider.value = hp / maxHp;
    }

    private IEnumerator ShakeScoreText()
    {
        float elapsed = 0f;
        var rt = ScoreText.rectTransform;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / shakeDuration;
            float damper = 1f - Mathf.Clamp01(percent);

            Vector2 offset = Random.insideUnitCircle * shakeMagnitude * damper;
            rt.localPosition = scoreTextOriginalPos + new Vector3(offset.x, offset.y, 0f);
            yield return null;
        }

        rt.localPosition = scoreTextOriginalPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JudgeZone")) IsInJudge = true;

        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("yay");
            HandleProgress(false);
            collision.GetComponent<SpriteRenderer>().color = Color.green;
            collision.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject, 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("JudgeZone")) IsInJudge = false;
    }

    public void Miss()
    {
        HandleProgress(true);
    }
}
