using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackTriggerHandler : MonoBehaviour
{
    [SerializeField] private List<EnemyMono> _enemies;
    public EnemyMono TargetEnemy { get; private set; }

    public UnityEvent<EnemyMono> OnEnemyChanged;

    private void Awake()
    {
        _enemies = new List<EnemyMono>();
        TargetEnemy = null;
    }

    private void SetNearestEnemy()
    {
        if (_enemies.Count > 0)
        {
            float smallestLength = 1000f;
            EnemyMono currentEnemy = _enemies[0];

            foreach (EnemyMono enemy in _enemies)
            {
                float currentLength = Vector3.Distance(transform.position, enemy.transform.position);
                if (currentLength < smallestLength)
                {
                    smallestLength = currentLength;
                    currentEnemy = enemy;
                }
            }
            if (TargetEnemy != currentEnemy)
            {
                TargetEnemy?.SetTargetIndicator(false);
                TargetEnemy = currentEnemy;
            }
            TargetEnemy.SetTargetIndicator(true);
            
        }
        else
        {
            TargetEnemy = null;
        }

        OnEnemyChanged.Invoke(TargetEnemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EnemyMono>(out EnemyMono enemy))
        {
            if (enemy.IsActive())
            {
                _enemies.Add(enemy);
                enemy.OnDead.AddListener(RemoveEnemy);
                SetNearestEnemy();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyMono>(out EnemyMono enemy))
        {

            _enemies.Remove(enemy);
            SetNearestEnemy();
            enemy.SetTargetIndicator(false);
        }
    }

    //public EnemyMono GetCurrentEnemyTarget()
    //{
    //    if (_enemies.Count > 0)
    //    {
    //        float smallestLength = 1000f;
    //        EnemyMono currentEnemy = _enemies[0];

    //        foreach (EnemyMono enemy in _enemies)
    //        {
    //            float currentLength = Vector3.Distance(transform.position, enemy.transform.position);
    //            if (currentLength < smallestLength)
    //            {
    //                smallestLength = currentLength;
    //                currentEnemy = enemy;
    //            }
    //        }
    //        if (TargetEnemy != currentEnemy)
    //        {
    //            TargetEnemy?.SetTargetIndicator(false);
    //            TargetEnemy = currentEnemy;
    //        }
    //        TargetEnemy.SetTargetIndicator(true);
    //        return TargetEnemy;
    //    }

    //    return null;
    //}

    private void RemoveEnemy(EnemyMono enemy)
    {
        _enemies.Remove(enemy);
        SetNearestEnemy();
    }

    public void UpdateTarget()
    {
        SetNearestEnemy();
    }
}
