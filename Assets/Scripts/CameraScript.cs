using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform sphere;
    [SerializeField]
    private Transform cameraFixed;
    [SerializeField]
    private float minFpvDistance = 2f;
    [SerializeField]
    private float maxFpvDistance = 10f;

    private Vector3 rod;
    private bool isFixed;
    private InputAction lookAction;
    private float rotV;
    private float sensV = 2f;
    private float rotH;
    private float sensH = 5f;
    private bool inFpv = false;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFixed = false;
        rod = this.transform.position - sphere.position;
        lookAction = InputSystem.actions.FindAction("Look");
        rotV = this.transform.eulerAngles.x;
        rotH = this.transform.eulerAngles.y;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isFixed)
        {
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            float dV = -lookValue.y * Time.deltaTime * sensV;
            float dH = lookValue.x * Time.deltaTime * sensH;

            // Handle zooming
            float zoom = Input.mouseScrollDelta.y;
            if (zoom != 0.0f)
            {
                if (rod.magnitude >= minFpvDistance)
                {
                    float k = 1 - zoom * 0.1f;
                    Vector3 newRod = rod * k;
                    
                    if (newRod.magnitude < minFpvDistance)
                    {
                        rod = rod * 0.01f;
                        inFpv = true;
                        cam.fieldOfView = 90f;
                    }
                    else if (newRod.magnitude <= maxFpvDistance)
                    {
                        rod = newRod;
                    }
                }
                else
                {
                    if (zoom < 0.0f)
                    {
                        rod = rod * 101f;
                        if (rod.magnitude > maxFpvDistance)
                        {
                            rod = rod.normalized * maxFpvDistance;
                        }
                        inFpv = false;
                        cam.fieldOfView = 52f;
                        rotV = 45f;
                    }
                }
            }
            
            // Handle rotation
            if (inFpv)
            {
                rotV += dV;
                rotV = Mathf.Clamp(rotV, -89f, 89f);
                rotH += dH;
            }
            else
            {
                if (rotV + dV >= 35 && rotV + dV <= 65)
                {
                    rotV += dV;
                }
                rotH += dH;
            }

            this.transform.eulerAngles = new Vector3(rotV, rotH, 0f);

            // Handle position
            Vector3 baseOffset = new Vector3(0f, 0.05f, 0f);
            if (inFpv)
            {
                this.transform.position = sphere.position + baseOffset;
            }
            else
            {
                this.transform.position = sphere.position + baseOffset + Quaternion.Euler(0, rotH, 0) * (rod - baseOffset);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isFixed = !isFixed;
            if (isFixed)
            {
                this.transform.position = cameraFixed.position;
                this.transform.rotation = cameraFixed.rotation;
            }
        }
    }
}
