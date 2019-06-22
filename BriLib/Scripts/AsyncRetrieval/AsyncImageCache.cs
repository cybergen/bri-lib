using UnityEngine;

namespace BriLib
{
  public class AsyncImageCache : AsyncCache<Texture>
  {
    public override void OnCreate()
    {
      base.OnCreate();
      _getResult = GetResult;
    }

    private Texture GetResult(WWW www)
    {
      return www.texture;
    }
  }
}