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
    [SerializeField] public bool NinoWin { get; set; }
    [SerializeField] public bool IchikaWin { get; set; }
    [SerializeField] public bool ItsukiWin { get; set; }
    [SerializeField] public bool YotsubaWin { get; set; }

    void Awake()
    {
        MikuWin = true;
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
        yield return new WaitForSeconds(3f);
        introPrompt = true;
        introRoutine = null;
    }
}
