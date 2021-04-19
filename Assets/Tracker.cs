using UnityEngine;
using UI = UnityEngine.UI;

sealed class Tracker : MonoBehaviour
{
    [SerializeField] RectTransform _target = null;

    [SerializeField] UI.Slider _betaSlider = null;
    [SerializeField] UI.Text _betaLabel = null;

    [SerializeField] UI.Slider _minCutoffSlider = null;
    [SerializeField] UI.Text _minCutoffLabel = null;

    OneEuroFilter2 _filter = new OneEuroFilter2();
    RectTransform _xform;

    void Start()
      => _xform = GetComponent<RectTransform>();

    void Update()
    {
        _filter.Beta = _betaSlider.value;
        _filter.MinCutoff = _minCutoffSlider.value;

        _betaLabel.text = $"{_filter.Beta:0.0000}";
        _minCutoffLabel.text = $"{_filter.MinCutoff:0.00}";

        _xform.anchoredPosition =
          _filter.Step(Time.time, _target.anchoredPosition);
    }
}
