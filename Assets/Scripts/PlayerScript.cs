using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;

    private Rigidbody2D player;
    //private GameObject player;
    private Vector2 movement;
    private Vector2 smoothMovement;
    private Vector2 smoothInputVelocity;

    void Awake() 
    {
        player = GetComponent<Rigidbody2D>();

        //player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        smoothMovement = Vector2.SmoothDamp(smoothMovement, movement, ref smoothInputVelocity, 0.1f);

        player.velocity = smoothMovement * moveSpeed;
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
