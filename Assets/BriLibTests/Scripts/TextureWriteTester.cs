using UnityEngine;

public class TextureWriteTester : MonoBehaviour
{
    public int Width;
    public int Height;
    public Material Material;
    public Color BgColor;

    protected Texture2D _texture;

    protected virtual void Initialize()
    {
        _texture = new Texture2D(Width, Height, TextureFormat.RGB24, false) { wrapMode = TextureWrapMode.Clamp };
        DrawBackground();

    }

    protected virtual void DrawBackground()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _texture.SetPixel(x, y, BgColor);
            }
        }
    }

    protected virtual void UpdateTexture()
    {
        _texture.Apply();
        Material.SetTexture("_MainTex", _texture);
    }

    protected virtual void DrawPoint(int x, int y, int size, Color pointColor)
    {
        var startX = (int)Mathf.Max(0, x- size / 2);
        var endX = (int)Mathf.Min(Width, x + size / 2);
        var startY = (int)Mathf.Max(0, y - size / 2);
        var endY = (int)Mathf.Min(Height, y + size / 2);

        for (int currentY = startY; currentY <= endY; currentY++)
        {
            for (int currentX = startX; currentX <= endX; currentX++)
            {
                _texture.SetPixel(currentX, currentY, pointColor);
            }
        }
    }
}
