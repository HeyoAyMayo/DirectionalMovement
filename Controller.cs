
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RhythmCube.DataTypes;

public class Controller : MonoBehaviour
{


    // Some inspector stuff
    [Header("Movement Settings")]
    [Tooltip("The points where the player move to when inputs are made")] // Tooltip to display when hovered
    public Directions<Transform> pointLocations;  // All the locations where our player moves to

    [SerializeField] private InputActionAsset input;
    // Both vertical and horizontal directions 
    private float vertical, horizontal;
    // Lerping
    private Vector3 lerpTarget;
    [Tooltip("How much to lerp to the target position")]
    [SerializeField]
    private float lerpSpeed = 0.1f;


    public bool moving { get { return true; } set { moving = value; } }
    private void Start()
    {
        transform.position = pointLocations.middle.position;
    }
    private void OnEnable()
    {
        input["Directional"].Enable();
    }

    private void OnDisable()
    {
        input["Directional"].Disable();
    }

    // Start is called before the first frame update
    private void Update()
    {
        
        if (transform.position != lerpTarget)
        {
            LerpToTarget();
        }
        if (GameManager.Instance.gameOver == false && moving)
        {
            DirectionalMovement();
        }
    }
    private void LerpToTarget()
    {
        transform.position = LerpVector(transform.position, lerpTarget, lerpSpeed * Time.deltaTime);
    }
    public static Vector3 LerpVector( Vector3 position, Vector3 targetPosition, float t)
    {
        float x = Mathf.Lerp(position.x, targetPosition.x, t);
        float y = Mathf.Lerp(position.y, targetPosition.y, t);
        float z = Mathf.Lerp(position.z, targetPosition.z, t);
        return new Vector3(x, y, z);
    }
    private void DirectionalMovement()
    {
        lerpTarget = GetLerpTarget();
    }
    private Vector3 GetLerpTarget()
    {
        Vector2 m_Move = input["Directional"].ReadValue<Vector2>();
        horizontal = m_Move.x;
        vertical = m_Move.y;
        if (horizontal == 0 && vertical == 0)
        {
            return pointLocations.middle.position;
        }
        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            return GetHorizontalTarget(horizontal);
        }
        if ( Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            return GetVerticalTarget(vertical);
        }
        return GetDiagonalTarget(vertical, horizontal);
    }

    private Vector3 GetDiagonalTarget(float verticalInput, float horizontalInput)
    {
        if (!(verticalInput > 0) && !(verticalInput < 0))
        {
            return new();
        }
        if (verticalInput > 0)
        {
            if (horizontalInput > 0 )
            {
                return pointLocations.upLeft.position;
            }
            if (horizontalInput < 0)
            {
                return pointLocations.upRight.position;
            }
        }
        if (verticalInput < 0)
        {
            if (horizontalInput > 0)
            {
                return pointLocations.downLeft.position;
            }
            if (horizontalInput < 0)
            {
                return pointLocations.downRight.position;
            }
        }
        return new();
    }
    private Vector3 GetVerticalTarget(float verticalInput)
    {
        switch (Mathf.RoundToInt(verticalInput))
        {
            default:
                return new();
            case 1:
                return pointLocations.up.position;
            case -1:
                return pointLocations.down.position;
        }
    }
    private Vector3 GetHorizontalTarget(float horizontalInput)
    {
        switch (Mathf.RoundToInt(horizontalInput))
        {
            default:
                return new();
            case 1:
                return pointLocations.left.position;
            case -1:
                return pointLocations.right.position;
        }
    }
}
