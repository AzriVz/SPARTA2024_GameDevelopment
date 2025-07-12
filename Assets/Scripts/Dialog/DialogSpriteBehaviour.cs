using UnityEngine;
using UnityEngine.UI;
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
    SpriteActionMarkupHandler actionMarkupHandler = GetComponent<SpriteActionMarkupHandler>();
    actionMarkupHandler.spriteImage = _spriteImage;
    actionMarkupHandler.characterText = _dialogPresenter.characterNameText;
    _dialogPresenter.ActionMarkupHandlers.Add(actionMarkupHandler);
  }
}