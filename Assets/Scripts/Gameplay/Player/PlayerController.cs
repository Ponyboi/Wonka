using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// Main Components
	public int id;
	public int idSelect;
	public GameController gameController;
	public CameraController cameraController;
	public Transform cameraMain;
	CharacterController controller;
	public bool isGrabbed = false;

	public AudioClip shoutingClip;      // Audio clip of the player shouting.
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.1f;  // The damping for the speed parameter

	private HashIDs hash;               // Reference to the HashIDs.
	private Animator anim;              // Reference to the animator component.
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	static int jumpState = Animator.StringToHash("Base Layer.Jump");

	// Movement
	private Vector3 targetDirection;
	private Vector3 airDirection;
	public float playerSpeed = 5f;
	private Vector3 lastPos;
	public float speedMod = 1;

	// Moving platform support
	public Transform activePlatform;
	public Transform lastPlatform;
	private Vector3 activeLocalPlatformPoint;
	private Vector3 activeGlobalPlatformPoint;
	private Vector3 lastPlatformVelocity;

	// Moving platform rotation
	private Quaternion activeLocalPlatformRotation;
	private Quaternion activeGlobalPlatformRotation;

	private float candyInterval = 1f;
	private float candyTime = 0.0f;
	
	private float speedModifier = 1.0f;

	//Jumping
	public bool jumpButton = false;
	public bool isJumping = false;
	public bool jumpStart = false;
	private bool vertSpeedSet = false;
	private float jumpMaxHold = 0.35f;
	public bool isGravity = true;

	public float inAirControlAcceleration = 18.0F;

	public bool isDrowning = false;

	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight = 4.5F;
	
	// The gravity for the character
	public float gravity = 55.0F;
	// The gravity in controlled descent mode
	public float speedSmoothing = 10.0F;
	public float rotateSpeed = 2000.0F;

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
	private float moveSpeed = 5F;
	
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

	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		gameController = GameObject.Find("_GameController").GetComponent<GameController>();
		cameraController = GameObject.Find("_CameraController").GetComponent<CameraController>();
		controller = GetComponent<CharacterController>();
		//anim = GetComponent<Animator>();
		// Set the weight of the shouting layer to 1.
//		anim.SetLayerWeight(1, 1f);
	}

	void FixedUpdate ()
	{
		//Animation
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation

		Grabbing();

//		bool sneak = false; //Input.GetButton("Sneak");
		// isGravity is used when player is in river to prevent simpleMove() as it uses gravity
		if (isGravity) {
			PlatformMovement();
		}
		MovementManagement();

		Jumping();

		Drowning();
	}
	


	void Update ()
	{
		// Cache the attention attracting input.
//		bool shout = Input.GetButtonDown("Attract");
		
		// Set the animator shouting parameter.
//		anim.SetBool(hash.shoutingBool, shout);
		
//		AudioManagement(shout);
	}
	
	
	void MovementManagement () //old params (float horizontal, float vertical, bool sneaking)
	{
		// Set the sneaking parameter to the sneak input.
//		anim.SetBool(hash.sneakingBool, sneaking);

		//Camera Control Orientation
		Transform cameraTransform = cameraController.GetCurrentCamera().transform;
		//bool grounded = IsGrounded();
		
		// Forward vector relative to the camera along the x-z plane   
		Vector3 forward= cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);
		right = right.normalized;
		
		float v= 0;
		float h= 0;
		if (id == 1) {
			v = Input.GetAxisRaw("Vertical");
			h = Input.GetAxisRaw("Horizontal");
		} else {
			v = Input.GetAxisRaw("Vertical2");
			h = Input.GetAxisRaw("Horizontal2");
			if (Input.GetAxisRaw("Vertical2") == 0 && Input.GetAxisRaw("Horizontal2") == 0) {
				if (Mathf.Abs(Input.GetAxisRaw("RightAnalog_V")) > 0.8f) {
					v = -Input.GetAxisRaw("RightAnalog_V");
				}
				if (Mathf.Abs(Input.GetAxisRaw("RightAnalog_H")) > 0.8f) {
					h = Input.GetAxisRaw("RightAnalog_H");
				}
			}
		}
		
		// Target direction relative to the camera
		targetDirection= h * right + v * forward;
		if (IsGrounded()){
			airDirection = targetDirection;
			inAirVelocity = Vector3.zero;
		}
		//playerSpeed = (transform.position - lastPos).magnitude;
		float horizontal = targetDirection.x;
		float vertical = targetDirection.z;
		
		// If there is some axis input...
		if(horizontal != 0f || vertical != 0f)
		{
			// ... set the players rotation and set the speed parameter to 5.5f.
			Vector2 moveDir = new Vector2(horizontal, vertical);
			moveDir = moveDir * moveSpeed * speedMod;
			//moveDir *= moveDir.magnitude;
			Rotating(horizontal, vertical);
			anim.SetFloat(hash.speedFloat, moveDir.magnitude, speedDampTime, Time.deltaTime);
		}
		else
			// Otherwise set the speed parameter to 0.
			anim.SetFloat(hash.speedFloat, 0);
		lastPos = transform.position;
	}
	
	
	void Rotating (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		//Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		//rigidbody.MoveRotation(newRotation);
		transform.rotation = newRotation;
	}

	void ApplyGravity() {
		if (IsGrounded()) {
			verticalSpeed = 0.0f;
		} else {
			if (isGravity) {
				verticalSpeed -= gravity * Time.deltaTime;
			}
			//Debug.Log("gravity");
		}
	}

	void Jumping(){
		//Debug.Log("isGrounded: " + IsGrounded() + " " + verticalSpeed);
		// First check if jump is pushed
		if (IsGrounded()) {
			if (id == 1){
				if (Input.GetButtonDown("Jump")) {
					lastJumpButtonTime = Time.time;
					vertSpeedSet = true;
				}
			} else {
				if (Input.GetButtonDown("Jump2")) {
					lastJumpButtonTime = Time.time;
					vertSpeedSet = true;
				}
			}
			verticalSpeed = 0.0f;
		} else {
		//variable height
			if(Input.GetButton("Jump") && Time.time < lastJumpButtonTime + jumpMaxHold){
				verticalSpeed += 0.8f;
			}
			AirMovement();
			if (isGravity) {
				verticalSpeed -= gravity * Time.deltaTime;
			}
			//Debug.Log("gravity");
		}
		if (Time.time < lastJumpButtonTime + jumpTimeout) {
			anim.SetBool(hash.isJumping, true);
			isJumping = true;
		}
		// Only apply jumping if anims state starts
		if (currentBaseState.nameHash == jumpState) {
			anim.SetBool(hash.isJumping, false);
			jumpStart = true;
			if (vertSpeedSet) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				vertSpeedSet = false;
			Debug.Log("VERT SPEED");
			}
			//isJumping = true
		}
		if (currentBaseState.nameHash != jumpState && jumpStart && IsGrounded()) {
			isJumping = false;
			jumpStart = false;
		}
		if (isJumping) {
			anim.applyRootMotion = false;
			if (verticalSpeed <= 0.0f) {
				//falling animation
			}
			if (IsGrounded()) {
				AirMovement();
			}
		
		}else {
			anim.applyRootMotion = true;
		}
		anim.SetBool(hash.isGrounded, IsGrounded());
		anim.SetFloat(hash.verticalSpeed, verticalSpeed);
	}

	public float  CalculateJumpVerticalSpeed ( float targetJumpHeight  ){
		// From the jump height and gravity we deduce the upwards speed
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight*gravity);
	} 

	public void AirMovement() {
		inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
		inAirVelocity += lastPlatformVelocity;
		
		Vector3 movement= airDirection.normalized * (playerSpeed * airDirection.magnitude) + new Vector3 (0, verticalSpeed, 0) + inAirVelocity; //moveSpeed
		//Debug.Log("movement: " + movement);
		Vector3 clampedAirAccel = Vector3.ClampMagnitude(movement, 14);
		movement = new Vector3(clampedAirAccel.x, movement.y, clampedAirAccel.z);
		movement *= speedMod;
		movement *= Time.deltaTime;
		
		collisionFlags = controller.Move(movement);
	}

	void Drowning(){
		anim.SetBool(hash.isDrowning, isDrowning);
	}

	void Grabbing() {
		if (isGrabbed) {
	//		rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
		}else{
//			rigidbody.constraints = RigidbodyConstraints.FreezeRotation;;
		}
	}

	void PlatformMovement(){
		// Moving platform support
		if (activePlatform != null && lastPlatform == activePlatform) {
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
			//controller.Move(Vector3.zero);
		}
		// Move the controller
		//controller = GetComponent<CharacterController>();
		//collisionFlags = controller.Move(movement);
		
		
		
		if (activePlatform != null) {
			//Debug.Log("activePlatform not null");
//			if (!IsGrounded()) {
//				activePlatform = null;
//				return;
//			}
			activeGlobalPlatformPoint = transform.position;
			activeLocalPlatformPoint = activePlatform.InverseTransformPoint (transform.position);
			
			// If you want to support moving platform rotation as well:
			activeGlobalPlatformRotation = transform.rotation;
			activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation; 
			lastPlatform = activePlatform;
		}
	}

	void  OnControllerColliderHit (ControllerColliderHit hit){
		//  Debug.DrawRay(hit.point, hit.normal);

		if (hit.gameObject.tag == "Candy"){
			if (hit.moveDirection.y < -0.8 && hit.normal.y > 0.5 && hit.gameObject.layer == 9) {
				activePlatform = hit.collider.transform;   
			}
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
	
	
	void AudioManagement (bool shout)
	{
		// If the player is currently in the run state...
		if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
		{
			// ... and if the footsteps are not playing...
			if(!audio.isPlaying)
				// ... play them.
				audio.Play();
		}
		else
			// Otherwise stop the footsteps.
			audio.Stop();
		
		// If the shout input has been pressed...
		if(shout)
			// ... play the shouting clip where we are.
			AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
	}

	public bool  IsGrounded (){
		//return (CollisionFlags.Below) != 0;
		return controller.isGrounded;
	}

	bool IsMoving (){
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}
}