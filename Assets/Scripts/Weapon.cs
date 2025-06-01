using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour {
    public int roundsMax = 5;
    public float roundDelay = 0.1f;
    public bool firing = false;

    public int roundsRemaining;
    private float _sinceLastShot = float.MaxValue;

    public float reloadTime = 2f;
    public bool reloading = false;
    private float _reloadProgress = 0f;

    public float accLosePerRound = 0.2f;
    public float accGainPerSecond = 1f;
    public float accMaxSpread = 45f;

    private int damage = 0;
    private float _accCur = 0f;

    public string name = "Weapon";

    public bool held = false;

    public Vector2 gunOffset = Vector3.zero;
    public Vector2 bulletOffset = Vector3.zero;

    public GameObject bullet;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        roundsRemaining = roundsMax;

        Sprite renderer = gameObject.GetComponent<SpriteRenderer>().sprite;
        
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(
            renderer.texture.width, 
            renderer.texture.height) / renderer.pixelsPerUnit;

        if (held == false) gameObject.GetComponent<PlayerWeapon>().enabled = false;
    }

    public void Attach(GameObject obj) {
        transform.SetParent(obj.transform); 
        transform.localPosition = gunOffset;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        gameObject.GetComponent<PlayerWeapon>().enabled = true;
    }
    
    void Update() {
        // If we aren't ready to shoot, increase shot delay timer
        if (_sinceLastShot < roundDelay) _sinceLastShot += Time.deltaTime;
        // Adjust recoil
        _accCur = Mathf.Max(0, _accCur - (accGainPerSecond * Time.deltaTime));

        // If we are reloading
        if (reloading) {
            _reloadProgress += Time.deltaTime; // Increment reload timer

            if (_reloadProgress >= reloadTime) { // Is the reload timer up?
                reloading = false;               // Stop reloading
                roundsRemaining = roundsMax;     // Gun now full of bullets
                _reloadProgress = 0f;            // Restart timer
            }
        }
        
        // Shoot if we can
        if (!reloading && firing && roundsRemaining > 0 && _sinceLastShot >= roundDelay) {
            _sinceLastShot = 0; // Reset shot timer
            roundsRemaining--; // Remove round from gun
            reloading = roundsRemaining == 0; // If gun is empty, automatically begin reloading
            
            GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation); // Fire bullet
            bulletClone.transform.localPosition += bulletClone.transform.right * bulletOffset.x;
            bulletClone.transform.localPosition += bulletClone.transform.up * bulletOffset.y;
            bulletClone.GetComponent<Bullet>().damage = damage;
            
            
            // Throw bullet off angle based off of current accuracy
            Vector3 angles = bulletClone.transform.rotation.eulerAngles;
            angles.z += Random.Range(-_accCur, _accCur);
            bulletClone.transform.rotation = Quaternion.Euler(angles);
            
            // // Rehome bullet to root so it down't get messed with by parent movement/rotation
            // bulletClone.transform.parent = transform.root;
            
            // Decrease accuracy
            _accCur = Mathf.Min(accMaxSpread, _accCur + accLosePerRound);
        }
    }
    
}
