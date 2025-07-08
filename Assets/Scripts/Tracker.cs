using UnityEngine;
using UnityEngine.UI;

public class DistanceTrackerWithBar : MonoBehaviour
{
    public Transform playerA;
    public string yotsubaTag = "Yotsuba";
    public Slider distanceSlider;
    public float maxDistance = 30f;

    private Transform yotsuba;

    void Start()
    {
        distanceSlider.gameObject.SetActive(true); 
    }

    void Update()
    {
        if (yotsuba == null || !yotsuba.gameObject.activeInHierarchy)
        {
            GameObject obj = GameObject.FindWithTag(yotsubaTag);

            if (obj != null)
            {
                yotsuba = obj.transform;
            }
            else
            {
                distanceSlider.value = 0f;
                yotsuba = null;
                return;
            }
        }

        float distance = Vector3.Distance(playerA.position, yotsuba.position);
        float normalized = Mathf.Clamp01(1 - (distance / maxDistance));
        distanceSlider.value = normalized;
    }
}
