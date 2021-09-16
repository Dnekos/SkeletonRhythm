using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLegStepper : MonoBehaviour
{ // aspects of the code taken from https://www.weaverdev.io/blog/bonehead-procedural-animation

    [SerializeField] SkeletonLegStepper LeftLegStepper;
    [SerializeField] SkeletonLegStepper RightLegStepper;


    // The position and rotation we want to stay in range of
    [SerializeField] Transform homeTransform;
    // Stay within this distance of home
    [SerializeField] float wantStepAtDistance;
    // How long a step takes to complete
    [SerializeField] float moveDuration;

    float homeheight; // keep height standard

    // Is the leg moving?
    public bool Moving;

    void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
        homeheight = homeTransform.position.y;
    }

    private void Update()
    {
        homeTransform.position = new Vector3(homeTransform.position.x, homeheight, homeTransform.position.z);
    }

    // Only allow diagonal leg pairs to step together
    IEnumerator LegUpdateCoroutine()
    {
        // Run continuously
        while (true)
        {
            // Try moving one diagonal pair of legs
            do
            {
                LeftLegStepper.TryMove();
                // Wait a frame
                yield return null;

                // Stay in this loop while either leg is moving.
                // If only one leg in the pair is moving, the calls to TryMove() will let
                // the other leg move if it wants to.
            } while (LeftLegStepper.Moving);

            // Do the same thing for the other diagonal pair
            do
            {
                RightLegStepper.TryMove();
                yield return null;
            } while (RightLegStepper.Moving);
        }
    }


    // Coroutines must return an IEnumerator
    IEnumerator MoveToHome()
    {
        // Indicate we're moving (used later)
        Moving = true;

        // Store the initial conditions
        Quaternion startRot = transform.rotation;
        Vector3 startPoint = transform.position;

        Quaternion endRot = homeTransform.rotation;
        Vector3 endPoint = homeTransform.position;

        // Time since step started
        float timeElapsed = 0;

        // Here we use a do-while loop so the normalized time goes past 1.0 on the last iteration,
        // placing us at the end position before ending.
        do
        {
            // Add time since last frame to the time elapsed
            timeElapsed += Time.deltaTime;

            float normalizedTime = timeElapsed / moveDuration;

            // Interpolate position and rotation
            transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime);
            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            // Wait for one frame
            yield return null;
        }
        while (timeElapsed < moveDuration);

        // Done moving
        Moving = false;
    }

    // Was previously void Update()
    public void TryMove()
    {
        if (Moving) return;

        float distFromHome = Vector3.Distance(transform.position, homeTransform.position);

        // If we are too far off in position or rotation
        if (distFromHome > wantStepAtDistance)
        {
            StartCoroutine(Move());
        }
    }


    // New variable!
    // Fraction of the max distance from home we want to overshoot by
    [SerializeField] float stepOvershootFraction;

    IEnumerator Move()
    {
        Moving = true;

        Vector3 startPoint = transform.position;
        Quaternion startRot = transform.rotation;

        Quaternion endRot = homeTransform.rotation;

        // Directional vector from the foot to the home position
        Vector3 towardHome = (homeTransform.position - transform.position);
        // Total distnace to overshoot by   
        float overshootDistance = wantStepAtDistance * stepOvershootFraction;
        Vector3 overshootVector = towardHome * overshootDistance;
        // Since we don't ground the point in this simplified implementation,
        // we restrict the overshoot vector to be level with the ground
        // by projecting it on the world XZ plane.
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

        // Apply the overshoot
        Vector3 endPoint = homeTransform.position + overshootVector;

        // We want to pass through the center point
        Vector3 centerPoint = (startPoint + endPoint) / 2;
        // But also lift off, so we move it up by half the step distance (arbitrarily)
        centerPoint += homeTransform.up * Vector3.Distance(startPoint, endPoint) / 2f;

        float timeElapsed = 0;
        do
        {
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / moveDuration;
            //normalizedTime = Easing.Cubic.InOut(normalizedTime);

            // Quadratic bezier curve
            transform.position =
              Vector3.Lerp(
                Vector3.Lerp(startPoint, centerPoint, normalizedTime),
                Vector3.Lerp(centerPoint, endPoint, normalizedTime),
                normalizedTime
              );

            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }
        while (timeElapsed < moveDuration);

        Moving = false;
    }
}
