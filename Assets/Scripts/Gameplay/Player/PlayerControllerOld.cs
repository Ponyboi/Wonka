/**
 * Team Spicy Bison
 * Members: Aidan Arrowood, Yan Zhang, Drew Shneider
 * Script found online, modified form unitys stadard asset and then modified by Aidan
**/


using UnityEngine;
using System.Collections;
using System;
//[AddComponentMenu("Rayco's scripts/third person controller")]
public class PlayerControllerOld : MonoBehaviour {
	public int id;
	public int idSelect;
	public GameController gameController;
	public CameraController cameraController;
	public Transform cameraMain;
	CharacterController controller;

	//Grabbing
	public bool isGrabbed = false;

	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;
	
	public float walkMaxAnimationSpeed = 0.75F;
	public float trotMaxAnimationSpeed = 1F;
	public float runMaxAnimationSpeed = 1F;
	public float jumpAnimationSpeed = 1F;
	public float landAnimationSpeed =1F;

	// Moving platform support
	private Transform activePlatform;
	private Vector3 activeLocalPlatformPoint;
	private Vector3 activeGlobalPlatformPoint;
	private Vector3 lastPlatformVelocity;
	
	// If you want to support moving platform rotation as well:
	private Quaternion activeLocalPlatformRotation;
	private Quaternion activeGlobalPlatformRotation;

	private float candyInterval = 1f;
	private float candyTime = 0.0f;

	private float speedModifier = 1.0f;
	
	private Animation _animation;
	
	enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,   
	}
	private CharacterState _characterState;
	// The speed when walking
	public float walkSpeed = 5.0F;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed = 6.0F;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 12.0F;
	
	public float inAirControlAcceleration = 18.0F;
	
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight = 4.5F;
	
	// The gravity for the character
	public float gravity = 55.0F;
	// The gravity in controlled descent mode
	public float speedSmoothing = 10.0F;
	public float rotateSpeed = 2000.0F;
	public float trotAfterSeconds = 3.0F;
	
	public bool canJump= true;
	
	private float jumpRepeatTime = 0.05F;
	private float jumpTimeout = 0.15F;
	private float groundedTimeout = 0.25F;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0F;
	
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	public float verticalSpeed = 0.0F;
	// The current x-z move speed
	private float moveSpeed = 0.0F;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags ;
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping = false;
	private bool jumpingReachedApex = false;
	public float bounceSpeed = 10;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	private bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart = 0.0F;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime = -10.0F;
	// Last time we performed a jump
	private float lastJumpTime = -1.0F;
	
	
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	private float lastJumpStartHeight = 0.0F;
	
	
	private Vector3 inAirVelocity = Vector3.zero;
	
	private float lastGroundedTime = 0.0F;
	
	
	private bool isControllable = true;
	
	// Use this for initialization
	void  Awake (){
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
		cameraController = GameObject.Find("_CameraController").GetComponent<CameraController>();
		controller = GetComponent<CharacterController>();

		moveDirection = transform.TransformDirection(Vector3.forward);
		
		_animation = GetComponent<Animation>();
		if(!_animation)
			Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		
		/*
public AnimationClip idleAnimation;
public AnimationClip walkAnimation;
public AnimationClip runAnimation;
public AnimationClip jumpPoseAnimation;
    */
		if(!idleAnimation) {
			_animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if(!walkAnimation) {
			_animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if(!runAnimation) {
			_animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if(!jumpPoseAnimation  && canJump) {
			_animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
		
	}
	void  UpdateSmoothedMovementDirection (){
		Transform cameraTransform = cameraController.GetCurrentCamera().transform;
		bool grounded = IsGrounded();
		
		// Forward vector relative to the camera along the x-z plane   
		Vector3 forward= cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);

		float v= 0;
		float h= 0;
		if (gameController.getIdSelect() == id) {
			v = Input.GetAxisRaw("Vertical");
			h = Input.GetAxisRaw("Horizontal");
		} else {

		}
		
		// Are we moving backwards or looking backwards
		if (v < -0.2f)
			movingBack = true;
		else
			movingBack = false;
		
		bool wasMoving= isMoving;
		isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;
		
		// Target direction relative to the camera
		Vector3 targetDirection= h * right + v * forward;
		
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving  standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
			
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9f  && grounded)
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			float curSmooth= speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed= Mathf.Min(targetDirection.magnitude, 1.0f);
			
			_characterState = CharacterState.Idle;
			
			// Pick speed modifier
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift) || 0.7f < Input.GetAxisRaw("LeftTrigger"))
			{
//				if (speedModifier < runSpeed) {
//					speedModifier += speedModifier * 0.1f;
//					targetSpeed *= speedModifier;
//
//				} else {
					targetSpeed *= runSpeed;
//				}
				_characterState = CharacterState.Running;
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				_characterState = CharacterState.Trotting;
			}
			else
			{
				targetSpeed *= walkSpeed;
				_characterState = CharacterState.Walking;
			}

			if (targetSpeed == 0) {
				if (speedModifier > 1) {
					speedModifier -= speedModifier * 0.1f;
				}
			}
			
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
			
			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;
		}
		// In air controls
		else
		{
			// Lock camera while in air
			if (jumping)
				lockCameraTimer = 0.0f;
			
			if (isMoving) {
				inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
				inAirVelocity += lastPlatformVelocity;
			}
		}
		
		
		
	}
	void  ApplyJumping (){
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
		
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing    
			if (canJump  && Time.time < lastJumpButtonTime + jumpTimeout) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	void  ApplyGravity (){
		if (isControllable) // don't move player at all if not controllable.
		{
			// Apply gravity
			bool jumpButton = false;
			jumpButton= Input.GetButton("Jump");
			
			// When we reach the apex of the jump we send out a message
			if (jumping  && !jumpingReachedApex  && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
			
			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}
	public float  CalculateJumpVerticalSpeed ( float targetJumpHeight  ){
		// From the jump height and gravity we deduce the upwards speed
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight*gravity);
	}  
	void  DidJump (){
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpStartHeight = transform.position.y;
		lastJumpButtonTime = -10;
		
		_characterState = CharacterState.Jumping;
	}
	void  Update (){
		
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}

		if (gameController.getIdSelect() == id) {
			if (Input.GetButtonDown ("Jump") ||  Input.GetButtonDown("PlayerJump"))
			{
				lastJumpButtonTime = Time.time;
			}
		}
		
		UpdateSmoothedMovementDirection();
		
		// Apply gravity
		// - extra power jump modifies gravity
		// - controlledDescent mode modifies gravity
		ApplyGravity ();
		
		// Apply jumping logic
		ApplyJumping ();


		
		// Calculate actual motion
		//Debug.Log("IsGrounded(): " + IsGrounded());
		//Debug.Log("verticalSpeed: " + verticalSpeed);
		Vector3 movement= moveDirection * moveSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
		//Debug.Log("movement: " + movement);
		movement *= Time.deltaTime;

	
		// Moving platform support
		if (activePlatform != null) {
			var newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
			var moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
			if (moveDistance != Vector3.zero)
				controller.Move(moveDistance);
			lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
			
			// If you want to support moving platform rotation as well:
			var newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;
			var rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);
			
			// Prevent rotation of the local up vector
			rotationDiff = Quaternion.FromToRotation(rotationDiff * transform.up, transform.up) * rotationDiff;
			
			transform.rotation = rotationDiff * transform.rotation;
		}
		else {
			lastPlatformVelocity = Vector3.zero;
		}
		
		activePlatform = null;

		if (activePlatform == null) {
			controller.SimpleMove(Vector3.zero);
		}
		// Move the controller
		//controller = GetComponent<CharacterController>();
		collisionFlags = controller.Move(movement);



		if (activePlatform != null) {
			activeGlobalPlatformPoint = transform.position;
			activeLocalPlatformPoint = activePlatform.InverseTransformPoint (transform.position);
			
			// If you want to support moving platform rotation as well:
			activeGlobalPlatformRotation = transform.rotation;
			activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation; 
		}

		
		// ANIMATION sector
		if(_animation) {
			if(_characterState == CharacterState.Jumping)
			{
				if(!jumpingReachedApex) {
					_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
					_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(jumpPoseAnimation.name);
				} else {
					_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
					_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(jumpPoseAnimation.name);              
				}
			}
			else
			{
				if(controller.velocity.sqrMagnitude < 0.1f) {
					_animation.CrossFade(idleAnimation.name);
				}
				else
				{
					if(_characterState == CharacterState.Running) {
						_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
						_animation.CrossFade(runAnimation.name);   
					}
					else if(_characterState == CharacterState.Trotting) {
						_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
						_animation.CrossFade(walkAnimation.name);  
					}
					else if(_characterState == CharacterState.Walking) {
						_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
						_animation.CrossFade(walkAnimation.name);  
					}
					
				}
			}
		}
		// ANIMATION sector
		
		// Set rotation to the move direction
		if (IsGrounded())
		{
			
			transform.rotation = Quaternion.LookRotation(moveDirection);
			
		}  
		else
		{
			Vector3 xzMove= movement;
			xzMove.y = 0;
			if (xzMove.sqrMagnitude > 0.001f)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}
		}  
		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

//	void OnCollisionEnter(Collision hit) {
//		if (hit.gameObject.layer == 9) {
//			activePlatform = hit.collider.transform;    
//		}
//	}

	void  OnControllerColliderHit (ControllerColliderHit hit){
		//  Debug.DrawRay(hit.point, hit.normal);

		if (hit.moveDirection.y < -0.8 && hit.normal.y > 0.5 && hit.gameObject.layer == 9) {
			activePlatform = hit.collider.transform;    
		}
		if (hit.moveDirection.y > 0.01f)
			return;

		if (id == 2 && hit.gameObject.tag == "DoorHinge") {
			//Rigidbody body = null;
			Rigidbody body = hit.gameObject.GetComponent<Rigidbody>();
			if (body != null) {
				body.AddForce(moveDirection * 50);
			}
		}

		if (hit.gameObject.tag == "Candy" && candyTime < Time.time) {
			Candy candy = (Candy)hit.gameObject.GetComponent<Candy>();
			candy.PlayAudio();
			candyTime = Time.time + candyInterval;
		}
	}  
	
	float  GetSpeed (){
		return moveSpeed;
	}
	
	bool  IsJumping (){
		return jumping;
	}
	
	public bool  IsGrounded (){
		//return (CollisionFlags.Below) != 0;
		return controller.isGrounded;
	}
	
	Vector3  GetDirection (){
		return moveDirection;
	}
	
	bool  IsMovingBackwards (){
		return movingBack;
	}
	
	float  GetLockCameraTimer (){
		return lockCameraTimer;
	}
	
	bool IsMoving (){
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}
	
	bool  HasJumpReachedApex (){
		return jumpingReachedApex;
	}
	
	bool  IsGroundedWithTimeout (){
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	void  Reset (){
		gameObject.tag = "Player";
	}

	public float GetBounceSpeed(){
		return bounceSpeed;
	}
	
}
