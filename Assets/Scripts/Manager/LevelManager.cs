using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void ChangeLevel(SceneID source, SceneID target, bool win)
    {
        Debug.Log("Level CHanged");
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
}
