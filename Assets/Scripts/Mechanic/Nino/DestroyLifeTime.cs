using System;
using UnityEngine;

namespace Mechanic.Nino
{
  public class DestroyLifeTime : MonoBehaviour
  {
    [SerializeField] private float timeToDestroy;
    private void Start()
    {
      Destroy(gameObject, timeToDestroy);
    }
  }
}