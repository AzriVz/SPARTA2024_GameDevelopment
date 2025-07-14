using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionToggle : MonoBehaviour
{
    [SerializeField] private GameObject option;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            option.gameObject.SetActive(!option.activeSelf);
            Time.timeScale = option.activeSelf ? 0.0f : 1.0f;
        }
    }

    public void ReturnToMenu()
    {
        MasterGameManager.Instance.gameStarted = false;
        SceneManager.LoadScene(0);
    }
}
