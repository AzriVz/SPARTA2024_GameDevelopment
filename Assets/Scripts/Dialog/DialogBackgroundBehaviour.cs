using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class DialogBackgroundBehaviour : MonoBehaviour
{
  private static DialogBackgroundBehaviour Instance;

  public Image backgroundImageBack;
  public Image backgroundImageFront;
  private DialogueRunner dialogueRunner;

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
    backgroundImageBack.enabled = false;
    backgroundImageFront.enabled = false;
  }

  private void Start()
  {
    dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    if (dialogueRunner != null) dialogueRunner.onDialogueComplete.AddListener(OnDialogComplete);
    else Debug.LogError("DialogueRunner not found in the scene.");
  }

  public void OnDialogComplete()
  {
    SetBackground(null, 0.5f);
  }

  [YarnCommand("SetBackground")]
  public static void SetBackground(string spriteName = "", float fadeDuration = 0.5f)
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
        Debug.LogWarning($"Sprite '{spriteName}' not found in Resources. Using black background instead.");
        SetBackgroundBlack(fadeDuration);
        return;
      }
    }
        
    Instance.backgroundImageFront.color = Color.white;
    Instance.backgroundImageFront.sprite = newSprite;
    Instance.backgroundImageFront.enabled = true;
    Instance.StopAllCoroutines();
    Instance.StartCrossFade(fadeDuration, newSprite == null);
  }

  [YarnCommand("SetBackgroundBlack")]
  public static void SetBackgroundBlack(float fadeDuration = 0.5f)
  {
    if (Instance == null)
    {
      Debug.LogError("DialogBackgroundBehaviour instance not found.");
      return;
    }
    
    Instance.backgroundImageFront.color = Color.black;
    Instance.backgroundImageFront.sprite = null;
    Instance.backgroundImageFront.enabled = true;
    Instance.StopAllCoroutines();
    Instance.StartCrossFade(fadeDuration, false);
  }

  public void StartCrossFade(float duration, bool isFadeOut)
  {
    StartCoroutine(CrossFade(duration, isFadeOut));
  }

  private IEnumerator CrossFade(float duration, bool isFadeOut)
  {
    if (!isFadeOut)
    {
      float time = 0f;
      Color startColor = new(backgroundImageFront.color.r, backgroundImageFront.color.g, backgroundImageFront.color.b, 0);
      Color endColor = new(backgroundImageFront.color.r, backgroundImageFront.color.g, backgroundImageFront.color.b, 1);
      backgroundImageFront.color = startColor;
      while (time < duration)
      {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        backgroundImageFront.color = Color.Lerp(startColor, endColor, t);
        yield return null;
      }
      backgroundImageFront.color = endColor;
      backgroundImageBack.color = endColor;      
      backgroundImageBack.sprite = backgroundImageFront.sprite;
      backgroundImageBack.enabled = true;
    }
    else
    {
      float time = 0f;
      Color startColor = new(backgroundImageBack.color.r, backgroundImageBack.color.g, backgroundImageBack.color.b, backgroundImageBack.color.a);
      Color endColor = new(backgroundImageBack.color.r, backgroundImageBack.color.g, backgroundImageBack.color.b, 0);
      backgroundImageFront.color = endColor;
      backgroundImageBack.color = startColor;
      backgroundImageBack.enabled = true;
      while (time < duration)
      {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        backgroundImageBack.color = Color.Lerp(startColor, endColor, t);
        yield return null;
      }
      backgroundImageBack.color = endColor;
      backgroundImageBack.sprite = null;
      backgroundImageFront.sprite = null;
      backgroundImageBack.enabled = false;
      backgroundImageFront.enabled = false;
    }
  }
}
