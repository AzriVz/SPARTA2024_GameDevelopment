using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Markup;
using Yarn.Unity;

public class DialogSpriteBehaviour : MonoBehaviour
{
  private DialogueRunner _dialogRunner;
  private LinePresenter _dialogPresenter;
  private Image _spriteImage;

  private void Start()
  {
    _dialogRunner = FindFirstObjectByType<DialogueRunner>();
    _spriteImage = GetComponent<Image>();
    if (_dialogRunner == null)
    {
      Debug.LogError("DialogueRunner not found in the scene. Please ensure it is present.");
      return;
    }
    _dialogPresenter = _dialogRunner.GetComponentInChildren<LinePresenter>();    

    _spriteImage.enabled = false;
    SpriteActionMarkupHandler actionMarkupHandler = new(_spriteImage, _dialogPresenter.characterNameText);
    _dialogPresenter.ActionMarkupHandlers.Add(actionMarkupHandler);
  }
}

public class SpriteActionMarkupHandler : ActionMarkupHandler
{
  private readonly Image _spriteImage;
  private readonly TMP_Text _characterText;
  public SpriteActionMarkupHandler(Image spriteImage, TMP_Text characterText)
  {
    _characterText = characterText;
    _spriteImage = spriteImage;
  }

  public override void OnPrepareForLine(MarkupParseResult line, TMP_Text text) { }

  public override void OnLineDisplayBegin(MarkupParseResult line, TMP_Text text)
  {
    // string dialogLine = line.Text;
    // Debug.Log($"Dialog started: {dialogLine}");
    // string spriteName = GetSpriteNameFromDialog(dialogLine);    
    string characterName = _characterText.text;
    if (!string.IsNullOrEmpty(characterName))
    {
      string spriteName = $"Art/CharacterBig/{characterName}/{characterName}_Sprite_1";      
      _spriteImage.sprite = Resources.Load<Sprite>(spriteName);
      if (_spriteImage.sprite != null)
      {
        _spriteImage.enabled = true;
      }
      else
      {
        Debug.LogWarning($"Sprite '{spriteName}' not found in Resources.");
        _spriteImage.enabled = false;
      }
    }
    else
    {
      _spriteImage.enabled = false;
    }
  }

  public override YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line, CancellationToken cancellationToken)
  {
    return YarnTask.CompletedTask;
  }

  public override void OnLineDisplayComplete() { }

  public override void OnLineWillDismiss() { }

  private static readonly string[] chaeracterNames = { "Ichika", "Itsuki", "Miku", "Nino", "Yotsuba" };
  private string GetSpriteNameFromDialog(string dialogLine)
  {
    string speakerName = dialogLine.Split(':')[0].Trim();
    if (System.Array.Exists(chaeracterNames, name => name.Equals(speakerName, System.StringComparison.OrdinalIgnoreCase)))
    {
      return $"Art/CharacterBig/{speakerName}_Sprite_1";
    }
    return null;
  }
}