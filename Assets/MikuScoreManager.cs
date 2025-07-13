using System;
using System.Collections;
using Mechanic.Itsuki;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MikuScoreManager : MonoBehaviour
{
  #region Singleton
  public static MikuScoreManager Instance;
  private void Awake() 
  { 
    if (Instance != null && Instance != this) 
    { 
      Destroy(this); 
    } 
    else 
    { 
      Instance = this; 
    } 
  }
  #endregion
  
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private int currentScore;
  [SerializeField] private float shakeDuration = 0.3f;
  [SerializeField] private float shakeMagnitude = 5f;
  private Vector3 _scoreTextOriginalPos;
  private Health _playerHealth;

  public void Start()
  {
    _scoreTextOriginalPos = scoreText.rectTransform.localPosition;
    PlayerManager.Instance.OnSpawn += GetHealth;
  }

  private void GetHealth()
  {
    _playerHealth = PlayerManager.Instance.player.GetComponent<Health>();
  }

  public void AddScore(int score)
  {
    currentScore += score;
    UpdateScoreText();
    ShakeScoreText();
  }

  public void ResetScore()
  {
    currentScore = 0;
    UpdateScoreText();
    ShakeScoreText();
  }

  public void UpdateScoreText()
  {
    scoreText.text = currentScore.ToString();
  }

  private void ShakeScoreText()
  {
    StartCoroutine(ShakeScoreTextCoroutine());
  }
  
  private IEnumerator ShakeScoreTextCoroutine()
  {
      float elapsed = 0f;
      var rt = scoreText.rectTransform;

      while (elapsed < shakeDuration)
      {
          elapsed += Time.deltaTime;
          float percent = elapsed / shakeDuration;
          float damper = 1f - Mathf.Clamp01(percent);

          Vector2 offset = Random.insideUnitCircle * shakeMagnitude * damper;
          rt.localPosition = _scoreTextOriginalPos + new Vector3(offset.x, offset.y, 0f);
          yield return null;
      }

      rt.localPosition = _scoreTextOriginalPos;
  }
    public void Miss()
    {
        // HandleProgress(true);
        ResetScore();
        _playerHealth.Damage();
    }
}
