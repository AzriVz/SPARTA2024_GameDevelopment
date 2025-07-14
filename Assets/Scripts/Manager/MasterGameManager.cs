using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class MasterGameManager : MonoBehaviour
{
    public static MasterGameManager Instance { get; private set; }
    public bool gameStarted { get; set; }
    bool introPrompt;
    Coroutine introRoutine;

    public bool IntroPrompt => introPrompt;
    [field: SerializeField]
    public int TryLeft { get; set; }
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

    public int CountSister()
    {
        int count = 0;
        if (MikuWin) count++;
        if (NinoWin) count++;
        if (IchikaWin) count++;
        if (ItsukiWin) count++;
        if (YotsubaWin) count++;
        return count;
    }

    public void GoodEnd()
    {
        var ie = FindFirstObjectByType<DialogueRunner>();
        ie.StartDialogue("End_Good");
    }
    public void BadEnd()
    {
        var ie = FindFirstObjectByType<DialogueRunner>();
        ie.StartDialogue("End_Bad");
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

    [YarnCommand("CloseApp")]
    public static void CloseApp()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(1f);
        introPrompt = true;
        introRoutine = null;
    }
}
