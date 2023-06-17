using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;


public class ArOriginScaleManager : MonoBehaviour
{
    public ARSessionOrigin m_ARSessionOrigin;

    public Slider scaleSlider;


    private void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnsliderValueChanged);
    }
    private void Awake()
    {
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
    }

    public void OnsliderValueChanged(float value)
    {
        if (scaleSlider!=null)
        {
            m_ARSessionOrigin.transform.localScale = Vector3.one / value;
        }
    }
}
