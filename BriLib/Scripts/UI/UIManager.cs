using UnityEngine;

namespace BriLib
{
  /// <summary>
  /// Primary entry point for all UI. Used to show and hide panels placed at different
  /// onscreen regions.
  /// </summary>
  public class UIManager : Singleton<UIManager>
  {
    public static Region Header { get { return Instance._header; } }
    public static Region Footer { get { return Instance._footer; } }
    public static Region Body { get { return Instance._body; } }
    public static Region Overlay { get { return Instance._overlay; } }

    [SerializeField] private Region _header;
    [SerializeField] private Region _footer;
    [SerializeField] private Region _body;
    [SerializeField] private Region _overlay;
    [SerializeField] private RectTransform _regionContainer;
    [SerializeField] private GameObject _interactionBlocker;

    /// <summary>
    /// Controls an interaction blocker that will intercept touch events
    /// </summary>
    /// <param name="interactable"></param>
    public static void SetInteractable(bool interactable)
    {
      Instance._interactionBlocker.SetActive(!interactable);
    }

    /// <summary>
    /// Tell all regions to hide their current screens
    /// </summary>
    public static void HideAll()
    {
      Header.HidePanel();
      Footer.HidePanel();
      Body.HidePanel();
      Overlay.HidePanel();
    }

    /// <summary>
    /// On initialization, UIManager will update positions/sizes of main canvas based on safeArea for notched devices
    /// target device
    /// </summary>
    public override void Begin()
    {
      base.Begin();

      //https://github.com/Goropocha/UniSafeAreaAdjuster/blob/master/UniSafeAreaAdjuster/Assets/UniSafeAreaAdjuster/SafeAreaAdjuster.cs
      var safeArea = Screen.safeArea;
      var screenSize = new Vector2(Screen.width, Screen.height);
      var anchorMin = safeArea.position;
      var anchorMax = safeArea.position + safeArea.size;
      anchorMin.x /= screenSize.x;
      anchorMin.y /= screenSize.y;
      anchorMax.x /= screenSize.x;
      anchorMax.y /= screenSize.y;

      _regionContainer.anchorMin = anchorMin;
      _regionContainer.anchorMax = anchorMax;
    }
  }
}