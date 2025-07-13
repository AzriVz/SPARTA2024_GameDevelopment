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

    private struct SpawnInfo
    {
        public float spawnLocal;   
        public float hitTime;      
        public Transform point;
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
        var hitTimes = data.note_events
            .Where(e => e.type == filter)
            .Select(e => e.timestamp)
            .OrderBy(t => t)
            .ToList();

        spawnEvents = new List<SpawnInfo>(hitTimes.Count);
        foreach (float hit in hitTimes)
        {
            float local = Mathf.Max(0f, hit - buffer);
            var pt = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            spawnEvents.Add(new SpawnInfo { spawnLocal = local, hitTime = hit, point = pt });
        }
        spawnEvents = spawnEvents.OrderBy(e => e.spawnLocal).ToList();
    }

    private IEnumerator CountdownAndBegin()
    {
        for (int i = countdownDuration; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "";
        float audioStartTime = Time.time;
        yield return new WaitForSeconds(buffer);
        audioSource.Play();
        StartCoroutine(WaitSongEnd());
        StartCoroutine(SpawnRoutine(audioStartTime));
    }

    private IEnumerator SpawnRoutine(float audioStartTime)
    {
        float lastLocal = 0f;
        foreach (var tile in spawnEvents)
        {
            float spawnAbs = audioStartTime + tile.spawnLocal;
            float wait = spawnAbs - Time.time;
            if (wait > 0f) yield return new WaitForSeconds(wait);
            else yield return null;

            var go = Instantiate(tilePrefab, tile.point.position, Quaternion.identity);
            var mover = go.GetComponent<PlatformMiku>();
            if (mover != null)
                mover.Initialize(audioStartTime + tile.hitTime, ground);

            lastLocal = tile.spawnLocal;
        }
    }

    private IEnumerator WaitSongEnd()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        SongEnd();
    }

    private void SongEnd()
    {
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
