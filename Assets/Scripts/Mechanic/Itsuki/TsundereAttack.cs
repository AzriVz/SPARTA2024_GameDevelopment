using System;
using UnityEngine;

namespace Mechanic.Itsuki
{
  public class TsundereAttack : MonoBehaviour
  {
    private SpriteRenderer _sr;
    private bool _isLaunched = false;
    private float _velocity;
    private Vector3 _direction;
    private void Start()
    {
      _sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Sprite sprite)
    {
      if (_sr == null)
      {
        _sr = GetComponent<SpriteRenderer>();
      }
      _sr.sprite = sprite;
    }

    public void Launch(Vector3 direction, float velocity)
    {
      _isLaunched = true;
      _velocity = velocity;
      _direction = direction;
    }

    private void FixedUpdate()
    {
      transform.position += _direction * (_velocity * Time.fixedDeltaTime);
    }
  }
}