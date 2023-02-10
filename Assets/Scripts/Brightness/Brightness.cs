using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Volume Vol;
    private ColorAdjustments colorAdjustments;

    public Slider Brightnessslider;

    void Start()
    {
        Vol.profile.TryGet<ColorAdjustments>(out colorAdjustments);
    }

    void Update()
    {
        colorAdjustments.postExposure.value = Brightnessslider.value;
    }
}
