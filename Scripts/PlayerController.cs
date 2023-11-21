using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public bool gameOver = false;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    public float jumpForce = 15.0f;
    public float gravityModifier = 2.0f;
    private bool isGrounded = true;
    private Animator playerAnimation;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // Player jumps when spacebar key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            playerAnimation.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        // Prevents the player from falling over
        if (transform.position.x != 0 || transform.position.z != 0)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
        if (transform.rotation.x != 0 || transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Enables jump when grounded
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            isGrounded = true;
            dirtParticle.Play();
        }

        // Stops the game when hitting obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!gameOver)
            {
                explosionParticle.Play();
                playerAudio.PlayOneShot(crashSound, 1.0f);
            }

            gameOver = true;
            Debug.Log("Game Over!");
            playerAnimation.SetInteger("DeathType_int", 1);
            playerAnimation.SetBool("Death_b", true);
            dirtParticle.Stop();
        }
    }
}