﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.


	public float moveForce = 150f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 4f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private BoxCollider2D groundCheck;			// A position marking where to check if the player is grounded.
	private BoxCollider2D leftCheck;			// A position marking if the player is running into a wall on the left
	private BoxCollider2D rightCheck;			// A position marking if the player is running into a wall on the right
	private bool touchingLeftWall = false;	// Whether or not the player is running into a wall on the left side
	private bool touchingRightWall = false;	// Whether or not the player is running into a wall on the right side
	private bool grounded = false;			// Whether or not the player is grounded.


	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck").GetComponent<BoxCollider2D>();
		leftCheck = transform.Find ("leftCheck").GetComponent<BoxCollider2D>();
		rightCheck = transform.Find ("rightCheck").GetComponent<BoxCollider2D>();
	}


	void Update()
	{
		// Check if ground collisder is touching the ground
		grounded = groundCheck.IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));

		// Check if the side colliders are thouching any walls
		touchingLeftWall = leftCheck.IsTouchingLayers( 1 << LayerMask.NameToLayer("Ground"));
		touchingRightWall = rightCheck.IsTouchingLayers ( 1<< LayerMask.NameToLayer ("Ground"));


		// If the jump button is pressed and the player is grounded then the player should jump.
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}
	}


	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
//		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if (h * GetComponent<Rigidbody2D> ().velocity.x < maxSpeed) {
			// ... add a force to the player.
			//Make sure the player can't get stuck to a wall
			if ((h < 0 && !touchingLeftWall) || (h > 0 && !touchingRightWall)) {
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
			}
		}
						
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// If the input is moving the player right and the player is facing left...
//		if(h > 0 && !facingRight)
//			// ... flip the player.
//			Flip();
//		// Otherwise if the input is moving the player left and the player is facing right...
//		else if(h < 0 && facingRight)
//			// ... flip the player.
//			Flip();

		// If the player should jump...
		if(jump)
		{
			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
}
