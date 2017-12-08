using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WormController : MonoBehaviour
{
    [SerializeField] protected bool moving = false;
    [SerializeField] protected Transform armature;
    [SerializeField] protected Transform target;
    [SerializeField] protected float maxAngle = 30f;
    [SerializeField] protected float offset = 0f;
    [SerializeField] protected float smoothness = 5f;
    [SerializeField] protected bool debugSwitch = false;


    protected Animator anim;
    private bool stagedTurn = false;
    private float timing = 0f;

    // Agnle calculations
    private Vector3 targetDir;
    private float angle;
    private bool negative;

    /// <summary>
    /// Gets or sets a value indicating whether this gameobject is moving.
    /// </summary>
    /// <value><c>true</c> if this gameobject is moving; otherwise, <c>false</c>.</value>
    public bool IsMoving
    {
        get { return moving; }

        set
        {
            anim.SetBool("Move", value);
            moving = value;
        }
    }


    /// <summary>
    /// Pauses the worms movement.
    /// </summary>
    /// <returns>Enumeration for coroutine usage.</returns>
    /// <param name="pauseTime">Pause time in seconds.</param>
    public IEnumerator PauseMovement(float pauseTime)
    {
        this.IsMoving = false;

        // Wait until in Idle state
        while(!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            yield return null;

        stagedTurn = true;
        yield return new WaitForSeconds(pauseTime);
        stagedTurn = false;

        this.IsMoving = true;
    }

    /// <summary>
    /// On initialization.
    /// </summary>
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Move", moving);

        if(armature == null)
            Debug.LogError("[WormController] Worm armature is not defined.");

        if(target == null)
            Debug.LogWarning("[WormController] Worm target is not defined.");
    }

    /// <summary>
    /// Every frame.
    /// </summary>
    private void Update()
    {
        // Reset Y-axis of the worm when stopping
        // Translated by root motion
        if(!moving)
        {
            if(smoothness <= 0f)
                smoothness = 1f;

            Vector3 newPosition = new Vector3(
                transform.position.x,
                Mathf.Lerp(transform.position.y, offset, Time.deltaTime * smoothness),
                transform.position.z
            );

            // Apply new position
            transform.position = newPosition;

            // Turn to target when movement is just paused
            if(stagedTurn)
            {
                // Calculate rotation to target
                CalculateTargetRotation();

                // Apply rotation
                transform.Rotate(Vector3.up, (negative ? -angle : angle) * Time.deltaTime * smoothness * 2f);
            }
        }

        // Turn to target without pausing when turn angle is small enough
        if(moving && (target != null))
        {
            // Calculate rotation to target
            CalculateTargetRotation();

            if(angle > maxAngle)
            {
                // Pause movement to turn
                StartCoroutine(PauseMovement(0.5f));
            }
            else
            {
                // Turn 'on the fly'
                transform.Rotate(Vector3.up, (negative ? -angle : angle) * Time.deltaTime * smoothness);
            }
        }
    }

    private void CalculateTargetRotation()
    {
        if(target != null)
        {
            // Calculate direction vector from worm to target
            targetDir = target.position - armature.position;
            targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up);

            if(debugSwitch)
                Debug.DrawRay(armature.position, targetDir, Color.red);

            // Calculate angle from the target direction to the worms forward vector
            // in order to get how many degrees the object has to turn
            angle = Vector3.Angle(-armature.up, targetDir);

            // Gather the normal of the vectors to determine in which direction the object
            // has to rotate
            negative = Vector3.Cross(-armature.up, targetDir).y < 0;
        }
    }
}
