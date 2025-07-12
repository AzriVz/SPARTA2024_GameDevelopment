using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Markup;
using Yarn.Unity;

public class SpriteActionMarkupHandler : ActionMarkupHandler
{
  public Image spriteImage;
  public TMP_Text characterText;

  public override void OnPrepareForLine(MarkupParseResult line, TMP_Text text) { }

  public override void OnLineDisplayBegin(MarkupParseResult line, TMP_Text text)
  {
    string characterName = characterText.text;
    if (!string.IsNullOrEmpty(characterName))
    {
      string spriteName = $"Art/CharacterBig/{characterName}/{characterName}_Sprite_1";
      spriteImage.sprite = Resources.Load<Sprite>(spriteName);
      if (spriteImage.sprite != null)
      {
        spriteImage.enabled = true;
      }
      else
      {
        Debug.LogWarning($"Sprite '{spriteName}' not found in Resources.");
        spriteImage.enabled = false;
      }
    }
    else
    {
      spriteImage.enabled = false;
    }
  }

  public override YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line, CancellationToken cancellationToken)
  {
    return YarnTask.CompletedTask;
  }

  public override void OnLineDisplayComplete() { }

  public override void OnLineWillDismiss() { }
}