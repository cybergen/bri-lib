using UnityEngine;

namespace BriLib
{
  /// <summary>
  /// Primary entry point for all UI. Used to show and hide panels placed at different
  /// onscreen regions.
  /// </summary>
  public class UIManager : Singleton<UIManager>
  {
    public static float CanvasScaler { get; private set; }
    public static float ScreenWidth { get; private set; }
    public static Region Header { get { return Instance._header; } }
    public static Region Footer { get { return Instance._footer; } }
    public static Region Body { get { return Instance._body; } }
    public static Region Overlay { get { return Instance._overlay; } }

    [SerializeField] private Region _header;
    [SerializeField] private Region _footer;
    [SerializeField] private Region _body;
    [SerializeField] private Region _overlay;
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
    /// On initialization, UIManager will update positions/sizes of regions based on
    /// target device
    /// </summary>
    public override void Begin()
    {
      base.Begin();

      RectTransform rt = GetComponent<RectTransform>();
      CanvasScaler = rt.localScale.x;
      ScreenWidth = rt.sizeDelta.x;

      var targetHeaderSize = _header.Rect.rect.height + Screen.safeArea.y;
      _header.Rect.sizeDelta = new Vector2(_header.Rect.sizeDelta.x, targetHeaderSize);

      var targetFooterSize = _footer.Rect.rect.height + Screen.safeArea.y;
      _footer.Rect.sizeDelta = new Vector2(_footer.Rect.sizeDelta.x, targetFooterSize);

      // resize main panel area according to height of header/footer
      _body.Rect.offsetMin = new Vector2(_body.Rect.offsetMin.x, targetFooterSize);
      _body.Rect.offsetMax = new Vector2(_body.Rect.offsetMax.x, -targetHeaderSize);
    }
  }
}