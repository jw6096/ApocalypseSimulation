using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float radius = 1;
    public GameObject target;
    public float maxSpeed;
    public float mass;
    Vector3 acceleration;
    public Vector3 velocity;
    Vector3 position;
    Vector3 steer;
    private string currentInput;

    // Use this for initialization
    void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        position = this.transform.position;
        //currentInput = "SEEK";
    }

    // Update is called once per frame
    void Update()
    {
        // Last thing: actually move
        Movement();

		this.transform.rotation = Quaternion.LookRotation (this.velocity);
    }

    /// <summary>
    /// Apply a force to this object this frame, taking
    /// into account its mass
    /// </summary>
    /// <param name="force">The overall force vector</param>
    public void ApplyForce(Vector3 force)
    {
        this.acceleration += force / this.mass;
    }

    /// <summary>
    /// Internally handles the overall movement based
    /// on acceleration, velocity, etc.
    /// </summary>
    public void Movement()
    {
        // Apply acceleration
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;

        // Throw the position back to Unity
        this.transform.position = position;

        // We're done with forces for this frame
        acceleration = Vector3.zero;
    }



    /// <summary>
    /// Calculate a steering force such that it
    /// allows us to seek a target position
    /// </summary>
    /// <param name="targetPosition">Where to seek</param>
    /// <returns>The steering force to get to the target</returns>
    public Vector3 Seek(Vector3 targetPosition)
    {
        // Calculate our "perfect" desired velocity
        Vector3 desiredVelocity = targetPosition - this.transform.position;

        // Limit desired velocity by max speed
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // How do we turn to start moving towards
        // the desired velocity?
        Vector3 steeringForce = desiredVelocity - this.velocity;
        return steeringForce;
    }

    public Vector3 Flee(Vector3 targetPosition)
    {
        // Calculate our "imperfect" desired velocity
        Vector3 desiredVelocity = -targetPosition + this.transform.position;

        // Limit desired velocity by max speed
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // How do we turn to start moving towards
        // the desired velocity?
        Vector3 steeringForce = desiredVelocity - this.velocity;
        return steeringForce;
    }

    public Vector3 AvoidObstacle(obstacleScript script, float safeDistance)
    {
        Vector3 vecToObj = script.transform.position - this.transform.position;

        float projectedDist = Vector3.Dot(vecToObj, this.transform.right);

		if (vecToObj.magnitude < safeDistance && (Vector3.Dot(vecToObj, this.transform.forward) > 0) && Mathf.Abs(projectedDist) < (script.radius + this.radius))
        {
			float weight = safeDistance / vecToObj.magnitude;

            if (projectedDist < 0)      //turn left
                return Seek(this.transform.right * 1500 * weight);
            else                        //turn right
				return Seek(-this.transform.right * 1500 * weight);
        }
        else
            return Vector3.zero;
    }

    public Vector3 Pursue(Vector3 pos, Vector3 vel)
    {
        Vector3 futurePos = pos + vel * Time.deltaTime;

        return Seek(futurePos);
    }

    public Vector3 Evade(Vector3 pos, Vector3 vel)
    {
        Vector3 futurePos = pos + vel * Time.deltaTime;

        return Flee(futurePos);
    }
}
