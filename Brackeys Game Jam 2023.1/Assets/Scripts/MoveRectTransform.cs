using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRectTransform : MonoBehaviour
{
    private RectTransform rectTransform;
    public float bottom;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ChangeYPosition(float top)
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, top);
    }
}
