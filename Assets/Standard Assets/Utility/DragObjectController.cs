using UnityEngine;
using UnityEngine.UI;

public class DragObjectController : MonoBehaviour
{
    public Transform cameraTransform;
    private Rigidbody pickedUpObject;
    private float pickupDistance;

    public float maxDistance = 5f;
    public float minHoldDistance = 2f;

    [Header("Pick Up Objects")]
    [SerializeField]
    private float pickedUpDrag = 10f;

    [SerializeField]
    private float pickupHoldForce = 50f;

    [SerializeField]
    private float throwForce = 750f;

    public LayerMask raycastLayerMask = ~(1 << 6);
    public bool raycastTriggers = false;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (pickedUpObject != null)
                DropObject(throwForce);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedUpObject == null)
                PickupObject();
            else
                DropObject(0);
        }
    }

    private void FixedUpdate()
    {
        UpdatePickedUpObject();
    }

    private void PickupObject()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, maxDistance, raycastLayerMask, raycastTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore))
        {
            if (hit.rigidbody != null)
            {
                pickupDistance = Mathf.Max(minHoldDistance, hit.distance);
                pickedUpObject = hit.rigidbody;
                pickedUpObject.useGravity = false;
                pickedUpObject.drag = pickedUpDrag;
                pickedUpObject.angularDrag = pickedUpDrag;
            }
        }
    }

    private void UpdatePickedUpObject()
    {
        if (pickedUpObject != null)
        {
            var target = cameraTransform.position + cameraTransform.forward * pickupDistance;
            var dir = (target - pickedUpObject.position).normalized;

            var distance = Vector3.Distance(target, pickedUpObject.transform.position);
            var force = Mathf.Min(pickupHoldForce, distance);

            pickedUpObject.AddForce(dir * force, ForceMode.VelocityChange);
        }
    }

    private void DropObject(float force)
    {
        if (pickedUpObject != null)
        {
            pickedUpObject.useGravity = true;
            pickedUpObject.drag = 0;
            pickedUpObject.angularDrag = 0.05f;
            pickedUpObject.AddForce(cameraTransform.forward * force);
            pickedUpObject = null;
        }
    }
}