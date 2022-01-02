using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public float walkingSpeed;

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

    public bool isOnGround = true;
    public bool gameOver = false;
    private bool jumpedAgain = false;

    public bool startGame = false;

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
        if (transform.position.x < 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * walkingSpeed);
            dirtParticle.Pause();
        }
        else
        {
            startGame = true;
            playerAnimator.SetFloat("Speed_f", 1);
            dirtParticle.Play();

            if (moveLeftScript.dashModeActivated)
            {
                playerAnimator.speed = dashAnimSpeed;
            }
            else
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
                }
                else if (!isOnGround && !jumpedAgain)
                {
                    jumpedAgain = true;
                    playerAnimator.Play("Running_Jump", 3, 0f);
                    jump();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {   
            if (startGame)
            {
                playerAnimator.SetFloat("Speed_f", 1);
                dirtParticle.Play();
            }
            
            isOnGround = true;
            jumpedAgain = false;
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
        dirtParticle.Pause();
        playerAudio.PlayOneShot(jumpSound, jumpSoundAudacity);
    }
}
