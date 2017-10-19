using System;

namespace BriLib
{
  public class AsyncTask
  {
    public bool Finished { get { return _disposed; } }

    private Action _onFinish;
    private Action _onFailed;
    private bool _disposed;

    public void SetCallbacks(Action onFinish, Action onFailed)
    {
      _onFinish = onFinish;
      _onFailed = onFailed;
    }

    protected void Finish()
    {
      UnityEngine.Debug.Log("Calling OnFinish on task: " + this.GetType());
      _onFinish.Execute();
      Cleanup();
    }

    protected void Fail()
    {
      UnityEngine.Debug.Log("Calling OnFail on task: " + this.GetType());
      _onFailed.Execute();
      Cleanup();
    }

    public virtual void Tick(float delta)
    {
      if (_disposed)
      {
        throw new Exception("AsyncTask already disposed");
      }
    }

    protected virtual void Cleanup()
    {
      _onFinish = null;
      _onFailed = null;
      _disposed = true;
    }
  }
}