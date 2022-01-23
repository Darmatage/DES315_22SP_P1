using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class HyungseobKim_TriggerEffect : MonoBehaviour
{
    public float destroyTime;
    public float speed;

    private float elapsedTime = 0.0f;
    
    private SpriteRenderer spriteRenderer;
    private Color color;

    private Quaternion rotation;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;

        rotation = gameObject.transform.rotation;
        originalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < destroyTime)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= destroyTime)
            {
                elapsedTime = destroyTime;
            }

            color.a = (destroyTime - elapsedTime) / destroyTime;
            spriteRenderer.color = color;

            rotation.z = elapsedTime * speed;
            gameObject.transform.rotation = rotation;

            Vector3 scale = originalScale;
            scale.x *= (1.0f + elapsedTime * speed);
            scale.y *= (1.0f + elapsedTime * speed);
            gameObject.transform.localScale = scale;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
