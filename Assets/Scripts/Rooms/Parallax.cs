﻿using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxEffect = 0.5f;

    private float spriteWidth;
    protected float startX;
    private float prevCam;
    private float offsetX;
    private float allOffset;
    public float OffsetSpeed;
    public bool isNotMoving;
    void Start()
    {
        allOffset = 0;
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        var sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
        startX = transform.position.x;
        prevCam = cameraTransform.position.x;
        offsetX = 0f;
    }

    void LateUpdate()
    {
        if (!isNotMoving)
        {
            float camX = cameraTransform.position.x;

            if (camX < prevCam)
            {
                float delta = prevCam - camX;
                offsetX += delta;
            }
            prevCam = camX;

            float newcamX = camX + offsetX;
            float offset = newcamX * parallaxEffect;

            float wrap = offset - Mathf.Floor(offset / spriteWidth) * spriteWidth;
            SetPosition(wrap);
        }
        else
        {
            allOffset += OffsetSpeed * Time.deltaTime;
            float offset = allOffset * parallaxEffect;

            float wrap = offset - Mathf.Floor(offset / spriteWidth) * spriteWidth;
            SetPosition(wrap);
        }
    }

    protected virtual void SetPosition(float wrap)
    {
        transform.position = new Vector3(startX + wrap, transform.position.y, transform.position.z);
    }
}
