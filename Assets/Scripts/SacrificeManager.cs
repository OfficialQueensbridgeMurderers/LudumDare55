using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeManager : MonoBehaviour
{
    int leaperSacrifices = 0;
    int leaperSacrificesNeeded = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SacrificeLeaper()
    {
        leaperSacrifices++;

        if (leaperSacrificesNeeded == leaperSacrifices){
            leaperSacrifices = 0;
            Debug.Log("Summon Big Leaper");
        }
    }
}
