using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
        [Header("Field of View")]
        [Tooltip("Radius of enemy's view")]
        [Range(1, 60)]
        public float radius;
        [Tooltip("Angle of enemy's view")]
        [Range(1, 360)]
        public float angle;
        public LayerMask targetMask;
        public LayerMask obstructionMask;

        [Space(10)]
        [Header("Attack")]
        public float attackRange;
        public float attackDamage;
        public float timeBetweenAttacks;

        [Space(10)]
        [Header("Health")]
        public float maxHealth;
    }
}
