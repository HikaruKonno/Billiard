using UnityEngine.UI;
using UnityEngine;

public class GaugeControl : MonoBehaviour
{

    private Image _gauge;
    private void Start()
    {
        _gauge = GetComponent<Image>();
        BallShooter.PowerUI = OnPowerGaugeUI;
    }

    /// <summary>
    /// Š„‡‚ğ•\¦
    /// </summary>
    /// <param name="ratio">Š„‡</param>
    public void OnPowerGaugeUI(float ratio)
    {
        _gauge.fillAmount = ratio;
    }
}
