using UnityEngine;
using System.Collections;

public class CurtainRandomizer : MonoBehaviour
{
    public GameObject[] curtains; 
    public Sprite defaultCurtainSprite;
    public Sprite ichikaCurtainSprite;
    public float revealDuration = 2f;
    public float shuffleDuration = 2f; 
    public float moveSpeed = 5f;
    public float singleShuffleDuration = 0.5f;
    public int totalShuffles = 5;
    
    private Vector2[] originalPositions;
    private bool isShuffling = false;
    private int ichikaCurtainIndex;
    

    void Start()
    {
        originalPositions = new Vector2[curtains.Length];
        for (int i = 0; i < curtains.Length; i++)
        {
            originalPositions[i] = curtains[i].transform.position;
            curtains[i].tag = "Untagged";
        }

        ichikaCurtainIndex = Random.Range(0, curtains.Length);

        curtains[ichikaCurtainIndex].GetComponent<SpriteRenderer>().sprite = ichikaCurtainSprite;
        curtains[ichikaCurtainIndex].GetComponent<Curtain>().ichikaCurtain = true;

    }

    public IEnumerator GameSequence()
    {   
        yield return new WaitForSeconds(revealDuration);
        curtains[ichikaCurtainIndex].GetComponent<SpriteRenderer>().sprite = ichikaCurtainSprite;
        yield return new WaitForSeconds(revealDuration);
        
        for (int i = 0; i < curtains.Length; i++)
        {
            curtains[i].GetComponent<SpriteRenderer>().sprite = defaultCurtainSprite;
            curtains[i].tag = "Untagged";
        }
        
        yield return StartCoroutine(PairwiseShuffle());
        
    }

    IEnumerator PairwiseShuffle()
    {
        if (isShuffling) yield break;
        
        isShuffling = true;
        
        for (int i = 0; i < totalShuffles; i++)
        {
            int curtainA, curtainB;
            
            int pairType = Random.Range(0, 3);
            switch(pairType)
            {
                case 0: 
                    curtainA = 0;
                    curtainB = 1;
                    break;
                case 1: 
                    curtainA = 1;
                    curtainB = 2;
                    break;
                case 2: 
                    curtainA = 0;
                    curtainB = 2;
                    break;
                default:
                    curtainA = 0;
                    curtainB = 1;
                    break;
            }

            Vector2 posA = curtains[curtainA].transform.position;
            Vector2 posB = curtains[curtainB].transform.position;
            
            float elapsedTime = 0f;
            while (elapsedTime < singleShuffleDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsedTime / singleShuffleDuration);
                
                curtains[curtainA].transform.position = Vector2.Lerp(posA, posB, t);
                curtains[curtainB].transform.position = Vector2.Lerp(posB, posA, t);
                
                yield return null;
            }

            curtains[curtainA].transform.position = posB;
            curtains[curtainB].transform.position = posA;
                        
            yield return new WaitForSeconds(0.1f);
        }
        
        isShuffling = false;
        
        for (int i = 0; i < curtains.Length; i++)
        {
            curtains[i].tag = "Interactable";
        }

    }

    public void ResetGame()
    {
        StopAllCoroutines();
        
        for (int i = 0; i < curtains.Length; i++)
        {
            curtains[i].transform.position = originalPositions[i];
            curtains[i].GetComponent<SpriteRenderer>().sprite = defaultCurtainSprite;
        }
        
        ichikaCurtainIndex = Random.Range(0, curtains.Length);
        curtains[ichikaCurtainIndex].GetComponent<SpriteRenderer>().sprite = ichikaCurtainSprite;
        
        StartCoroutine(GameSequence());
    }
}
