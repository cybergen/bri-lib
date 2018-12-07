using System;

public interface IShowable
{
  void OnAwake();
  void Show(Action onShown = null, Action onCancelled = null);
  void Hide(Action onHidden = null, Action onCancelled = null);
}
