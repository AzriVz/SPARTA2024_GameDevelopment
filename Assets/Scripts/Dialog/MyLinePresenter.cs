using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Markup;
using Yarn.Unity.Attributes;
using Yarn.Unity;
using UnityEngine.UI;

public class MyLinePresenter : DialoguePresenterBase
{
  [Space]
  [MustNotBeNull(null)]
  public CanvasGroup? canvasGroup;

  [MustNotBeNull(null)]
  public TMP_Text? lineText;

  [Group("Character", false)]
  [Label("Shows Name")]
  public bool showCharacterNameInLineView = true;

  [Group("Character", false)]
  [ShowIf("showCharacterNameInLineView")]
  [Label("Name field")]
  [MustNotBeNullWhen("showCharacterNameInLineView", "A text field must be provided when Shows Name is set")]
  public TMP_Text? characterNameText;

  [Group("Character", false)]
  [ShowIf("showCharacterNameInLineView")]
  public GameObject? characterNameContainer = null;

  [Group("Fade", false)]
  [Label("Fade UI")]
  public bool useFadeEffect = true;

  [Group("Fade", false)]
  [ShowIf("useFadeEffect")]
  public float fadeUpDuration = 0.25f;

  [Group("Fade", false)]
  [ShowIf("useFadeEffect")]
  public float fadeDownDuration = 0.1f;

  [Group("Automatically Advance Dialogue", false)]
  public bool autoAdvance = false;

  [Group("Automatically Advance Dialogue", false)]
  [ShowIf("autoAdvance")]
  [Label("Delay before advancing")]
  public float autoAdvanceDelay = 1f;

  [Group("Typewriter", false)]
  public bool useTypewriterEffect = true;

  [Group("Typewriter", false)]
  [ShowIf("useTypewriterEffect")]
  [Label("Letters per second")]
  [Min(0f)]
  public int typewriterEffectSpeed = 60;

  [Group("Typewriter", false)]
  [ShowIf("useTypewriterEffect")]
  [Label("Event Handler")]
  [FormerlySerializedAs("actionMarkupHandlers")]
  [SerializeField]
  private List<ActionMarkupHandler> eventHandlers = new List<ActionMarkupHandler>();

  public Image spriteImage;

  public override YarnTask OnDialogueCompleteAsync()
  {
    if (canvasGroup != null)
    {
      canvasGroup.alpha = 0f;
    }
    if (spriteImage != null)
    {
      spriteImage.enabled = false;
    }

    return YarnTask.CompletedTask;
  }

  public override YarnTask OnDialogueStartedAsync()
  {
    if (canvasGroup != null)
    {
      canvasGroup.alpha = 0f;
    }

    return YarnTask.CompletedTask;
  }

  private void Awake()
  {
    if (useTypewriterEffect)
    {
      PauseEventProcessor item = new PauseEventProcessor();
      ActionMarkupHandlers.Insert(0, item);
    }

    if (characterNameContainer == null && characterNameText != null)
    {
      characterNameContainer = characterNameText.gameObject;
    }
  }

  private void Start()
  {
    ActionMarkupHandlers.AddRange(eventHandlers);
  }

  public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
  {
    if (lineText == null)
    {
      Debug.LogError("Line view does not have a text view. Skipping line " + line.TextID + " (\"" + line.RawText + "\")");
      return;
    }

    spriteImage.enabled = false;
    foreach (string s in line.Metadata)
    {
      if (s.StartsWith("sprite:"))
      {
        string spriteName = s[7..];
        spriteImage.sprite = Resources.Load<Sprite>($"Art/CharacterBig/{spriteName}");
        if (spriteImage.sprite != null)
        {
          spriteImage.enabled = true;
        }
        else
        {
          Debug.LogWarning($"Sprite '{spriteName}' not found in Resources.");
        }
      }
    }

    MarkupParseResult text;
    if (showCharacterNameInLineView)
    {
      if (characterNameText == null)
      {
        Debug.LogWarning("Line view is configured to show character names, but no character name text view was provided.", this);
      }
      else
      {
        characterNameText.text = line.CharacterName;
      }

      text = line.TextWithoutCharacterName;
      if (line.Text.TryGetAttributeWithName("character", out var characterAttribute))
      {
        text.Attributes.Add(characterAttribute);
      }
    }
    else
    {
      if (characterNameContainer != null)
      {
        characterNameContainer.SetActive(value: false);
      }

      text = line.TextWithoutCharacterName;
    }

    lineText.text = text.Text;
    if (useTypewriterEffect)
    {
      lineText.maxVisibleCharacters = 0;
      foreach (IActionMarkupHandler processor in ActionMarkupHandlers)
      {
        processor.OnPrepareForLine(text, lineText);
      }
    }
    else
    {
      lineText.maxVisibleCharacters = text.Text.Length;
    }

    if (canvasGroup != null)
    {
      if (useFadeEffect)
      {
        await Effects.FadeAlphaAsync(canvasGroup, 0f, 1f, fadeDownDuration, token.HurryUpToken);
      }
      else
      {
        canvasGroup.alpha = 1f;
      }
    }

    if (useTypewriterEffect)
    {
      BasicTypewriter typewriter = new BasicTypewriter
      {
        ActionMarkupHandlers = ActionMarkupHandlers,
        Text = lineText,
        CharactersPerSecond = typewriterEffectSpeed
      };
      await typewriter.RunTypewriter(text, token.HurryUpToken);
    }

    if (!autoAdvance)
    {
      await YarnTask.WaitUntilCanceled(token.NextLineToken).SuppressCancellationThrow();
    }
    else
    {
      await YarnTask.Delay((int)(autoAdvanceDelay * 1000f), token.NextLineToken).SuppressCancellationThrow();
    }

    foreach (IActionMarkupHandler processor2 in ActionMarkupHandlers)
    {
      processor2.OnLineWillDismiss();
    }

    if (canvasGroup != null)
    {
      if (useFadeEffect)
      {
        await Effects.FadeAlphaAsync(canvasGroup, 1f, 0f, fadeDownDuration, token.HurryUpToken).SuppressCancellationThrow();
      }
      else
      {
        canvasGroup.alpha = 0f;
      }
    }
  }

  public override YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
  {
    return YarnTask<DialogueOption>.FromResult(null);
  }
}