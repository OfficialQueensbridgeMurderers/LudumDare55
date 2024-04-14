using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject fly;
    public GameObject leaper;
    public int round = 0;
    private int flyPerRound = 2;
    private int leapPerRound = 4; 
    private int flyToSpawn = 0;
    private int leapToSpawn = 0;
    public Transform rightSpawn;
    public Transform leftSpawn;
    public Transform botrightSpawn;
    public Transform botleftSpawn;
    public float spawnDelay = 2;
    public bool delaySpawn = true;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    bool allEnemiesSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        NewRound();
    }

    private void NewRound(){
        round++;
        spawnedEnemies = new List<GameObject>();
        flyToSpawn = round * flyPerRound;
        leapToSpawn = round * leapPerRound;
        allEnemiesSpawned = false;
        Invoke(nameof(ResetSpawnDelay), spawnDelay);
        spawnDelay -= 0.5f;
        if (spawnDelay < 1){
            spawnDelay = 1;
        }

        float enemyDamage = round / 2;
        if (enemyDamage < 1){
            enemyDamage = 1;
        }

        GameObject.Find("Player").GetComponent<PlayerController>().enemyDamage = enemyDamage;
    }

    private void ResetSpawnDelay(){
        delaySpawn = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!allEnemiesSpawned && !delaySpawn)
        {
            if (leapToSpawn > 0){
                if (leapToSpawn == 1){
                    spawnedEnemies.Add(Instantiate(leaper, leftSpawn.position, Quaternion.identity));
                    leapToSpawn--;
                }
                else if (leapToSpawn >= 4){
                    spawnedEnemies.Add(Instantiate(leaper, leftSpawn.position, Quaternion.identity));
                    leapToSpawn--;

                    spawnedEnemies.Add(Instantiate(leaper, rightSpawn.position, Quaternion.identity));
                    leapToSpawn--;

                    spawnedEnemies.Add(Instantiate(leaper, botleftSpawn.position, Quaternion.identity));
                    leapToSpawn--;

                    spawnedEnemies.Add(Instantiate(leaper, botrightSpawn.position, Quaternion.identity));
                    leapToSpawn--;
                }
                else{
                    spawnedEnemies.Add(Instantiate(leaper, leftSpawn.position, Quaternion.identity));
                    leapToSpawn--;

                    spawnedEnemies.Add(Instantiate(leaper, rightSpawn.position, Quaternion.identity));
                    leapToSpawn--;
                }
            }
            else if (flyToSpawn > 0){
                if (flyToSpawn == 1){
                    spawnedEnemies.Add(Instantiate(fly, leftSpawn.position, Quaternion.identity));
                    flyToSpawn--;
                }
                else if (flyToSpawn >= 4){
                    spawnedEnemies.Add(Instantiate(fly, leftSpawn.position, Quaternion.identity));
                    flyToSpawn--;

                    spawnedEnemies.Add(Instantiate(fly, rightSpawn.position, Quaternion.identity));
                    flyToSpawn--;

                    spawnedEnemies.Add(Instantiate(fly, botleftSpawn.position, Quaternion.identity));
                    flyToSpawn--;

                    spawnedEnemies.Add(Instantiate(fly, botrightSpawn.position, Quaternion.identity));
                    flyToSpawn--;
                }
                else{
                    spawnedEnemies.Add(Instantiate(fly, leftSpawn.position, Quaternion.identity));
                    flyToSpawn--;

                    spawnedEnemies.Add(Instantiate(fly, rightSpawn.position, Quaternion.identity));
                    flyToSpawn--;
                }
            }

            if (leapToSpawn == 0 && flyToSpawn == 0){
                allEnemiesSpawned = true;
            }

            delaySpawn = true;
            Invoke(nameof(ResetSpawnDelay), spawnDelay);

        }

        if (AllEnemiesDead() && allEnemiesSpawned){
            NewRound();
        }
    }

    private bool AllEnemiesDead(){
        if (spawnedEnemies.Count == 0){
            return false;
        }

        foreach(GameObject obj in spawnedEnemies){
            if (obj != null && !obj.GetComponent<Enemy>().dead){
                return false;
            }
        }
        return true;
    }
}
