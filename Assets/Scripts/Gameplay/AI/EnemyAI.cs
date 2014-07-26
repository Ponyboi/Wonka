using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
	public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.
	
	
	public EnemySight enemySight;                          // Reference to the EnemySight script.
	public EnemyGrab enemyGrab;
	public NavMeshAgent nav;                               // Reference to the nav mesh agent.
	public Transform player;                               // Reference to the player's transform.
	//private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	public LastPlayerSighting lastPlayerSighting;          // Reference to the last global sighting of the player.
	public float chaseTimer;                               // A timer for the chaseWaitTime.
	public float patrolTimer;                              // A timer for the patrolWaitTime.
	public int wayPointIndex;                              // A counter for the way point array.

	//searching
	public bool isSearcher = false;
	public bool isSearching = false;
	public float searchSpeed = 1.0f;
	public float rotationDegree = 3.0f;
	private Quaternion initialRotation;
	public GameObject lookObject;

	//Grabbing
	public GameObject dumpPos;
	
	void Awake ()
	{
		// Setting up the references.
		enemySight = Nozzle.TraverseHierarchy(transform, "Head").GetComponent<EnemySight>();
		enemyGrab = GetComponent<EnemyGrab>();
		nav = GetComponent<NavMeshAgent>();

		if (GameObject.FindWithTag(Tags.player) != null) {
			player = GameObject.FindWithTag(Tags.player).transform;
		}

		//playerHealth = player.GetComponent<PlayerHealth>();
		lastPlayerSighting = GameObject.FindWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
		initialRotation = transform.rotation;
	}
	
	
	void Update ()
	{
		// If the player is in sight and is alive...
		if(enemyGrab.isGrabbing) {

			Grabbing();
		}
		 else if(enemySight.playerInSight)// && playerHealth.health > 0f)
			// ... shoot.
			//Shooting();
			Chasing();
		
		// If the player has been sighted and isn't dead...
		else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition)// && playerHealth.health > 0f)
			// ... chase.
			Chasing();
		
		// Otherwise...
		else if (isSearching)
			Searching();

		else 
			// ... patrol.
			Patrolling();
	}
	
	
	void Searching ()
	{
		// Stop the enemy where it is.
		nav.Stop();
		float rotation = Mathf.Sin(Time.time * searchSpeed) * rotationDegree;
		Quaternion lookRotation = Quaternion.AngleAxis(rotation , transform.up);
		enemySight.transform.rotation = transform.rotation * lookRotation;

	}

	void Grabbing() {
		//if (enemyGrab.isGrabbing) {
			nav.destination = dumpPos.transform.position;
			nav.speed = patrolSpeed;
		//}
	}
	
	void Chasing ()
	{
		if (isSearcher) {
			isSearching = false;
		}
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		
		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f){
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			Vector3 targetLoc = player.transform.position;// + (player.transform.forward * 3);
			nav.destination = targetLoc;
		}
			//nav.destination = enemySight.personalLastSighting;
		
		// Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;
		
		// If near the last personal sighting...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;
			
			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;
	}
	
	
	void Patrolling ()
	{
		// Set an appropriate speed for the NavMeshAgent.
		nav.speed = patrolSpeed;
		
		// If near the next waypoint or there is no destination...
		if(nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
		{

			// ... increment the timer.
			patrolTimer += Time.deltaTime;
			
			// If the timer exceeds the wait time...
			if(patrolTimer >= patrolWaitTime)
			{
				// ... increment the wayPointIndex.
				if(wayPointIndex == patrolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;
				
				// Reset the timer.
				patrolTimer = 0;

				if(isSearcher) {
					isSearching = true;
					//transform.rotation = Quaternion.LookRotation(lookObject.transform.position);
					transform.LookAt(new Vector3(lookObject.transform.position.x, transform.position.y, lookObject.transform.position.z));
				}
			}
		}
		else
			// If not near a destination, reset the timer.
			patrolTimer = 0;
		
		// Set the destination to the patrolWayPoint.
		nav.destination = patrolWayPoints[wayPointIndex].position;
	}
}