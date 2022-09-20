using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public GameObject CurrentSpawnPoint;
    [SerializeField] private GameObject spawnPointPrefab;

    private void Start()
    {
        if (!CurrentSpawnPoint)
        {
            CurrentSpawnPoint = Instantiate(spawnPointPrefab, transform.position, Quaternion.identity);
        }
    }

    public void CreateSpawnPoint()
    {
        Instantiate(spawnPointPrefab, transform.position + transform.forward, Quaternion.identity);
    }
}
