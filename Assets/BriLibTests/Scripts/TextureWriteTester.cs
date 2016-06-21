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

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var point = Input.mousePosition;
            var hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(point), out hit, Mathf.Infinity))
            {
                var uv = hit.textureCoord;
                var x = uv.x * Width;
                var y = uv.y * Height;
                OnMouseClick((int)x, (int)y);
                UpdateTexture();
            }
        }
    }

    protected virtual void OnMouseClick(int x, int y) { }

    protected virtual void DrawBackground()
    {
        if (_texture == null) return;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _texture.SetPixel(x, y, BgColor);
            }
        }
    }

    protected virtual void DrawCircle(float centerX, float centerY, float radius, Color color)
    {
        for (float angle = 0; angle < 360; angle += 0.2f)
        {
            var rad = angle * System.Math.PI / 180f;
            var x = System.Math.Cos(rad) * radius + centerX;
            var y = System.Math.Sin(rad) * radius + centerY;
            _texture.SetPixel((int)x, (int)y, color);
        }
    }

    protected virtual void DrawLine(double startX, double startY, double endX, double endY, float width, Color color)
    {
        var start = new Vector3((float)startX, 0f, (float)startY);
        var end = new Vector3((float)endX, 0f, (float)endY);
        var dir = end - start;
        var length = dir.magnitude;
        dir.Normalize();

        var cross = Vector3.Cross(dir, new Vector3(0f, 1f, 0f));

        for (float i = 0; i < length; i++)
        {
            var point = start + dir * i;
            for (float j = 0; j < width / 2; j++)
            {
                var left = point + cross * j;
                var right = point - cross * j;
                _texture.SetPixel((int)left.x, (int)left.z, color);
                _texture.SetPixel((int)right.x, (int)right.z, color);
            }
        }
    }

    protected virtual void UpdateTexture()
    {
        if (_texture == null) return;

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
