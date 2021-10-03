using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Phyiscs")]
    [SerializeField] private float gravity = 10f;
    [SerializeField] private Vector3 velocity;
    private float currentGravityScale = 1f;

    [Header("Movement")]
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float maxVelocityXZ = 5f;
    [SerializeField] private float drag = .5f;

    private Rigidbody platformRB;
    Vector2 moveInput;
   
    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] [Range(1f,10f)] private float gravityJumpScale = 1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferTimer;

    [Header("Look")]
    [SerializeField] private float lookSpeedX = 100;
    [SerializeField] private float lookSpeedY = 100;
    [SerializeField] private float lookXLimit = 80;
    float rotationX;
    private Vector2 axisInput;

    [Header("Hookshot")]
    [SerializeField] private float hookshotDistance = 50;
    [SerializeField] private float hookshotSpeed = 10;
    [SerializeField] private float hookshotYOffset = 1.5f;
    [SerializeField] private LayerMask hookshotLayerMask;
    [SerializeField] private float magnetDistance = 1;
    [SerializeField] private GameObject clawPrefab;
    private GameObject claw;
    private bool hookshotting = false;
    private Transform hookshotTarget;
    private float hookshotTime;
    private float hookshotTimer;
    private Vector3[] lineRendererVerticies;
    Vector3 startPosition;
    Vector3 hookTargetPosition;

    LineRenderer lineRenderer;

    [Header("Slowmo")]
    [SerializeField] private float slowMotionSpeed = 0.3f;


    CharacterController controller;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
        lineRendererVerticies = new Vector3[2];
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.menuOn) return;
        GetInput();
        HandleJump();
        HandleHookshot();
    }
    private void FixedUpdate()
    {
        if (Menu.menuOn) return;
        HandleLook();
        Move();
        HandleGravity();
        HandlePlatforms();
        Vector3 platformVelocity = Vector3.zero;
        if (platformRB != null) platformVelocity = platformRB.velocity;
        if (!hookshotting)
        {
            controller.Move((velocity + platformVelocity) * Time.fixedDeltaTime);
        }
    }

    private void HandlePlatforms()
    {
        if ((controller.collisionFlags & CollisionFlags.Below) != 0 && velocity.y <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, hookshotLayerMask))
            {
                platformRB = hit.transform.GetComponent<Rigidbody>();
                //if(platformRB != null)
                //    transform.SetParent(platformRB.transform);
            }
        }
        else if (platformRB != null)
        {
            velocity += platformRB.velocity * Time.fixedDeltaTime;
            platformRB = null;
            //transform.SetParent(null);
        }
    }
    private void HandleHookshot()
    {
        if (Input.GetMouseButtonDown(0) && !hookshotting)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hookshotDistance, hookshotLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Hookshot"))
                {
                    hookshotting = true;
                    hookshotTarget = hit.transform;
                    controller.detectCollisions = false;
                    float distance = (hookshotTarget.position + hookshotYOffset * Vector3.up - transform.position).magnitude;
                    hookshotTime = distance / hookshotSpeed;
                    hookshotTimer = 0;
                    controller.enabled = false;
                    platformRB = null;
                    startPosition = transform.position;
                    lineRenderer.enabled = true;
                    hookTargetPosition = hit.point - hookshotTarget.position;
                    claw = Instantiate(clawPrefab);
                    claw.transform.position = hit.point;
                    Vector3 scale = claw.transform.localScale;
                    claw.transform.parent = (hookshotTarget);
                    claw.transform.localScale = scale;
                    claw.transform.rotation = Quaternion.LookRotation(hit.point - transform.position, Vector3.up);
                    UpdateLineRenderer();
                }
            }
        }
        if (hookshotting)
        {
            float t = hookshotTimer / hookshotTime;
            if(hookshotTarget == null)
            {
                hookshotting = false;
                controller.detectCollisions = true;
                controller.enabled = true;
                lineRenderer.enabled = false;
                if (claw) Destroy(claw);
                return;
            }
            Vector3 newPosition = Vector3.Lerp(startPosition, hookshotTarget.position + hookshotYOffset * Vector3.up, t);
            transform.position = newPosition;
            UpdateLineRenderer();
            if (t >= 1)
            {
                hookshotting = false;
                transform.position = hookshotTarget.position + hookshotYOffset * Vector3.up;
                controller.detectCollisions = true;
                controller.enabled = true;
                lineRenderer.enabled = false;
                if (claw) Destroy(claw);
                if (Input.GetButton("Jump"))
                {
                    velocity.y = jumpVelocity;
                    velocity.x = transform.forward.x * hookshotSpeed;
                    velocity.z = transform.forward.z * hookshotSpeed;
                    jumpBufferTimer = -1;
                }
                else velocity = Vector3.down * 10;
            }
            else hookshotTimer += Time.deltaTime;
        }
    }

    private void UpdateLineRenderer()
    {
        Vector3 targetPos = (hookTargetPosition) + hookshotTarget.position;
        lineRendererVerticies[0] = transform.position;
        lineRendererVerticies[1] = targetPos;
        lineRenderer.SetPositions(lineRendererVerticies);
    }

    private void GetInput()
    {
        axisInput.x = Input.GetAxisRaw("Mouse X");
        axisInput.y = Input.GetAxisRaw("Mouse Y");
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        jumpBufferTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
        }

    }
    private void HandleLook()
    {
        rotationX += -axisInput.y * lookSpeedY * Time.fixedDeltaTime * GameManager.instance.sensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, axisInput.x * lookSpeedX * Time.fixedDeltaTime * GameManager.instance.sensitivity, 0);
    }
    private void HandleGravity()
    {
        if (!controller.isGrounded && !hookshotting)
        {
            currentGravityScale = Input.GetButton("Jump") && velocity.y < 0 ? 1 : gravityJumpScale;
            velocity.y -= gravity * Time.fixedDeltaTime * currentGravityScale;
        }
    }
    private void Move()
    {
        if (hookshotting) return;
        //calculate acceleration from input
        float accXPart = controller.isGrounded ?
                        moveInput.x * groundAcceleration * Time.fixedDeltaTime:
                        moveInput.x * airAcceleration * Time.fixedDeltaTime;
        float accZPart = controller.isGrounded ?
                        moveInput.y * groundAcceleration * Time.fixedDeltaTime :
                        moveInput.y * airAcceleration * Time.fixedDeltaTime;
        //calculate acceleration relative to view
        Vector3 acceleration;
        acceleration = transform.forward * accZPart;
        acceleration += transform.right * accXPart;

        velocity.x += acceleration.x;
        velocity.z += acceleration.z;

        //handle max speed
        Vector2 xzVelocity = new Vector2(velocity.x, velocity.z);
        if(xzVelocity.magnitude > maxVelocityXZ)
        {
            xzVelocity = xzVelocity.normalized * maxVelocityXZ;
            velocity.x = xzVelocity.x;
            velocity.z = xzVelocity.y;
        }
        if (controller.isGrounded && moveInput.sqrMagnitude < .5f) {
            velocity.x *= drag;
            velocity.z *= drag;
        }
    }
    private void HandleJump()
    {
        if (controller.isGrounded && !hookshotting && jumpBufferTimer >= 0)
        {
            velocity.y = jumpVelocity;
            jumpBufferTimer = -1f;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Death death = hit.gameObject.GetComponent<Death>();
        if (death != null) GameManager.instance.Lose();
    }
}
