using UnityEngine;
using UI = UnityEngine.UI;
using Unity.Mathematics;

sealed class NoisyTarget : MonoBehaviour
{
    [SerializeField] UI.Slider _noiseAmountSlider = null;
    [SerializeField] UI.Text _noiseAmountLabel = null;

    RectTransform _xform;

    float NoiseTime
      => Time.time * 10;

    float2 NoiseOffset
      => math.float2(noise.snoise(math.float2(NoiseTime, 0.32f)),
                     noise.snoise(math.float2(NoiseTime, 2.71f)));

    void Start()
      => _xform = GetComponent<RectTransform>();

    void Update()
    {
        _noiseAmountLabel.text = $"{_noiseAmountSlider.value:0}";

        _xform.anchoredPosition =
          math.float3(Input.mousePosition).xy +
          NoiseOffset * _noiseAmountSlider.value;
    }
}
