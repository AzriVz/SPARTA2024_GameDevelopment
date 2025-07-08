using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum SceneID
    {
        MainMenu,
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
    }

    public static void ChangeLevel(SceneID scene)
    {
        if (Instance == null) return;
        for (int i = 0; i < Instance.s_sceneMapping.Length; i++)
        {
            var sceneValue = Instance.s_sceneMapping[i];
            if (sceneValue.id == scene)
            {
                SceneManager.LoadScene(sceneValue.sceneName);
                return;
            }
        }
    }
}
