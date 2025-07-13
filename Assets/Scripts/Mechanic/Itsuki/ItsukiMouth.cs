using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Itsuki
{
  public class ItsukiMouth : InteractableObject
  {
    [SerializeField] private PlayerFoodGrabber playerFoodGrabber;
    [SerializeField] public int nFoodToWin;
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private SpriteRenderer itsukiSprite;
    [SerializeField] private List<Sprite> chewSprites;
    [SerializeField] private Sprite openMouthSprite;
    [SerializeField] private Sprite closeMouthSprite;
    [SerializeField] private float totalChewTime;
    [SerializeField] private bool canConsume = true;
    private int _foodCount = 0;
    private StageManager _stageManager;
    
    private void Start()
    {
      _stageManager = StageManager.Instance;
      PlayerManager.Instance.OnSpawn += SetFoodGrabber;
      SetText(_foodCount);
    }

    private void SetText(int foodCount)
    {
      textObject.text = foodCount+"/"+nFoodToWin;
    }

    private void ChangeSprite(Sprite sprite)
    {
      itsukiSprite.sprite = sprite;
    }
    public override void IsOverEnter()
    {
      if(canConsume)
        ChangeSprite(openMouthSprite);
    }

    public override void IsOverExit()
    {
      if(canConsume)
        ChangeSprite(closeMouthSprite);
    }

    private void LockMouth(bool locked)
    {
      canConsume = !locked;
      if (locked) promptMessage = "Eating";
      else promptMessage = "(E) Feed";
    }
    private void SetFoodGrabber()
    {
      playerFoodGrabber = PlayerManager.Instance.player.GetComponent<PlayerFoodGrabber>();
    }
    
    public override void Interact()
    {
      if (!playerFoodGrabber) return;
      if (!playerFoodGrabber.FoodInventory.FoodExists()) return;
      if (!canConsume) return;
      LockMouth(true);
      playerFoodGrabber.LetGo();
      SetText(_foodCount);
      _foodCount++;

      StartCoroutine(ChewCoroutine());
    }

    private IEnumerator ChewCoroutine()
    {
      float startChewTime = Time.time;
      int i = 0;
      int max = chewSprites.Count;
      while (true)
      {
        ChangeSprite(chewSprites[i]);
        float delay = totalChewTime/chewSprites.Count;
        yield return new WaitForSeconds(delay);
        if (i == max-1) i = 0;
        else i++;
        float deltaChewTime = Time.time-startChewTime;
        if (deltaChewTime >= totalChewTime) break;
      }
      ChangeSprite(closeMouthSprite);
      LockMouth(false);
      if (_foodCount >= nFoodToWin)
      {
        _stageManager.Win();
      }
    }
  }
}