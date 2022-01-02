using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float movingSpeed = 25;

    private PlayerController playerControllerScript;
    private float leftBound = -10;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!playerControllerScript.gameOver)
        transform.Translate(Vector3.left * movingSpeed * Time.deltaTime);

        if (gameObject.CompareTag("Obstacle"))
        {
            if (transform.position.x < leftBound)
            {
                Destroy(gameObject);
            }
        }
    }
}
