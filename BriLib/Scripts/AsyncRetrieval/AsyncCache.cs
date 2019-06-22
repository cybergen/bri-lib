﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BriLib
{
  public class AsyncCache<K> : GOSingleton<AsyncCache<K>> where K : class
  {
    public int MaxRequests = 3;

    protected Func<WWW, K> _getResult;

    private Dictionary<string, AsyncToken<K>> _storedTokens = new Dictionary<string, AsyncToken<K>>();
    private int _requestsActive;

    public async Task<K> GetResult(string url)
    {
      if (string.IsNullOrEmpty(url)) return null;
      AsyncToken<K> token = null;

      if (_storedTokens.TryGetValue(url, out token)) return await token.GetResult();

      token = new AsyncToken<K>();
      _storedTokens.Add(url, token);
      await GetRequestReady();
      _requestsActive++;

      var request = new WWW(url);
      while (!request.isDone) await Task.Delay(50);

      if (!string.IsNullOrEmpty(request.error))
      {
        LogManager.Error("Encountered error during retrieval: " + request.error);
        return null;
      }
      _requestsActive--;
      if (_getResult == null)
      {
        LogManager.Error("Get result function undefined for AsyncCache of type: " + typeof(K));
        return null;
      }
      token.Result = _getResult(request);

      return token.Result;
    }

    private async Task GetRequestReady()
    {
      while (_requestsActive >= MaxRequests) await Task.Delay(100);
    }
  }
}