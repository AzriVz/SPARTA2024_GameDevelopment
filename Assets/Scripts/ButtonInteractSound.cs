using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
  [RequireComponent(typeof(Button))]
  public class ButtonInteractSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
  {
    private Button _button;
    private void Start()
    {
      _button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      AudioManager.instance.PlaySFX("HoverButton");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      AudioManager.instance.PlaySFX("ClickButton");
    }
  }
}