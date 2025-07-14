using System;
using UnityEngine;

public class PlatformMiku : MonoBehaviour
{
    private Transform ground;
    private float hitTime;
    private SpriteRenderer _spriteRenderer;
    private TileSprites _sprites;
    public bool isTouched = false;

    [Serializable]
    public struct TileSprites
    {
        public Sprite defaultSprite;
        public Sprite touchedSprite;
        public Sprite failedSprite;
    }
    
    void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Initialize(float hitTime, Transform ground, TileSprites tileSprites)
    {
        this.hitTime = hitTime;
        this.ground = ground;
        _spriteRenderer.sprite = tileSprites.defaultSprite;
        _sprites = tileSprites;
    }

    public void Touch()
    {
        isTouched = true;
        _spriteRenderer.sprite = _sprites.touchedSprite;
    }

    public void Fail()
    {
        _spriteRenderer.sprite = _sprites.failedSprite;
    }

    void Update()
    {
        float timeLeft = hitTime - Time.time;
        if (timeLeft <= 0f)
        {
            transform.position = new Vector3(transform.position.x, ground.position.y, transform.position.z);
            enabled = false;
            return;
        }

        float deltaY = ground.position.y - transform.position.y;
        float velocityY = deltaY / timeLeft;
        transform.position += Vector3.up * (velocityY * Time.deltaTime);
    }
}
