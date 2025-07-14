using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Mechanic.Itsuki
{
  public class ItsukiMouth : InteractableObject
  {
    [SerializeField] private PlayerFoodGrabber playerFoodGrabber;
    [SerializeField] public int nFoodToWin;
    [FormerlySerializedAs("textObject")] [SerializeField] private TextMeshProUGUI scoreTextObject, textObject;
    [SerializeField] private SpriteRenderer itsukiSprite;
    [SerializeField] private Image foodImage;
    [SerializeField] private List<Sprite> chewSprites;
    [SerializeField] private Sprite openMouthSprite;
    [SerializeField] private Sprite closeMouthSprite;
    [SerializeField] private float totalChewTime;
    [SerializeField] private bool canConsume = true;
    [SerializeField] private FoodSpawnManager foodSpawnManager;
    private Food _currentFood;
    private int _foodCount = 0;
    private StageManager _stageManager;
    private List<Food> _foodList;
    
    private void Start()
    {
      _stageManager = StageManager.Instance;
      PlayerManager.Instance.OnSpawn += SetFoodGrabber;
      SetText(_foodCount);
      _foodList = foodSpawnManager.foods;
      RandomizeFood();
    }

    private void SetText(int foodCount)
    {
      scoreTextObject.text = foodCount+"/"+nFoodToWin;
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

    private void RandomizeFood()
    {
      _currentFood = _foodList[UnityEngine.Random.Range(0, _foodList.Count)];
      foodImage.sprite = _currentFood.sprite;
    }

    private IEnumerator SetText(float seconds, string text)
    {
      textObject.enabled = true;
      textObject.text = text;
      yield return new WaitForSeconds(seconds);
      textObject.enabled = false;
    }
    private void WrongFood()
    {
      StartCoroutine(SetText(2, "Wrong Food :("));
      GameObject player = PlayerManager.Instance.player;
      player.GetComponent<Health>().Damage();
      playerFoodGrabber.LetGo();
    }
    public override void Interact()
    {
      if (!playerFoodGrabber) return;
      if (!playerFoodGrabber.FoodInventory.FoodExists()) return;
      if (!canConsume) return;
      Debug.Log(_currentFood.foodName + " " + playerFoodGrabber.FoodInventory.currentFood.Food.foodName);
      if (_currentFood != playerFoodGrabber.FoodInventory.currentFood.Food)
      {
        WrongFood();
        return;
      }
      LockMouth(true);
      playerFoodGrabber.LetGo();
      SetText(_foodCount);
      _foodCount++;
      RandomizeFood();

      AudioManager.instance.PlaySFX("Eating");
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