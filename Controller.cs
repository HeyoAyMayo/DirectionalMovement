
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Controller : MonoBehaviour
{
    // A struct for horizontal, vertical, and diagonal positions 
    [Serializable]
    public struct Directions
    {
        public Transform middle;
        public Transform up;
        public Transform down;
        public Transform left;
        public Transform right;

        public Transform upLeft;
        public Transform upRight;
        public Transform downLeft;
        public Transform downRight;
    }
    private PlayerInput input;


    // Some inspector stuff
    [Header("Movement Settings")]
    [Tooltip("The points where the player move to when inputs are made")] // Tooltip to display when hovered
    [SerializeField] // Serialize private field
    private Directions pointLocations;  // All the locations where our player moves to

    [SerializeField] private InputAction movementInput;

    // Both vertical and horizontal directions 
    private float vertical, horizontal; 
    private Vector3 lerpTarget;
    private float lerpT = 0.1f;

    private void Start()
    {
        movementInput.Enable();
    }
    // Start is called before the first frame update
    private void Update()
    {
        if (transform.position != lerpTarget)
        {
            float x = transform.position.x, y = transform.position.y, z = transform.position.z;
            transform.position = new Vector3(Mathf.Lerp(x, lerpTarget.x, lerpT), Mathf.Lerp(y, lerpTarget.y, lerpT), Mathf.Lerp(z, lerpTarget.z, lerpT));
        }
        DirectionalMovement();
        
    }
    private void DirectionalMovement()
    {
        Vector2 m_Move = movementInput.ReadValue<Vector2>();
        horizontal = m_Move.x;
        vertical = m_Move.y;
        if (horizontal == 0 && vertical == 0)
        {
            lerpTarget = pointLocations.middle.position;
        }
        if (Mathf.Abs(horizontal) > 0 && vertical == 0)
        {
            switch (Mathf.RoundToInt(horizontal))
            {
                default:
                    break;
                case 1:
                    lerpTarget = pointLocations.left.position;
                    break;
                case -1:
                    lerpTarget = pointLocations.right.position;
                    break;
            }
        }
        if ( Mathf.Abs(vertical) > 0 && horizontal == 0)
        {
            switch (Mathf.RoundToInt(vertical))
            {
                default:
                    break;
                case 1:
                    lerpTarget = pointLocations.up.position;
                    break;
                case -1:
                    lerpTarget = pointLocations.down.position;
                    break;
            }
        }
        if (Mathf.Abs(vertical) > 0 && Mathf.Abs(horizontal) > 0)
        {
            if (vertical > 0)
            {
                if (horizontal > 0)
                {
                    lerpTarget = pointLocations.upLeft.position;
                }
                if (horizontal < 0)
                {
                    lerpTarget = pointLocations.upRight.position;
                }
            }
            if (vertical < 0)
            {
                if (horizontal > 0)
                {
                    lerpTarget = pointLocations.downLeft.position;
                }
                if (horizontal < 0)
                {
                    lerpTarget = pointLocations.downRight.position;
                }
            }
        }
    }

}

 