using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = FindObjectOfType<RespawnSystem>().CurrentSpawnPoint.transform.position;
            
            var characterController = other.GetComponent<CharacterController>();
            if (characterController)
            {
                Physics.SyncTransforms();
            }
        }
    }
}
