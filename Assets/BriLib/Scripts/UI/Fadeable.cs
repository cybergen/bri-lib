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
    public bool Hiding { get; private set; }

    private EaseWrapper easer;
    private Action<float> _onUpdate;

    private void Awake()
    {
      easer = new EaseWrapper(EaseDuration, StartValue, EndValue, EaseType, (a) => CG.alpha = a);
      CG.alpha = 0f;
      gameObject.SetActive(false);
    }

    public virtual void Show(Action onFinish = null, Action onCancel = null)
    {
      gameObject.SetActive(true);
      easer.SetEase(EaseWrapper.Direction.Forward, onFinish, onCancel);
      Hiding = false;
    }

    public virtual void Hide(Action onFinish = null, Action onCancel = null)
    {
      onFinish += () => gameObject.SetActive(false);
      easer.SetEase(EaseWrapper.Direction.Backward, onFinish, onCancel);
      Hiding = true;
    }

    public void Show()
    {
      Show(null, null);
    }

    public void Hide()
    {
      Hide(null, null);
    }

    protected virtual void Update()
    {
      easer.Tick(Time.deltaTime);
    }
  }
}
