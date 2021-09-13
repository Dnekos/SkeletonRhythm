using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonMover : MonoBehaviour
{
    [SerializeField]
    bool gravity;

    [SerializeField]
    Transform Lefty, Righty, hip, RightBone, LeftBone,Spine;

    Vector3 LOrig, ROrig, L_motion, R_motion;

    PlayerInput input;

    [SerializeField]
    float Fallspeed = 9.8f, speed = 1, StopDistance;

    [SerializeField]
    float LeanAngle = 5, LeanSpeed = 4;

    [SerializeField]
    float CrouchDifference = 1, CrouchSpeedMod = 0.5f;
    float crouchheight, normalheight;
    bool moving;
    [SerializeField]
    float movespeed;

    // Start is called before the first frame update
    void Start()
    {
        LOrig = Lefty.localPosition;
        ROrig = Righty.localPosition;
        input = GetComponent<PlayerInput>();
        normalheight = transform.position.y;
        crouchheight = (transform.position - Vector3.up * CrouchDifference).y;
    }

    public void OnLeft(InputAction.CallbackContext value)
    {
        L_motion = value.ReadValue<Vector2>();
    }

    public void OnRight(InputAction.CallbackContext value)
    {
        R_motion = value.ReadValue<Vector2>();
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() != 0)
            moving = true;
        else
            moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (R_motion != Vector3.zero)
        {
            if (Lefty.GetComponent<Rigidbody>().velocity.magnitude > 0)
                Lefty.position = LeftBone.position;
            Lefty.localPosition = Vector3.Lerp(Lefty.localPosition, LOrig + new Vector3(-R_motion.x, R_motion.y), Time.deltaTime * speed);
            Lefty.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (L_motion != Vector3.zero)
        {
            if (Righty.GetComponent<Rigidbody>().velocity.magnitude > 0)
                Righty.position = RightBone.position;
            Righty.localPosition = Vector3.Lerp(Righty.localPosition, ROrig + new Vector3(-L_motion.x, L_motion.y), Time.deltaTime * speed);
            Righty.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (moving)
        {
            float horz = (R_motion + L_motion).x;
            transform.position += Vector3.right * horz * movespeed * Time.deltaTime;
            Spine.rotation = Quaternion.Lerp(Spine.rotation,
                Quaternion.Euler(Spine.rotation.eulerAngles.x, Spine.rotation.eulerAngles.y, LeanAngle * horz),
                LeanSpeed * Time.deltaTime);

            float vert = -(R_motion + L_motion).y;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, normalheight - 0.5f * CrouchDifference + 0.5f *(CrouchDifference * -Mathf.Sign(vert)), Mathf.Abs(vert) * Time.deltaTime * 2), transform.position.z);
            
        }
    }

    //void FallingMotion(Transform IK)
    //{

    //}
}
