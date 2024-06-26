using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    float xScaleFactor = 0;
    float yScaleFactor = 0;
    float duration = 0.25f;
    float elapsed = 0;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        //float xScale = Mathf.PingPong(elapsed / duration, maxScale) * xScaleFactor;
        //float yScale = Mathf.PingPong(elapsed / duration, maxScale) * yScaleFactor;
        float xScale = (elapsed / duration) * player.range * xScaleFactor;
        float yScale = (elapsed / duration) * player.range * yScaleFactor;
        if (xScale == 0){
            xScale = 1;
        }

        if (yScale == 0){
            yScale = 1;
        }

        transform.localScale = new Vector3(xScale, yScale, 1);

        if (elapsed >= duration){
            Destroy(gameObject);
        }
    }

    public void SetScaleFactor(float xScaleFactor, float yScaleFactor){
        this.xScaleFactor = xScaleFactor;
        this.yScaleFactor = yScaleFactor;
    }
}
