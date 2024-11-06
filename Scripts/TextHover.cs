using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator textAnimator;

    void Start()
    {
        if (textAnimator == null)
        {
            textAnimator = GetComponentInChildren<Animator>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textAnimator.SetBool("isHovered", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textAnimator.SetBool("isHovered", false);
    }

    public void ResetHoverState()
    {
        textAnimator.SetBool("isHovered", false);
    }
}

