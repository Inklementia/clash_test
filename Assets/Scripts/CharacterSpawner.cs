using UnityEngine;
using UnityEngine.Serialization;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Tag playerTag;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxAllowedPlayers;
    
    private int _currentPlayerNumber;
    private Camera _camera;

    private void Awake()
    {
        _currentPlayerNumber = maxAllowedPlayers;
        _camera = Camera.main;
    }

    private void Update()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && _currentPlayerNumber > 0)
            {
                ObjectPooler.Instance.SpawnFromPool(playerTag,
                    new Vector3(hit.point.x, hit.point.y + playerPrefab.transform.position.y, hit.point.z),
                    Quaternion.identity);
                _currentPlayerNumber--;
            }
        }
    }
}