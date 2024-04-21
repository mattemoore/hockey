using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputActionAsset actions;
    public Vector2 movementSpeed = new Vector2(100.0f, 100.0f);
    public int shotPower = 1000;

    private GameController gameController;
    private Rigidbody2D rigidBody;
    private Vector2 playerMoveVector = Vector2.zero;
    private bool isHoldingShoot;
    private bool isWindingUp;
    private float windingUpTimeElapsed;
    private InputActionMap gameplayActions;
    private InputAction moveAction, fireAction, aimAction;
    private bool hasPuck;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        gameplayActions = actions.FindActionMap("Gameplay");
        moveAction = gameplayActions.FindAction("PlayerMove");
        fireAction = gameplayActions.FindAction("PlayerFire");
        aimAction = gameplayActions.FindAction("PlayerAim");
    }

    private void OnDestroy()
    {
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        // If game paused skip Update()
        //if (gameController.isPaused || gameController.isGameOver) return;

        UpdateShooting();
        //UpdateAiming();
        UpdateMoving();
    }
    private void FixedUpdate()
    {
        //if (gameController.isPaused || gameController.isGameOver) { return; }
        rigidBody.AddForce(playerMoveVector * 200);
        if (hasPuck)
        {
            GameObject.FindGameObjectWithTag("Puck").transform.position = this.transform.position;
        }
    }

    private void UpdateShooting()
    {
        // Not using callback as we want to support holding down the fire button
        isHoldingShoot = fireAction.ReadValue<float>() > 0;
       
        if (isHoldingShoot)
        {
            // Start winding up for shot or a hit
            if (!isWindingUp)
            {
                isWindingUp = true;
                windingUpTimeElapsed = 0;
            }
            else
            {
                windingUpTimeElapsed += Time.deltaTime;
            }
        }
        else
        {
            // Release shot
            if (hasPuck && isWindingUp)
            {
                ShootPuck(Vector2.up, windingUpTimeElapsed);
            } 
            // else release hit
        }

        
    }

    private void ShootPuck(Vector2 shotDirection, float windUpTime)
    {
        isWindingUp = false;
        hasPuck = false;
        
        GameObject.FindGameObjectWithTag("Puck").GetComponent<Rigidbody2D>().AddForce(shotDirection * 1000 * windUpTime);
    }

    private void UpdateMoving()
    {
        playerMoveVector = moveAction.ReadValue<Vector2>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Puck")
        {
            hasPuck = true;
        }
    }
}