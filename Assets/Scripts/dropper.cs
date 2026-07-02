using UnityEngine;

public class dropper : MonoBehaviour
{
    [Header("Dropper Configuration")]
    [Tooltip("The obstacle/dropper object that will be dropped.")]
    public GameObject dropperObject;

    [Tooltip("The trigger collider GameObject that detects the player walking under.")]
    public GameObject dropperTrigger;

    [Tooltip("Delay in seconds before enabling gravity once triggered.")]
    public float dropDelay = 0.5f;

    private Rigidbody dropperRigidbody;
    private bool isDropped = false;

    void Start()
    {
        // Cache the Rigidbody component from the dropper object if it is assigned
        if (dropperObject != null)
        {
            dropperRigidbody = dropperObject.GetComponent<Rigidbody>();
            if (dropperRigidbody != null)
            {
                // Disable gravity initially so it doesn't fall on game start
                dropperRigidbody.useGravity = false;
            }
            else
            {
                Debug.LogWarning($"[Dropper] The assigned dropperObject '{dropperObject.name}' does not have a Rigidbody component.", this);
            }
        }
        else
        {
            Debug.LogWarning("[Dropper] No dropperObject is assigned in the Inspector.", this);
        }

        // Set up the trigger helper if the trigger GameObject is assigned
        if (dropperTrigger != null)
        {
            // Verify that the trigger GameObject has a Collider set as a trigger
            Collider triggerCollider = dropperTrigger.GetComponent<Collider>();
            if (triggerCollider == null || !triggerCollider.isTrigger)
            {
                Debug.LogWarning($"[Dropper] The assigned dropperTrigger '{dropperTrigger.name}' either does not have a Collider or its Collider is not configured as a Trigger.", this);
            }

            DropperTriggerHelper triggerHelper = dropperTrigger.GetComponent<DropperTriggerHelper>();
            if (triggerHelper == null)
            {
                triggerHelper = dropperTrigger.AddComponent<DropperTriggerHelper>();
            }
            triggerHelper.Initialize(this);
        }
        else
        {
            Debug.LogWarning("[Dropper] No dropperTrigger is assigned in the Inspector.", this);
        }
    }

    public void TriggerDrop()
    {
        if (!isDropped)
        {
            isDropped = true;
            Invoke(nameof(EnableGravity), dropDelay);
        }
    }

    private void EnableGravity()
    {
        if (dropperRigidbody != null)
        {
            dropperRigidbody.useGravity = true;
        }
    }
}

/// <summary>
/// Dynamic helper class that forwards trigger enter events from the trigger GameObject to the dropper script.
/// </summary>
public class DropperTriggerHelper : MonoBehaviour
{
    private dropper mainDropper;

    public void Initialize(dropper script)
    {
        mainDropper = script;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mainDropper != null)
        {
            mainDropper.TriggerDrop();
        }
    }
}
