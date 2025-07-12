using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class FollowerHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform mikuFollower;
    [SerializeField] private Transform ninoFollower;
    [SerializeField] private Transform ichikaFollower;
    [SerializeField] private Transform itsukiFollower;
    [SerializeField] private Transform yotsubaFollower;

    [SerializeField] private float followSpeed;
    [SerializeField] private float followDistance; 

    private readonly List<Transform> activeFollowers = new List<Transform>();

    void Awake()
    {
        mikuFollower.gameObject.SetActive(false);
        ninoFollower.gameObject.SetActive(false);
        ichikaFollower.gameObject.SetActive(false);
        itsukiFollower.gameObject.SetActive(false);
        yotsubaFollower.gameObject.SetActive(false);

        CheckWins();
    }

    void LateUpdate()
    {
        if (player == null) return;
        Vector3 target = player.position;
        foreach (var follower in activeFollowers)
        {
            float deltaX = target.x - follower.position.x;
            if (Mathf.Abs(deltaX) > followDistance)
            {
                var sr = follower.GetComponent<SpriteRenderer>();
                if(deltaX > 0) sr.flipX = true;
                else sr.flipX = false;
                follower.GetComponent<Animator>().SetBool("isRunning", true);
                float targetX = Mathf.MoveTowards(follower.position.x, target.x, followSpeed * Time.deltaTime);
                follower.position = new Vector3(targetX, player.position.y, follower.position.z);
            }
            var movement = player.gameObject.GetComponent<movement>();
            if (movement != null)
            {
                if (!movement.isMoving())
                {
                    follower.GetComponent<Animator>().SetBool("isRunning", false);
                }
            }
            target = follower.position;
        }
    }

    public void ResetPositions()
    {
        if (player == null) return;

        Vector3 playerPos = player.position;
        for (int i = 0; i < activeFollowers.Count; i++)
        {
            var follower = activeFollowers[i];
            follower.position = new Vector3(playerPos.x, playerPos.y, follower.position.z);
        }
    }

    public void CheckWins()
    {
        var gm = MasterGameManager.Instance;
        if (gm.MikuWin && !activeFollowers.Contains(mikuFollower)) ActivateFollower(mikuFollower);
        if (gm.NinoWin && !activeFollowers.Contains(ninoFollower)) ActivateFollower(ninoFollower);
        if (gm.IchikaWin && !activeFollowers.Contains(ichikaFollower)) ActivateFollower(ichikaFollower);
        if (gm.ItsukiWin && !activeFollowers.Contains(itsukiFollower)) ActivateFollower(itsukiFollower);
        if (gm.YotsubaWin && !activeFollowers.Contains(yotsubaFollower)) ActivateFollower(yotsubaFollower);
    }

    private void ActivateFollower(Transform follower)
    {
        follower.gameObject.SetActive(true);
        follower.position = new Vector3(player.position.x, player.position.y, follower.position.z);
        activeFollowers.Add(follower);
    }
}
