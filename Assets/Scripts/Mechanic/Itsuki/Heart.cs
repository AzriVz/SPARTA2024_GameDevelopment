using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Mechanic.Itsuki
{
  public class Heart : MonoBehaviour
  {
    [SerializeField] private Sprite normalHeart;
    [SerializeField] private Sprite brokenHeart;
    private Image _heartImage;

    public void OnEnable()
    {
      _heartImage = GetComponent<Image>();
    }

    public enum HeartType
    {
      Normal,
      Broken,
    }
    public void Initialize(HeartType heartType)
    {
      if(!_heartImage)
        _heartImage = GetComponent<Image>();
      switch (heartType)
      {
        case HeartType.Normal: { _heartImage.sprite = normalHeart;
          break;
        }
        case HeartType.Broken: { _heartImage.sprite = brokenHeart;
          break;
        }
      }
    }
  }
}