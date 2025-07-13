using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Mechanic.Itsuki;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private TextAsset chartJson;
    [SerializeField] private string filter = "quickPress";
    [SerializeField] private float buffer = 2f;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int countdownDuration = 3;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform ground;

    [SerializeField] private List<PlatformMiku.TileSprites> tileSprites;

    private struct SpawnInfo
    {
        public float spawnLocal;    
        public float hitTime;      
        public Transform point;    
        public int pointIndex;     
    }
    private List<SpawnInfo> spawnEvents;

    void Start()
    {
        LoadSpawnEvents();
        StartCoroutine(CountdownAndBegin());
    }

    private void LoadSpawnEvents()
    {
        if (chartJson == null) return;
        var data = JsonUtility.FromJson<RhythmData>(chartJson.text);
        var hits = data.note_events.Where(e => e.type == filter).Select(e => e.timestamp).OrderBy(t => t).ToList();

        spawnEvents = hits.Select(hit =>
        {
            int idx = UnityEngine.Random.Range(0, spawnPoints.Length);
            return new SpawnInfo
            {
                hitTime = hit,
                spawnLocal = hit - buffer,
                point = spawnPoints[idx],
                pointIndex = idx
            };  
        })
        .OrderBy(s => s.spawnLocal)
        .ToList();
    }

    private IEnumerator CountdownAndBegin()
    {
        for (int i = countdownDuration; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "";

        audioSource.Play();
        float audioStartTime = Time.time;

        StartCoroutine(WaitSongEnd());
        StartCoroutine(SpawnRoutine(audioStartTime));
    }

    private enum GroundType
    {
        Default,
        Success,
        Fail,
    }
    private IEnumerator SpawnRoutine(float audioStartTime)
    {
        int i = 0;
        foreach (var tile in spawnEvents)
        {
            float spawnAbs = audioStartTime + tile.spawnLocal;
            float wait = spawnAbs - Time.time;
            if (wait > 0f)
                yield return new WaitForSeconds(wait);
            else
                yield return null;

            if (tile.hitTime > buffer)
            {
                var go = Instantiate(tilePrefab, tile.point.position, Quaternion.identity);
                go.name = $"Tile_{i++}";
                var mover = go.GetComponent<PlatformMiku>();
                if (mover != null)
                    mover.Initialize(audioStartTime + tile.hitTime, ground, tileSprites[tile.pointIndex]);
            }
        }
    }

    private IEnumerator WaitSongEnd()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        StageManager.Instance.Win();
    }

    [Serializable]
    private class RhythmData
    {
        public List<NoteEvent> note_events;
    }

    [Serializable]
    private class NoteEvent
    {
        public string type;
        public float timestamp;
    }
}
