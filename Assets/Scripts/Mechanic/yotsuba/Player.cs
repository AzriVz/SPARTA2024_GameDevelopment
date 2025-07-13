using System;
using Mechanic.Itsuki;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private bool isGroundedPrev;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    private Health _playerHealth;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Start()
    {
        _playerHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        direction += gravity * Time.deltaTime * Vector3.down;

        if(!isGroundedPrev && character.isGrounded){
        {
            AudioManager.instance.PlaySFX("LandHurdle");
        }}
        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (Input.GetButton("Jump"))
            {
                AudioManager.instance.PlaySFX("JumpHurdle");
                direction = Vector3.up * jumpForce;
            }
        }

        character.Move(direction * Time.deltaTime);
        isGroundedPrev = character.isGrounded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _playerHealth.Damage();
            // GameManager.Instance.GameOver();
        }
        else if (other.CompareTag("RealYotsuba"))
        {
            StageManager.Instance.Win();
            // StopBothCharacters(other.gameObject);
        }     
    }

    private void StopBothCharacters(GameObject yotsuba)
    {
        this.enabled = false;

        MonoBehaviour[] scripts = yotsuba.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        Time.timeScale = 0f;
    }
}