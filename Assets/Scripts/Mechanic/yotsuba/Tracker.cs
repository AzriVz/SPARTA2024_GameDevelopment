using Mechanic.Itsuki;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tracker : MonoBehaviour
{
    public Transform playerA;
    public string yotsubaTag = "Yotsuba";
    public Slider distanceSlider;

    public float maxSpawnDelay = 20f; // waktu maksimum sebelum Yotsuba wajib spawn
    public float maxDistance = 30f;

    private float spawnTimer = 0f;
    private Transform yotsuba;
    private bool trackingDistance = false;
    private bool gameActive = true;

    void Start()
    {
        playerA = PlayerManager.Instance.player.transform;
        distanceSlider.value = 0f;
        distanceSlider.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!gameActive) return;

        // FASE 1: COUNTDOWN SAMPAI YOTSUBA MUNCUL
        if (!trackingDistance)
        {
            spawnTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(spawnTimer / maxSpawnDelay);
            distanceSlider.value = progress;

            // Cek apakah Yotsuba sudah spawn
            if (yotsuba == null)
            {
                GameObject yotsubaObj = GameObject.FindWithTag(yotsubaTag);
                if (yotsubaObj != null)
                {
                    yotsuba = yotsubaObj.transform;
                    trackingDistance = true; // ganti ke fase jarak
                }
            }
        }
        else
        {
            // FASE 2: TRACKING JARAK KE YOTSUBA
            if (yotsuba == null || !yotsuba.gameObject.activeInHierarchy)
            {
                trackingDistance = false; // fallback ke fase waktu lagi
                return;
            }

            if (!playerA.IsDestroyed())
            {
                float distance = Vector3.Distance(playerA.position, yotsuba.position);
                float normalized = Mathf.Clamp01(1 - (distance / maxDistance));
                distanceSlider.value = normalized;
            }
        }
    }

    public void StopTracking()
    {
        gameActive = false;
        distanceSlider.gameObject.SetActive(false);
    }

    public void PlayerCaughtYotsuba()
    {
        gameActive = false;
        distanceSlider.value = 1f;
    }
}
