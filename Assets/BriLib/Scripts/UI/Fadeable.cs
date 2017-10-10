using UnityEngine;
using System;

namespace BriLib
{
  public class Fadeable : MonoBehaviour
  {
    public CanvasGroup CG;
    public float StartValue = 0f;
    public float EndValue = 1f;
    public float EaseDuration = 0.4f;
    public Easing.Method EaseType = Easing.Method.ExpoOut;

    private EaseWrapper easer;
    private Action<float> _onUpdate;

    private void Awake()
    {
      easer = new EaseWrapper(EaseDuration, StartValue, EndValue, EaseType, (a) => CG.alpha = a);
      CG.alpha = 0f;
      gameObject.SetActive(false);
    }

    public void Show(Action onFinish = null, Action onCancel = null)
    {
      gameObject.SetActive(true);
      easer.SetEase(EaseWrapper.Direction.Forward, onFinish, onCancel);
    }

    public void Hide(Action onFinish = null, Action onCancel = null)
    {
      onFinish += () => gameObject.SetActive(false);
      easer.SetEase(EaseWrapper.Direction.Backward, onFinish, onCancel);
    }

    public void Show()
    {
      Show(null, null);
    }

    public void Hide()
    {
      Hide(null, null);
    }

    private void Update()
    {
      easer.Tick(Time.deltaTime);
    }
  }
}