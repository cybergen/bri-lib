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
      _onFinish.Execute();
      Cleanup();
    }

    protected void Fail()
    {
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