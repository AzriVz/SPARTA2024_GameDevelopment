using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class DialogBackgroundBehaviour : MonoBehaviour
{
  private static DialogBackgroundBehaviour Instance;

  public Image backgroundImageBack;
  public Image backgroundImageFront;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    DialogueRunner dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    if (dialogueRunner != null) dialogueRunner.onDialogueComplete.AddListener(OnDialogComplete);
    else Debug.LogError("DialogueRunner not found in the scene.");
  }

  public void OnDialogComplete()
  {
    SetBackground(null, 0.5f);
  }

  [YarnCommand("SetBackground")]
  public static void SetBackground(string spriteName, float fadeDuration = 0.5f)
  {
    if (Instance == null)
    {
      Debug.LogError("DialogBackgroundBehaviour instance not found.");
      return;
    }

    Sprite newSprite = null;
    if (spriteName != null && spriteName != "")
    {
      newSprite = Resources.Load<Sprite>(spriteName);
      if (newSprite == null)
      {
        Debug.LogError($"Sprite '{spriteName}' not found in Resources.");
        return;
      }
    }

    Instance.backgroundImageFront.sprite = newSprite;
    Instance.StopAllCoroutines();
    Instance.StartCrossFade(fadeDuration);
  }

  public void StartCrossFade(float duration)
  {
    StartCoroutine(CrossFade(duration));
  }

  private IEnumerator CrossFade(float duration)
  {
    if (backgroundImageFront.sprite != null)
    {
      float time = 0f;
      Color startColor = new(1, 1, 1, 0);
      Color endColor = Color.white;
      backgroundImageFront.color = startColor;
      while (time < duration)
      {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        backgroundImageFront.color = Color.Lerp(startColor, endColor, t);
        yield return null;
      }
      backgroundImageFront.color = endColor;
      backgroundImageBack.sprite = backgroundImageFront.sprite;
      backgroundImageBack.color = Color.white;
    }
    else
    {
      float time = 0f;
      Color startColor = Color.white;
      Color endColor = new(1, 1, 1, 0);
      backgroundImageFront.color = endColor;
      backgroundImageBack.color = startColor;
      while (time < duration)
      {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        backgroundImageBack.color = Color.Lerp(startColor, endColor, t);
        yield return null;
      }
      backgroundImageBack.color = endColor;
      backgroundImageBack.sprite = null;
    }
  }
}
