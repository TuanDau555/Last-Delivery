using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] private Light flashLight;
    private bool _isOn = false;

    #region Execute
    void Start()
    {
        if(flashLight == null)
        {
            flashLight = GetComponentInChildren<Light>();

            flashLight.enabled = _isOn;
        }
        flashLight.enabled = _isOn;

    }

    void Update()
    {
        if (InputManager.Instance.IsOpenFlashLight())
        {
            ToggleFlashLight();
        }
    }

    private void ToggleFlashLight()
    {
        _isOn = !_isOn;
        flashLight.enabled = _isOn;
    }
    #endregion
}