using UnityEngine;
using UnityEngine.InputSystem;

public class SphereScript : MonoBehaviour
{
    [SerializeField]
    private float forceFactor = 10f;
    [SerializeField] 
    private Camera mainCamera;
    [SerializeField]
    private Light spotLight;

    private Rigidbody rb;
    private InputAction moveAction;
    private float charge = 100.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        // Get camera's forward and right vectors
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        // Project vectors onto the horizontal plane by setting Y to 0 and normalizing
        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.y = 0;
        cameraRight.Normalize();
        
        // Calculate force using the projected vectors
        Vector3 force = cameraRight * moveValue.x * forceFactor + 
                       cameraForward * moveValue.y * forceFactor;
        
        rb.AddForce(force);
        charge -= Time.deltaTime;
        if (charge <= 0.0f)
        {
            charge = 0.0f;
            spotLight.enabled = false;
        }
        else
        {
            spotLight.enabled = true;
            spotLight.intensity = charge / 100.0f;
            spotLight.transform.forward = mainCamera.transform.forward;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Transform t = other.transform.parent;
        if (t == null)
        {
            t = other.transform;
        }
        if (t.gameObject.CompareTag("Battery"))
        {
            charge = 100.0f;
        }
    }
}
