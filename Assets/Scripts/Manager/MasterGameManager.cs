using System.Collections;
using UnityEngine;

public class MasterGameManager : MonoBehaviour
{
    public static MasterGameManager Instance { get; private set; }
    bool gameStarted;
    bool introPrompt;
    Coroutine introRoutine;
    public bool IsGameStarted => gameStarted;
    public bool IntroPrompt => introPrompt;
    [field: SerializeField]
    public bool MikuWin { get; set; }
    [field: SerializeField]
    public bool NinoWin { get; set; }
    [field: SerializeField]
    public bool IchikaWin { get; set; }
    [field: SerializeField]
    public bool ItsukiWin { get; set; }
    [field: SerializeField]
    public bool YotsubaWin { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        gameStarted = false;
        introPrompt = false;
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (introRoutine == null)
                introRoutine = StartCoroutine(GameIntro());
            if (introPrompt && Input.anyKeyDown)
                StartGame();
        }
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;
        introPrompt = false;

        if (introRoutine != null)
        {
            StopCoroutine(introRoutine);
            introRoutine = null;
        }
    }

    IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(1f);
        introPrompt = true;
        introRoutine = null;
    }
}
