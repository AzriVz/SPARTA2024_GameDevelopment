using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

Dictionary<LevelManager.SceneID, string> dialogName = new() {
    { LevelManager.SceneID.LevelMiku, "Miku" },
    { LevelManager.SceneID.LevelYotsuba, "Yotsuba" },
    { LevelManager.SceneID.LevelItsuka, "Itsuka" },
    { LevelManager.SceneID.LevelIchika, "Ichika" },
    { LevelManager.SceneID.LevelNino, "Nino" },
};

public class LevelManager : MonoBehaviour
{
    public enum SceneID
    {
        MainMenu,
        MapRoom,
        LevelMiku,
        LevelYotsuba,
        LevelItsuka,
        LevelIchika,
        LevelNino,
        Ending
    }

    [Serializable]
    struct SceneMapping
    {
        public SceneID id;
        public string sceneName;
    }

    [SerializeField] private SceneMapping[] s_sceneMapping;

    private Vector3 lastMapRoomPosition;
    private bool hasSavedMapPosition;

    public static LevelManager Instance { get; private set; }

    private static SceneID? NextSourceId = null;
    private static SceneID? NextTagetId = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // public void ChangeLevel(string source, string target, bool win)
    // {
    //     SceneID sourceId = GetSceneId(source);
    //     SceneID targetId = GetSceneId(target);
    //     ChangeLevel(sourceId, targetId, win);
    // }

    public void ChangeLevel(SceneID source, SceneID target, bool win)
    {
        string sourceName = GetSceneName(target);
        if (string.IsNullOrEmpty(sourceName))
        {
            string w = win ? "Win" : "Lose";
            NextSourceId = source;
            NextTagetId = target;
            var dr = FindFirstObjectByType<DialogueRunner>();
            dr.StartDialogue($"{sourceName}_{w}");
        }
        else if (source == SceneID.MapRoom)
        {
            string targetName = GetSceneName(target);
            if (string.IsNullOrEmpty(targetName))
            {
                Debug.LogError($"Target scene '{target}' not found in scene mapping.");
                return;
            }
            NextSourceId = source;
            NextTagetId = target;
            var dr = FindFirstObjectByType<DialogueRunner>();
            dr.StartDialogue($"{sourceName}_Pre");
        }
        else
        {
            ChangeLevelActual(source, target, win);
        }
    }

    [YarnCommand("LoadNextLevel")]
    public static void LoadNextLevel(bool win)
    {
        if (NextSourceId == null || NextTagetId == null)
        {
            Debug.LogError("NextSourceId or NextTagetId is not set. Cannot load next level.");
            return;
        }

        SceneID sourceId = NextSourceId.Value;
        SceneID targetId = NextTagetId.Value;
        NextSourceId = NextTagetId = null;
        Instance.ChangeLevelActual(sourceId, targetId, win);
    }

    public void ChangeLevelActual(SceneID source, SceneID target, bool win)
    {
        if (Instance == null) return;
        string current = SceneManager.GetActiveScene().name;
        string mapName = Instance.GetSceneName(SceneID.MapRoom);
        if (current == mapName)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Instance.lastMapRoomPosition = player.transform.position;
                Instance.hasSavedMapPosition = true;
            }
        }
        string targetName = Instance.GetSceneName(target);

        if (win)
        {
            var gm = MasterGameManager.Instance;
            if (source == SceneID.LevelMiku) gm.MikuWin = true;
            else if (source == SceneID.LevelYotsuba) gm.YotsubaWin = true;
            else if (source == SceneID.LevelItsuka) gm.ItsukiWin = true;
            else if (source == SceneID.LevelIchika) gm.IchikaWin = true;
            else if (source == SceneID.LevelNino) gm.NinoWin = true;
        }

        if (!string.IsNullOrEmpty(targetName))
            SceneManager.LoadScene(targetName);
    }

    private void OnSceneLoaded(Scene loaded, LoadSceneMode mode)
    {
        string mapName = GetSceneName(SceneID.MapRoom);
        if (loaded.name == mapName && hasSavedMapPosition)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = lastMapRoomPosition;
        }
    }

    private string GetSceneName(SceneID id)
    {
        for (int i = 0; i < s_sceneMapping.Length; i++)
            if (s_sceneMapping[i].id == id)
                return s_sceneMapping[i].sceneName;
        return null;
    }

    private SceneID GetSceneId(string name) { return s_sceneMapping.FirstOrDefault(v => v.sceneName == name).id; }
}
