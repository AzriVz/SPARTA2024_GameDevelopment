using Mechanic.Itsuki;
using UnityEngine;

namespace Mechanic.Ichika
{
  public class IchikaPlayerManager : PlayerManager
  {
    [SerializeField] private MovingPlatform movingPlatform;
    [SerializeField] private float upwardsDistance;

    protected override void TeleportPlayerToSpawn()
    {
      UpdatePlayerSpawn();
      player.transform.position = playerSpawn;
    }
    protected override void InstantiatePlayer()
    {
      UpdatePlayerSpawn();
      player = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
    }

    private void UpdatePlayerSpawn()
    {
      playerSpawn = movingPlatform.transform.position + new Vector3(0f, upwardsDistance, 0f);
    }

    public override void OnDrawGizmosSelected()
    {
      
    }
  }
}