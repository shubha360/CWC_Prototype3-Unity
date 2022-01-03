using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public float jumpForce;
    public float gravityModifier;
    public float walkingSpeed; // Speed of walking before the game is started
    
    // Particles
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    // Audio Clips
    public AudioClip jumpSound;
    public AudioClip crashSound;

    public float jumpSoundAudacity = 1.0f;
    public float crashSoundAudacity = 1.0f;

    private float normalAnimSpeed = 1.2f;
    private float dashAnimSpeed = 2.0f;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    private AudioSource playerAudio;

    private GameManager gameManagerScript;
    private MoveLeft moveLeftScript;

    public bool isOnGround = true;
    public bool gameOver = false;
    private bool jumpedAgain = false;  // To control double jumping
    public bool startGame = false; // TO determine if the game has started

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
        // Game is not started yet, player walks into the scene
        if (transform.position.x < 2)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * walkingSpeed);
            dirtParticle.Pause(); // No dirt while walking
        }
        else // Now the game has started
        {
            startGame = true;
            playerAnimator.SetFloat("Speed_f", 1); // Toggling the running animation

            if (isOnGround) // Checks if player is not in the air for a jump
            {
                dirtParticle.Play(); // Now the dirt should appear
            }
            
            // Faster animation speed for the dash mode
            if (moveLeftScript.dashModeActivated)
            {
                playerAnimator.speed = dashAnimSpeed;
            }
            else // Normal animation speed
            {
                playerAnimator.speed = normalAnimSpeed;
            }

            // Jumps if space bar is pressed
            if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
            {   
                // First jump
                if (isOnGround)
                {
                    isOnGround = false;
                    playerAnimator.SetTrigger("Jump_trig");
                    jump();
                }
                // Second jump if not already jumped for the second time
                else if (!isOnGround && !jumpedAgain)
                {
                    jumpedAgain = true;
                    playerAnimator.Play("Running_Jump", 3, 0f); // Forcing the jump animation to play again
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
                dirtParticle.Play();
            }
            
            isOnGround = true;
            jumpedAgain = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!gameOver) // Checking because there is possibility that the player can collide again after the game is over
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

    // Code for jumping
    private void jump()
    {   
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        dirtParticle.Pause(); // Dirt should pause
        dirtParticle.Clear(); // Clear the particles that are left
        playerAudio.PlayOneShot(jumpSound, jumpSoundAudacity);
    }
}