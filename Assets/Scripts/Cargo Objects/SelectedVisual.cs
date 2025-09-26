using System;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private BaseInteract baseInteract;
    [SerializeField] private GameObject[] selectedVisualArray;

    void Start()
    {
        Interactable.Instance.OnSelectedChanged += Interactable_OnSelectedChanged;
    }

    void OnDisable()
    {
        Interactable.Instance.OnSelectedChanged -= Interactable_OnSelectedChanged;
    }

    private void Interactable_OnSelectedChanged(object sender, Interactable.OnSelectedChangedEventArgs e)
    {
        if (e.selectedObjectEvent == baseInteract)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

     private void Show()
    {
        foreach (GameObject selectedVisual in selectedVisualArray)
        {
            selectedVisual.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject selectedVisual in selectedVisualArray)
        {
            selectedVisual.SetActive(false);
        }
    }
}
