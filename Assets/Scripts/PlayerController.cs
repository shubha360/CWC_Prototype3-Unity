using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;

    private float normalAnimSpeed = 1.2f;
    private float dashAnimSpeed = 2.0f;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    private AudioSource playerAudio;

    private GameManager gameManagerScript;
    private MoveLeft moveLeftScript;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    public float jumpSoundAudacity = 1.0f;
    public float crashSoundAudacity = 1.0f;

    private bool isOnGround = true;
    public bool gameOver = false;
    private bool jumpedAgain = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        moveLeftScript = GameObject.Find("Background").GetComponent<MoveLeft>();

        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {   
        if (moveLeftScript.dashModeActivated)
        {
            playerAnimator.speed = dashAnimSpeed;
        } else
        {
            playerAnimator.speed = normalAnimSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            if (isOnGround)
            {   
                isOnGround = false;
                playerAnimator.SetTrigger("Jump_trig");
                jump();
            } else if (!isOnGround && !jumpedAgain)
            {
                jumpedAgain = true;
                jump();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetFloat("Speed_f", 1);
            isOnGround = true;
            jumpedAgain = false;
            dirtParticle.Play();
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!gameOver)
            {
                Debug.Log("Game Over. Score: " + (int) gameManagerScript.score);
                playerAnimator.SetBool("Death_b", true);
                gameOver = true;
                explosionParticle.Play();
                dirtParticle.Stop();
                playerAudio.PlayOneShot(crashSound, crashSoundAudacity);
            }
        }
    }

    private void jump()
    {   
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        dirtParticle.Stop();
        playerAudio.PlayOneShot(jumpSound, jumpSoundAudacity);
    }
}
