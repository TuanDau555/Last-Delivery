using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats,", menuName = "Stats/Enemy")]

public class EnemyStatsSO : ScriptableObject
{
    public Stats stats;

    [System.Serializable]
    public class Stats
    {
        public string statsName;

        [Header("Patrol")]
        public bool patrolWaiting;
        public float waitTime;

        [Space(10)]
        [Header("Movement")]
        public float walkSpeed;
        public float chaseSpeed;
        
        [Space(10)]
        [Header("Field of View")]
        [Tooltip("Radius of enemy's view")]
        [Range(1, 60)]
        public float radius;

        [Tooltip("Angle of enemy's view")]
        [Range(1, 360)]
        public float angle;
        
        [Tooltip("Angle of enemy's view")]
        [Range(1, 3)]
        public float attackDistance;

        [Tooltip("Angle of enemy's view")]
        [Range(1, 3)]
        public float stoppingDistance;
        [Tooltip("Wait time before back to Patrol State")]
        [Range(1f, 3f)]
        public float lostSightDelay = 2f;
        public LayerMask targetMask;
        public LayerMask obstructionMask;

        [Space(10)]
        [Header("Attack")]
        public float attackRange;
        public float attackDamage;
        public float timeBetweenAttacks;

        [Space(10)]
        [Header("Move Prediction")]
        [Range(-1, 1)]
        public float movePredictionThreshold = 0;
        [Range(0.25f, 2f)]
        public float movePredictionTime = 1;
    }
}
