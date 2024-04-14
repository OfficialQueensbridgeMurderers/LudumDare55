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
    public int kills = 0;
    PlayerController player;
    GameObject spawnedBigLeaper = null;
    
    public GameObject red1;
    public GameObject red2;
    public GameObject red3;
    public GameObject blue1;
    public GameObject blue2;
    public GameObject blue3;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        UpdateIndicators();
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
        kills++;
        if (soulCollector){
            CollectSoul(position, SoulType.Leaper);
        }
        else{
            leaperSacrifices++;

            if (leaperSacrificesNeeded == leaperSacrifices && spawnedBigLeaper == null){
                leaperSacrifices = 0;
                //Debug.Log("Summon Big Leaper");
                spawnedBigLeaper = Instantiate(bigLeaper, bigLeaperSpawnPos.position, Quaternion.identity);
            }
        }
        UpdateIndicators();
    }

    public void SacrificeFlyingEnemy(Vector3 position)
    {
        kills++;
        if (soulCollector){
            CollectSoul(position, SoulType.Flying);
        }
        else{
            flyingSacrifices++;

            if (flyingSacrificesNeeded == flyingSacrifices){
                flyingSacrifices = 0;
                spawnedSoulCollector = Instantiate(soulCollectorPrefab, bigLeaperSpawnPos.position + new Vector3(0, 0, 2), Quaternion.identity);
                soulCollector = true;
                elapsed = 0;
            }
        }
        UpdateIndicators();
    }

    private void UpdateIndicators(){
        red1.SetActive(false);
        red2.SetActive(false);
        red3.SetActive(false);
        blue1.SetActive(false);
        blue2.SetActive(false);
        blue3.SetActive(false);

        if (flyingSacrifices >= 1){
            blue1.SetActive(true);
        }

        if (flyingSacrifices >= 2){
            blue2.SetActive(true);
        }

        if (flyingSacrifices >= 3){
            blue3.SetActive(true);
        }

        if (leaperSacrifices >= 1){
            red1.SetActive(true);
        }

        if (leaperSacrifices >= 2){
            red2.SetActive(true);
        }

        if (leaperSacrifices >= 3){
            red3.SetActive(true);
        }
    }

    private void CollectSoul(Vector3 position, SoulType type){
        spawnedSoulCollector.transform.position = position + new Vector3(0, 0, 2);
        if (player.IsFullHealth()){
            switch(type){
                case SoulType.Leaper:
                    player.GiveRange(1);
                    break;
                case SoulType.Flying:
                    player.GiveMaxHealth(1);
                    break;
            }
        }
        else{
            player.Heal(1);
        }
        
    }
}
