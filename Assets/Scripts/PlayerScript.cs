using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public PlayerWeapon weapon;
    public float moveSpeed = 5f;
    
    private Camera _camera;

    private bool _moving = false;
    private Vector2 movement;
    
    // We need a seperate player object to hold our rotation else the camera will spin with us
    private GameObject _player;
    
    private Vector2 _direction = Vector2.zero;

    private GameObject _staticContainer;
    
    // UI objects
    private TMPro.TMP_Text _uiTimer;
    private TMPro.TMP_Text _uiGunName;
    private TMPro.TMP_Text _uiCurrentAmmo;
    private TMPro.TMP_Text _uiMaxAmmo;
    private GameObject _uiAmmoBox;

    private float _score_time = 0f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GameObject uiBox = GameObject.FindGameObjectWithTag("ui");
        
        _uiTimer = uiBox.transform.Find("time").GetComponent<TMPro.TMP_Text>();
        _uiGunName = uiBox.transform.Find("name").GetComponent<TMPro.TMP_Text>();
        _uiCurrentAmmo = uiBox.transform.Find("remain").GetComponent<TMPro.TMP_Text>();
        _uiMaxAmmo = uiBox.transform.Find("max").GetComponent<TMPro.TMP_Text>();
        _uiAmmoBox = uiBox.transform.Find("ammo").gameObject;
        
        _camera = Camera.main;
        _player = gameObject.transform.Find("Player").gameObject;
        
        Sprite sprite = _player.GetComponent<SpriteRenderer>().sprite;
        
        gameObject.GetComponent<CircleCollider2D>().radius = 
            Mathf.Min(sprite.texture.width, sprite.texture.height) / sprite.pixelsPerUnit / 4;

        if (weapon != null) {
            weapon.transform.parent = _player.transform;
            weapon.transform.position = Vector3.zero;
            weapon.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        _staticContainer = GameObject.FindGameObjectWithTag("root");
    }

    // Update is called once per frame
    void Update() {
        // Rotation
        Vector2 pointer = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 position = transform.position;

        float rot = Mathf.Atan2(position.x - pointer.x,pointer.y - position.y);
        _player.transform.rotation = Quaternion.Euler(0f, 0f, rot * Mathf.Rad2Deg);
        
        // Movement
        if (_moving) {
            position.x += _direction.x * Time.deltaTime * moveSpeed;
            position.y += _direction.y * Time.deltaTime * moveSpeed;
            
            transform.position = position;
        }

        // Move the camera to be a quarter of the way between the player and the camera
        Vector3 temp = Vector2.Lerp(transform.position, pointer, 0.25f);
        temp.z = -10;
        _camera.transform.position = temp;
    }

    private float _lastPickup = 0f;
    private float _pickupDelay = 3f;
     
    private GameObject _lastGun;

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("gun_pickup")) {
            // If the pickup delay has not expired, check if gun is same as last gun and return if it is
            if (Time.time < _lastPickup + _pickupDelay && _lastGun == other.gameObject) return;
            
            // Otherwise, reset pickup timer, update last gun, and continue
            _lastPickup = Time.time;
            _lastGun = (weapon != null) ? weapon.gameObject : null;

            bool isFiring;
            
            if (weapon != null) {
                weapon.transform.SetParent(_staticContainer.transform, true);
                _lastGun = weapon.gameObject;
                isFiring = weapon.firing;
                weapon.firing = false;
                weapon.GetComponent<PlayerWeapon>().enabled = false;
                weapon = null;
            }
            else {
                isFiring = false;
            }
            weapon = other.gameObject.GetComponent<PlayerWeapon>();
            weapon.Attach(_player);
            weapon.firing = isFiring;
        };
    }

    public void OnMove(InputAction.CallbackContext context) {
        _direction = context.ReadValue<Vector2>();
        _moving = _direction != Vector2.zero;
    }

    public void OnFire(InputAction.CallbackContext context) {
        if (weapon == null) return;
        weapon.firing = context.ReadValue<float>() != 0;
    }

    private void OnMove(InputValue inputVal)
    {
        movement = inputVal.Get<Vector2>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    float sideToSide = Input.GetAxis("Horizontal");
    //    float upDown = Input.GetAxis("Vertical");

    //    Vector3 moveVector = new Vector3(sideToSide, upDown, 0);
    //    moveVector = moveVector.normalized * moveSpeed * Time.deltaTime;

    //    player.transform.position += moveVector;
    //player.MovePosition(player.transform.position + moveVector);
    //}

}
