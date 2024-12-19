using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Renderer not found on the ramp object!");
        }
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = new Vector2(0, -offset);
    }
}
