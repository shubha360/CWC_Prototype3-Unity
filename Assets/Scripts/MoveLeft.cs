using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float movingSpeed = 25;
    public float dashSpeed = 40;

    private PlayerController playerControllerScript;
    private float leftBound = -10;

    public bool dashModeActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!playerControllerScript.gameOver)
        {
            if (Input.GetKey(KeyCode.F))
            {
                dashModeActivated = true;
                transform.Translate(Vector3.left * dashSpeed * Time.deltaTime);
            } else
            {
                dashModeActivated = false;
                transform.Translate(Vector3.left * movingSpeed * Time.deltaTime);
            }
        }

        if (gameObject.CompareTag("Obstacle"))
        {
            if (transform.position.x < leftBound)
            {
                Destroy(gameObject);
            }
        }
    }
}
