using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public PlayerWeapon weapon;
    public float moveSpeed = 5f;
    
    private Camera _camera;

    private bool _moving = false;
    
    // We need a seperate player object to hold our rotation else the camera will spin with us
    private GameObject _player;
    
    private Vector2 _direction = Vector2.zero;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _camera = Camera.main;
        _player = gameObject.transform.Find("Player").gameObject;
        
        Sprite sprite = _player.GetComponent<SpriteRenderer>().sprite;
        
        _player.GetComponent<BoxCollider2D>().size = 
            new Vector2(sprite.texture.width, sprite.texture.height) / sprite.pixelsPerUnit;

        if (weapon != null) {
            weapon.transform.parent = _player.transform;
            weapon.transform.position = Vector3.zero;
            weapon.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
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

    public void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("WeaponPickup")) {
            if (weapon != null) weapon.GetComponent<PlayerWeapon>().Attach(gameObject);

            weapon = other.gameObject.GetComponent<PlayerWeapon>();
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
}
