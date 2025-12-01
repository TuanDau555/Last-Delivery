using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootstepManager : MonoBehaviour
{
    public List<AudioClip> groundSteps = new List<AudioClip>();
    public List<AudioClip> concreteSteps = new List<AudioClip>();

    private enum Surface { ground, concrete};
    private Surface surface;

    private List<AudioClip> currentList;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();            
    }

    public void PlayStep ()
    {
        if(currentList == null)
            return;
        
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);
    }

    private void SelectStepList ()
    {
        switch (surface)
        {
            case Surface.ground:
                currentList = groundSteps;
                break;
            case Surface.concrete:
                currentList = concreteSteps;
                break;
            default:
                currentList = null;
                break;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Ground")
        {
            surface = Surface.ground;
        }

        if (hit.transform.tag == "Concrete")
        {
            surface = Surface.concrete;
        }

        SelectStepList();
        
    }

}
