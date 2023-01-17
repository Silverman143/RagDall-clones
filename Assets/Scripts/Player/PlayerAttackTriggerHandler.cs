using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTriggerHandler : MonoBehaviour
{
    [SerializeField] private List<EnemyMono> _enemies;

    public EnemyMono GetNearestEnemy() => _enemies.FirstOrDefault();

    private void Awake()
    {
        _enemies = new List<EnemyMono>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EnemyMono>(out EnemyMono enemy))
        {
            _enemies.Add(enemy);
            Debug.Log("Add enemy in Trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyMono>(out EnemyMono enemy))
        {
            _enemies.Remove(enemy);
        }
    }
}
