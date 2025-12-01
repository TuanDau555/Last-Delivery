using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DoorTransition : BaseInteract
{
    [Space(10)]
    [Tooltip("Cat Reference")]
    [SerializeField] private CatAgent catAgent;

    [Space(10)]
    [Tooltip("Where you want player to appear the other side")]
    [SerializeField] private Transform destinationPoint;

    [Space(10)]
    [SerializeField] private Animator sceneTransition;

    [Tooltip("Fade in/out time")]
    [SerializeField] private float fadeDuration = 0.5f;

    private bool _isTransition = false; // alway set false at first because player don't open door at first 

    public override void Interact(PlayerController playerController)
    {
        if (_isTransition) return; // prevent open door while transition
        StartCoroutine(OpenDoor(playerController));
    }

    private IEnumerator OpenDoor(PlayerController player)
    {
        _isTransition = true;

        sceneTransition.SetTrigger("Day Crossfade Start");

        yield return new WaitForSeconds(fadeDuration);
        
        var _controller = player.GetComponent<CharacterController>();

        // turn of this component for a moment to move player 
        if (_controller != null)
            _controller.enabled = false;


        // Move player to the other side
        player.transform.position = destinationPoint.position;
        player.transform.forward = destinationPoint.forward;

        // Move cat to the other side
        if (catAgent != null && DeliveryManager.Instance.currentDeliveryState == DeliveryState.DELIVER)
        {
            var agent = catAgent.GetComponent<NavMeshAgent>();

            agent.enabled = false;

            catAgent.transform.position = destinationPoint.position + Vector3.left; // deviate to the left a bit

            agent.enabled = true;
        }

        if (_controller != null)
            _controller.enabled = true;

        sceneTransition.SetTrigger("Day Crossfade End");
        _isTransition = false;

    }
}