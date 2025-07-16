using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    public bool isPunched;
    public bool isPickedUp;
    private void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
    }
    private void Start()
    {
        DisableRagdoll();
    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableRagdoll();
        }
    }*/

    public void EnableRagdoll()
    {
        isPunched = true;
        if (animator != null)
        {
            animator.enabled = false;
        }

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
        }


        foreach (Collider col in ragdollColliders)
        {
            if (col.gameObject != this.gameObject)
            {
                col.enabled = true;
            }
        }
    }

    public void DisableRagdoll()
    {
        if (animator != null)
        {
            animator.enabled = true;
        }

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col.gameObject != this.gameObject)
            {
                col.enabled = false;
            }
        }
    }
    public void PrepareForPickup()
    {
        isPickedUp = true;
        foreach (Collider col in ragdollColliders)
        {
            if (col.gameObject != this.gameObject)
            {
                col.enabled = false;
            }
        }
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}