using System;
using System.Collections;
using UnityEngine;

namespace Mechanic.Itsuki
{
  public class TsundereAttack : MonoBehaviour
  {
    private SpriteRenderer _sr;
    private bool _isLaunched = false;
    private float _velocity;
    private Vector3 _direction;
    private float _lifetime;
    private void Start()
    {
      _sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Sprite sprite, float lifetime)
    {
      if (_sr == null)
      {
        _sr = GetComponent<SpriteRenderer>();
      }
      _sr.sprite = sprite;
      _lifetime = lifetime;
      StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
      yield return new WaitForSeconds(_lifetime);
      Destroy(this.gameObject);
    } 

    public void Launch(Vector3 direction, float velocity)
    {
      _isLaunched = true;
      _velocity = velocity;
      _direction = direction;
    }

    private void FixedUpdate()
    {
      if(_isLaunched)
        transform.position += _direction * (_velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      var health = other.GetComponent<Health>();
      if (health == null) return;
      health.Damage();
    }
  }
}