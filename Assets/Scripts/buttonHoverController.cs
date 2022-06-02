using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Used for buttons to capture the pointer events and perform an expecific task.
/// </summary>
public class buttonHoverController : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    //events
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.playBuuttonHoverSound();
        GameManager.Instance.setHandCursor();
        playZoomAnimation();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.setDefaultCursor();
        playIdleAnim();
    }
    //methods
    void playZoomAnimation()
    {
        if (_animator == null)
            return;
        _animator.Play(GameManager.ANIMATION_TAG);
    }
    void playIdleAnim()
    {
        if (_animator == null)
            return;
        _animator.Play(GameManager.IDLE_ANIMATION_TAG);
    }
    //mono
    private void OnBecameVisible()
    {
        playIdleAnim();
    }
    private void OnBecameInvisible()
    {
        playIdleAnim();
    }
}
