using QuickScripts;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    [SerializeField] 
    private Image aim;
    
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField] 
    private AudioClip interactSound;

    [SerializeField]
    private AudioClip throwSound;

    public LayerMask raycastLayerMask = ~(1 << 6);
    public LayerMask interactableMask;
    public bool raycastTriggers = false;

    private void Awake()
    {
        FindObjectsOfType<QuickDoor>().ToList().ForEach(door => door.gameObject.layer = 3);
        var allObjects = FindObjectsOfType<GameObject>();
        var rigidbodys =
            from obj in allObjects
            where obj.GetComponent<Rigidbody>() != null
            select obj;

        foreach (var rig in rigidbodys)
        {
            if (rig.CompareTag("Player"))
            {
                continue;
            }

            rig.gameObject.layer = 3;
        }
    }

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

            if (aim.color == Color.green)
            {
                PlaySoundInteract();
            }
        }
        
        aim.color = Color.white;
        
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, maxDistance, interactableMask))
        {
            aim.color = Color.green;
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

    private void PlaySoundInteract()
    {
        audioSource.clip = interactSound;
        audioSource.Play();
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
            audioSource.clip = throwSound;
            audioSource.Play();
        }
    }
}