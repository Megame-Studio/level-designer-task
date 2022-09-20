using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         FindObjectOfType<RespawnSystem>().CurrentSpawnPoint = gameObject;
      }
   }
}
