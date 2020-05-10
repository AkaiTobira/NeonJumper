using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CollisionDetectorPlayer m_detector;

	public float jumpHeight        = 4;
	public float timeToJumpApex    = .4f;
	float accelerationTimeAirborne = .1f;
	float accelerationTimeGrounded = .05f;
	float moveSpeed = 10;
	float gravity;
	float jumpVelocity;
    float climbVelocity;
	[HideInInspector] public Vector3 velocity;
	float velocityXSmoothing;

    [SerializeField] Animation m_hurtAnimation = null;

	void Start() {
		m_detector  = GetComponent<CollisionDetectorPlayer>();
		gravity       = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity  = Mathf.Abs(gravity) * timeToJumpApex;
        climbVelocity = 5;
	}

    [HideInInspector] public bool jumpPressed = false;
    const int FRAMES_TO_JUMP_EXPIRE = 10;
    int elapsedJumpFrames           = 0;

    // Function improving jump mechanism by
    // checking if player hit the floor in 
    // 10fps since jump key was pressed.
    void HandleJump(){
        if( jumpPressed ){
            elapsedJumpFrames += 1;
            if( m_detector.isOnGround() ){
                jumpPressed       = false;
                JumpAndSavePositionOnPlatform();
            }else if( elapsedJumpFrames > FRAMES_TO_JUMP_EXPIRE ){
                jumpPressed       = false;
            }
        }else if( Input.GetKeyDown (KeyCode.Space) ){
            if( m_detector.isOnGround() ){
                JumpAndSavePositionOnPlatform();
            }else{
                jumpPressed = true;
                elapsedJumpFrames = 0;
            }
        }
    }

    private void JumpAndSavePositionOnPlatform(){
        velocity.y        = jumpVelocity;
        GetComponent<PlayerSaver>().Save();
    }

    public bool isFallKeyPressed(){
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    public bool isJumpingKeyPresed(){
        return (Input.GetKeyDown(KeyCode.Space) || jumpPressed ) && m_detector.isOnGround(); 
    }

    public bool isClimbKeyPressed(){
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }

    void HandleMove(){
        Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, 
                                       targetVelocityX, 
                                       ref velocityXSmoothing, 
                                       (m_detector.isOnGround()) ? accelerationTimeGrounded : 
                                                                     accelerationTimeAirborne);
    }

    public void HurtPlayer(){
        BounceUpFromEnemy();
        GameObject.Find("/SceneController").
        GetComponent<GameFlowController>().ReducePlayerLife();
        m_hurtAnimation.Play("cameraJump");
    }

    void HandleClimbing(){
       if( m_detector.canClimbOnLadder() && isClimbKeyPressed() ){
            velocity.y = climbVelocity ;
        }else{
    		velocity.y += gravity * Time.deltaTime;
        }
    }

    void HandleFallByFloor(){
       if( m_detector.canFallByFloor() && isFallKeyPressed() ){
           m_detector.enableFallForOneWayFloor();
        }
    }

    public void BounceUpFromEnemy(){
        velocity.y = jumpVelocity;
    }

    private void HandleHitSurface(){
		if (m_detector.isOnCelling() || m_detector.isOnGround()) {
			velocity.y = 0;
		}
    }

    public void DeletePlayer(){
        Destroy(gameObject);
    }

	void Update() {
        HandleHitSurface();
		HandleMove();
        HandleFallByFloor();
        HandleJump();
        HandleClimbing();
		m_detector.Move (velocity * Time.deltaTime );
	}
}