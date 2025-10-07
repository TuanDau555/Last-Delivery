using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats,", menuName = "Stats/Cat")]
public class CatStatsSO : ScriptableObject
{
    public Stats stats;

    [System.Serializable]
    public class Stats
    {
        [Tooltip("If player move this range start follow")]
        [Range(1, 10)]
        public float followDistance;

        [Tooltip("If player in this range cat stop")]
        [Range(1, 10)]
        public float stopDistance;
    }
}