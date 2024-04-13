using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoulType{
    Leaper,
    Flying
}

public class SacrificeManager : MonoBehaviour
{
    int leaperSacrifices = 0;
    int leaperSacrificesNeeded = 3;
    int flyingSacrifices = 0;
    int flyingSacrificesNeeded = 3;
    public Transform bigLeaperSpawnPos;
    public GameObject bigLeaper;
    public GameObject soulCollectorPrefab;
    GameObject spawnedSoulCollector;
    float soulCollectorDuration = 15;
    float elapsed = 0;
    bool soulCollector = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (soulCollector){
            elapsed += Time.deltaTime;
            if (elapsed >= soulCollectorDuration){
                soulCollector = false;
                Destroy(spawnedSoulCollector);
            }
        }
    }

    public void SacrificeLeaper(Vector3 position)
    {
        if (soulCollector){
            CollectSoul(position, SoulType.Leaper);
        }
        else{
            leaperSacrifices++;

            if (leaperSacrificesNeeded == leaperSacrifices){
                leaperSacrifices = 0;
                //Debug.Log("Summon Big Leaper");
                Instantiate(bigLeaper, bigLeaperSpawnPos.position, Quaternion.identity);
            }
        }
    }

    public void SacrificeFlyingEnemy(Vector3 position)
    {
        if (soulCollector){
            CollectSoul(position, SoulType.Flying);
        }
        else{
            flyingSacrifices++;

            if (flyingSacrificesNeeded == flyingSacrifices){
                flyingSacrifices = 0;
                spawnedSoulCollector = Instantiate(soulCollectorPrefab, bigLeaperSpawnPos.position, Quaternion.identity);
                soulCollector = true;
                elapsed = 0;
            }
        }
        
    }

    private void CollectSoul(Vector3 position, SoulType type){
        spawnedSoulCollector.transform.position = position;
    }
}
