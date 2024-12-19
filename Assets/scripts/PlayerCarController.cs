using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; 

public class PlayerCarController : MonoBehaviour
{
    public Image destroyObstacleIcon;   

    private bool canDestroyObstacle = false;

    public float speed = 10f; 
    public float turnAngle = 15f; 
    public float turnResponseTime = 0.1f; 

    public float jumpForce = 8f; 

    public float miniJumpForce = 3f; 

    public float spaceJumpForce = 5f;

    private bool crashed = false;

    public TextMeshProUGUI object_to_destroy; 

    private Rigidbody rb; 

    [SerializeField] private bool isJumping = false;

    private ScoreCounter scoreCounter;

    public float destroyObjects = 1f;

    public ParticleSystem explosionEffectPrefab;

    private float currentSpeed = 0f;
    private float acceleration = 10f; 
    private float maxSpeed;


    public AudioClip crashSound;

    public AudioClip fellOffSound;

    public AudioClip jumpSound;

    public AudioClip rampJumpSound;

    public AudioClip jumpObstacleSound;

    public AudioClip landingSound;

    public AudioClip jumpDownSound;

    private AudioSource audioSource; 
    private AudioSource audioSourceFell; 
    private AudioSource audioSourceJump; 
    private AudioSource audioSourceRampJump;

    private AudioSource audioSourceObstacleJump;

    private AudioSource audioLandingSound;

    private AudioSource audioJumpDownSound;

private void Start()
{
    rb = GetComponent<Rigidbody>();
    scoreCounter = FindObjectOfType<ScoreCounter>();

    audioSource = gameObject.AddComponent<AudioSource>(); 
    audioSource.playOnAwake = false;
    audioSource.clip = crashSound;
    audioSource.volume = 1.0f; 

    audioSourceFell = gameObject.AddComponent<AudioSource>(); 
    audioSourceFell.playOnAwake = false;
    audioSourceFell.clip = fellOffSound;
    audioSourceFell.volume = 0.8f; 

    audioSourceJump = gameObject.AddComponent<AudioSource>(); 
    audioSourceJump.playOnAwake = false;
    audioSourceJump.clip = jumpSound;
    audioSourceJump.volume = 0.2f;

    audioSourceRampJump = gameObject.AddComponent<AudioSource>(); 
    audioSourceRampJump.playOnAwake = false;
    audioSourceRampJump.clip = rampJumpSound;
    audioSourceRampJump.volume = 0.4f; 

    audioSourceObstacleJump = gameObject.AddComponent<AudioSource>(); 
    audioSourceObstacleJump.playOnAwake = false;
    audioSourceObstacleJump.clip = jumpObstacleSound;
    audioSourceObstacleJump.volume = 0.3f; 

    audioLandingSound = gameObject.AddComponent<AudioSource>(); 
    audioLandingSound.playOnAwake = false;
    audioLandingSound.clip = landingSound;
    audioLandingSound.volume = 0.1f; 

    audioJumpDownSound = gameObject.AddComponent<AudioSource>(); 
    audioJumpDownSound.playOnAwake = false;
    audioJumpDownSound.clip = jumpDownSound;
    audioJumpDownSound.volume = 0.1f; 

    maxSpeed = speed;
}

    private void Update()
    {
        if (!isJumping)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;

                rb.AddForce(Vector3.up * spaceJumpForce, ForceMode.Impulse);

                PlayJumpSound();
            }

            rb.position = new Vector3(rb.position.x, rb.position.y, -2.816f);

            if (!((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))))
            {
                // Horizontal movement
                float horizontalInput = Input.GetAxis("Horizontal");
                Vector3 movement = Vector3.right * horizontalInput * speed * Time.deltaTime;
                transform.Translate(movement, Space.World); 

                // Visual turning
                float turn = horizontalInput * turnAngle;
                Quaternion turnRotation = Quaternion.Euler(-30, turn, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, turnRotation, Time.deltaTime / turnResponseTime);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * -spaceJumpForce, ForceMode.Impulse);

                PlayJumpDownSound();
            }

            if (!((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))))
            {
                // Horizontal movement
                float horizontalInput = Input.GetAxis("Horizontal");
                Vector3 movement = Vector3.right * horizontalInput * speed * Time.deltaTime;
                transform.Translate(movement, Space.World); 
            }

            rb.rotation = Quaternion.Euler(-30, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isJumping)
        {
            if (other.CompareTag("fell_right"))
            {
                isJumping = true;

                rb.useGravity = true; 
                rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z); 
                PlayFellOffSound();
            }

            if (other.CompareTag("fell_left"))
            {
                isJumping = true;

                rb.useGravity = true; 
                rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z); 
                PlayFellOffSound();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("obstacle"))
        {   
            if (!isJumping)
            {
                rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                rb.velocity = new Vector3(rb.velocity.x, -speed, -speed); 

                Instantiate(explosionEffectPrefab, collision.contacts[0].point, Quaternion.identity);

                PlayCrashSound();

                crashed = true;
            }
            else
            {
                Debug.Log("obstacle crash in the air");
                if (destroyObjects == 1)
                {
                    Debug.Log("allowed to destroy");
                    destroyObjects = destroyObjects - 1;

                    canDestroyObstacle = false;
                    UpdateUI();

                    Destroy(collision.collider.gameObject);

                    if (scoreCounter != null)
                    {
                        scoreCounter.IncreaseScore();
                    }

                    rb.velocity = new Vector3(rb.velocity.x, miniJumpForce, rb.velocity.z);

                    PlayObstacleJumpSound();
                }
                else
                {
                    Debug.Log("car not allowed to destroy, therefore dead");

                    rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                    rb.velocity = new Vector3(rb.velocity.x, -speed, -speed); 

                    Instantiate(explosionEffectPrefab, collision.contacts[0].point, Quaternion.identity);

                    PlayCrashSound();

                    crashed = true;
                }
            }
        }
        
        else if (collision.collider.CompareTag("mini_ramp"))
        {
            isJumping = true;
            
            destroyObjects = 1;
            
            canDestroyObstacle = true;
            UpdateUI();

            Debug.Log("now resetted destroy objects");

            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

            PlayRampJumpSound();
        }

        else if (collision.collider.CompareTag("ramp"))
        {
            if (isJumping)
            {
                PlayLandingSound();
            }

            destroyObjects = 0;

            canDestroyObstacle = false;
            UpdateUI();

            isJumping = false;
        }
    }

    private void PlayCrashSound()
    {
        if (audioSource != null && crashSound != null)
        {
            audioSource.Play();
        }
    }

    private void PlayFellOffSound()
    {
        if (audioSourceFell != null && fellOffSound != null)
        {
            audioSourceFell.Play();
        }
    }

    private void PlayJumpSound()
    {
        if (audioSourceJump != null && jumpSound != null)
        {
            audioSourceJump.Play();
        }
    }

    private void PlayRampJumpSound()
    {
        if (audioSourceRampJump != null && rampJumpSound != null)
        {
            audioSourceRampJump.Play();
        }
    }

    private void PlayObstacleJumpSound()
    {
        if (audioSourceObstacleJump != null && jumpObstacleSound != null)
        {
            audioSourceObstacleJump.Play();
        }
    }

    private void PlayLandingSound()
    {
        if (audioLandingSound != null && landingSound != null)
        {
            audioLandingSound.Play();
        }
    }

    private void PlayJumpDownSound()
    {
        if (audioJumpDownSound != null && jumpDownSound != null)
        {
            audioJumpDownSound.Play();
        }
    }

    private void FixedUpdate()
    {
        if (rb.position.y < -5 || crashed)
        {
            if (scoreCounter != null)
            {
                scoreCounter.CheckHighScore();
            }

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false; // Make the player invisible
            }

            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false; // Make the player non-interactive
            }

            StartCoroutine(WaitForSound());
        }
    }

    private void UpdateUI()
    {
        destroyObstacleIcon.enabled = canDestroyObstacle;
    }

    IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); 
    }
}
