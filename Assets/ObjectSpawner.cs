using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _spawnAmount;
    private void Awake()
    {
        for(int i = 0; i < this._spawnAmount; i++)
        {
            Instantiate(this._prefab, this.transform);
        }
    }
}
