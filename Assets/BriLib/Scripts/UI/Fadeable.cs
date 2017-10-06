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
    }

    public void Show(Action onFinish = null, Action onCancel = null)
    {
      easer.SetEase(EaseWrapper.Direction.Forward, onFinish, onCancel);
    }

    public void Hide(Action onFinish = null, Action onCancel = null)
    {
      easer.SetEase(EaseWrapper.Direction.Backward, onFinish, onCancel);
    }

    private void Update()
    {
      easer.Tick(Time.deltaTime);
    }
  }
}