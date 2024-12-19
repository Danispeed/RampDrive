using UnityEngine;

public class CloudWiggle : MonoBehaviour
{
    public float wiggleAmount = 0.5f;
    public float wiggleSpeed = 2f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; 
    }

    void Update()
    {
        transform.position = originalPosition + new Vector3(Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount, 0f, 0f);
    }
}
