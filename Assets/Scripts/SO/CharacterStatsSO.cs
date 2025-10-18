using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats,", menuName = "Stats/Player")]
public class CharacterStatsSO : ScriptableObject
{
    public Stats stats;
    
    [System.Serializable]
    public class Stats
    {
        public string statsName;

        [Space(10)]
        [Header("Move")]
        public float walkSpeed;
        [Tooltip("Walk Speed when player is crouching")]
        public float crouchSpeed;
        public float sprintSpeed;
        public float gravity;

        [Space(10)]
        [Header("Look Sensitive")]

        [Tooltip("Mouse Speed")]
        [Range(1, 200)]
        public float lookSensitive;

        [Tooltip("Is the limit degree that player can look up and down")]
        [Range(45, 90)]
        public float lookLimit;

        #region Crouch
        [Space(10)]
        [Header("Crouching")]
        [Tooltip("Character Collider when crouch")]
        public float crouchHeight;
        [Tooltip("Character Collider when standing")]
        public float standHeight;
        [Tooltip("Time started crouching")]
        public float timeToCrouch;
        [Tooltip("Viewpoint of character when crouching")]
        public Vector3 crouchingCenter;
        [Tooltip("Viewpoint of character when standing")]
        public Vector3 standingCenter;
        #endregion

        #region HeadBob
        [Space(10)]
        [Header("Head Bob")]
        [Range(1, 100)]
        public float walkBobSpeed = 14f;
        [Range(0, 30)]
        public float walkBobAmount = 0.5f;
        [Range(1, 100)]
        public float sprintBobSpeed = 18f;
        [Range(0, 30)]
        public float sprintBobAmount = 1f;
        [Range(1, 100)]
        public float crouchBobSpeed = 8f;
        [Range(0, 30)]
        public float crouchBobAmount = 0.25f;
        #endregion
    }
}
