using UnityEngine;

public class Bullet : Damager {
    
    public float speed = 0.1f;
    public float maxDistance = 100f;
    private float _distanceMoved = 0f;
    
    public Sprite sprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set sprite
        this.GetComponent<SpriteRenderer>().sprite = sprite;

        // Set collider size
        this.GetComponent<BoxCollider2D>().size =
            new Vector2(sprite.texture.width, sprite.texture.height) / sprite.pixelsPerUnit;

    }

    // Move
    void Update()
    {
        if (maxDistance < _distanceMoved) {Destroy(gameObject);}
        transform.position += transform.up * (speed * Time.deltaTime);
        _distanceMoved += (speed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
