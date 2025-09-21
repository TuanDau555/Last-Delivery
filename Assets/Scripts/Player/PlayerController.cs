using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Parameter 
    [Header("Reference")]
    [SerializeField] private CharacterStatsSO playerStatsSO;

    #endregion
}
