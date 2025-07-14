using UnityEngine;

public class yotsuba : MonoBehaviour
{
    public float forwardSpeed = 3f;      // Kecepatan awal ke kanan
    public float backwardSpeed = 5f;     // Kecepatan ke kiri setelah 20 detik
    public float runDuration = 20f;      // Waktu berjalan ke kanan

    private float timer = 0f;
    private bool switchedToBackward = false;

    private void Start()
    {
        AudioManager.instance.PlayMusic("Yotsuba");
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < runDuration)
        {
            // Fase 1: Bergerak ke kanan (ke depan)
            transform.position += Vector3.right * forwardSpeed * Time.deltaTime;
        }
        else
        {
            // Fase 2: Bergerak ke kiri (seperti obstacle)
            if (!switchedToBackward)
            {
                // Opsional: kamu bisa tambahkan animasi berubah arah di sini
                switchedToBackward = true;
            }

            transform.position += Vector3.left * backwardSpeed * Time.deltaTime;
        }
    }
}