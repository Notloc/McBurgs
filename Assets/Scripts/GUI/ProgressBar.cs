using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] RectTransform wholeBar;
    [SerializeField] RectTransform progressRect;

    public void UpdateProgress(float progress)
    {
        progress = Mathf.Clamp(progress, 0f, 1f);

        Vector2 sizeDelta = progressRect.sizeDelta;
        sizeDelta.x = wholeBar.sizeDelta.x * progress;

        progressRect.sizeDelta = sizeDelta;
    }
}
