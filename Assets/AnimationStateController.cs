using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    GroundCheck groundCheck;
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isCastingHash;

    public GameObject objectToSummon; // The object to summon
    public GameObject vfxPrefab; // The VFX prefab to appear in front of the character
    public float vfxLifespan = 2.0f; // Time in seconds before VFX despawns
    public float objectsLifespan = 2.0f;
    public float summonDistance = 2.0f;
    public float cooldownTime = 2.0f; // Cooldown time in seconds

    private float lastSummonTime; // Time when the last object was summoned

    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isCastingHash = Animator.StringToHash("isCasting");
        lastSummonTime = -cooldownTime; // Allow immediate summoning at start
    }

    // Update is called once per frame
    void Update()
    {
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isCasting = animator.GetBool(isCastingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKey(KeyCode.Space);
        bool castingPressed = Input.GetKey(KeyCode.E);

        // Handle walking state
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // Handle running state
        if (!isRunning && forwardPressed && runPressed)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }

        // Handle casting state
        if (!isCasting && castingPressed)
        {
            animator.SetBool(isCastingHash, true);
            TrySummonObject(); // Summon object when casting starts
            SummonVFX();
        }
        if (isCasting && !castingPressed)
        {
            animator.SetBool(isCastingHash, false);
        }

        // Handle jumping state
        if (jumpPressed && !isJumping)
        {
            animator.SetBool(isJumpingHash, true);
        }
        else if (!jumpPressed && isJumping)
        {
            animator.SetBool(isJumpingHash, false);
        }
    }

    void TrySummonObject()
    {
        // Check if the cooldown period has elapsed
        if (Time.time - lastSummonTime >= cooldownTime)
        {
            SummonObject();
            lastSummonTime = Time.time; // Update the last summon time
        }
    }

    void SummonObject()
    {
        if (objectToSummon != null)
    {
            // Calculate the position in front of the player
            Vector3 summonPosition = transform.position + transform.forward * summonDistance;

             // Based on characters current rotation
            Quaternion summonRotation = transform.rotation;
            Quaternion leftRotation = Quaternion.Euler(0, 90, 0);

            // Instantiate the object at the calculated position
            GameObject objectsInstance = Instantiate(objectToSummon, summonPosition, summonRotation * leftRotation);
            StartCoroutine(DestroyObjectAfterDelay(objectsInstance, objectsLifespan));
        }
        else
        {
            Debug.LogWarning("Object to summon is not assigned.");
        }
    }

    void SummonVFX()
    {
        if (vfxPrefab != null)
        {
            // Calculate the position in front of the player
            Vector3 vfxPosition = transform.position + transform.forward * summonDistance;

            // Instantiate the VFX at the calculated position
            GameObject vfxInstance = Instantiate(vfxPrefab, vfxPosition, Quaternion.identity);

            // Start a coroutine to destroy the VFX after a delay
            StartCoroutine(DestroyVFXAfterDelay(vfxInstance, vfxLifespan));

        }
        else
        {
            Debug.LogWarning("VFX prefab is not assigned.");
        }
    }

    IEnumerator DestroyVFXAfterDelay(GameObject vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(vfx);
    }
    IEnumerator DestroyObjectAfterDelay(GameObject objects, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objects);
    }
}
    
