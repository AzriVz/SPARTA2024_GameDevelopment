using System.Collections;
using UnityEngine;

public class MasterGameManager : MonoBehaviour
{
    public static MasterGameManager Instance { get; private set; }

    private bool gameStarted;
    private bool introPrompt;
    private Coroutine introRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        gameStarted = false;
        introPrompt = false;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (introRoutine == null) StartCoroutine(GameIntro());
            if (Input.GetKeyDown(KeyCode.Space) && introPrompt)
            {
                gameStarted = true;
            }
        }
    }

    IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(3f);
        introPrompt = true;
    }
}
