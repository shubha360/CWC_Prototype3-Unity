using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public float score = 0;
    private float toIncrease = 20;

    MoveLeft moveLeftScript;

    // Start is called before the first frame update
    void Start()
    {
        moveLeftScript = GameObject.Find("Background").GetComponent<MoveLeft>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (moveLeftScript.dashModeActivated)
        {
            score += toIncrease * Time.deltaTime * 5; // 5 times more points for dash mode
        } else
        {
            score += toIncrease * Time.deltaTime;
        }
        
    }
}