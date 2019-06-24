using UnityEngine;

namespace BriLib
{
  public class AsyncImageCache : AsyncCache<Texture>
  {
    public override void OnCreate()
    {
      base.OnCreate();
      LogManager.Info("On create for image cache");
      _getResult = GetResult;
    }

    private Texture GetResult(WWW www)
    {
      LogManager.Info("Got texture off of www obj");
      return www.texture;
    }
  }
}