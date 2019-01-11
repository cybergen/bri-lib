namespace BriLib
{
  public class DoAfterFramesTask : AsyncTask
  {
    private int _frameCount = int.MaxValue;

    public void SetDuration(int frameCount)
    {
      _frameCount = frameCount;
    }

    public override void Tick(float delta)
    {
      base.Tick(delta);
      _frameCount--;
      if (_frameCount <= 0) Finish();
    }

    public void ForceFail() { Fail(); }
  }
}
