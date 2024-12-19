using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    private Rigidbody rb;
    private ScoreCounter scoreCounter;

    private float countAllowedToDestroy = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        scoreCounter = FindObjectOfType<ScoreCounter>();
    }

    void FixedUpdate()
    {
        if (rb.position.y < -8)
        {
            if (scoreCounter != null)
            {
                scoreCounter.IncreaseScore();
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.tag == "mini_ramp")
        {
            if (collision.collider.CompareTag("player"))
            {
                if (scoreCounter != null)
                {
                    scoreCounter.IncreaseScore();
                }

                Destroy(gameObject);
            }
        }
        
        else if (collision.collider.CompareTag("obstacle"))
        {
            countAllowedToDestroy = countAllowedToDestroy - 1;

            if (countAllowedToDestroy == 0)
            {
                Destroy(gameObject);

                countAllowedToDestroy = 3;
            }
            else
            {
                if (Mathf.Approximately(transform.position.x, collision.collider.transform.position.x))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
