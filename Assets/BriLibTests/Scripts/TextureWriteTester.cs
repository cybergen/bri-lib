using UnityEngine;
using BriLib;

public class TextureWriteTester : MonoBehaviour
{
    public int Width;
    public int Height;
    public Material Material;
    public Material LinRenderMat;
    public Color BgColor;
    public float LineWidth;
    public Color TriangleColor;

    protected Texture2D _texture;

    private void Awake()
    {
        UnityEngine.Camera.onPostRender += OnCameraPost;
    }

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

    protected virtual void DrawLine(Vector3 start, Vector3 end)
    {
        lineList.Add(new Tuple<Vector3, Vector3>(start, end));
    }

    private System.Collections.Generic.List<Tuple<Vector3, Vector3>> lineList = new System.Collections.Generic.List<Tuple<Vector3, Vector3>>();

    protected void ClearLines()
    {
        lineList.Clear();
    }

    private void OnCameraPost(Camera cam)
    {
        foreach (var line in lineList)
        {
            var start = line.ItemOne;
            var end = line.ItemTwo;

            GL.Begin(GL.QUADS);
            LinRenderMat.SetPass(0);
            GL.Color(TriangleColor);

            //Shift each end point further along their angle by half the line width to get our edges to line up
            var upward = (start - end);
            upward.Normalize();
            upward *= LineWidth / 2;
            start += upward;
            end -= upward;

            //Determine the direction to offset our vertices to add line width
            var cross = Vector3.Cross(start - end, Vector3.up);
            cross.Normalize();
            cross *= LineWidth / 2;

            //Generate a vertex for each point on the quad
            var leftTopEdge = start + cross;
            var rightTopEdge = start - cross;
            var leftBottomEdge = end + cross;
            var rightBottomEdge = end - cross;

            //Push vertex list to gpu
            GL.Vertex3(rightTopEdge.x, rightTopEdge.y, rightTopEdge.z);
            GL.Vertex3(leftTopEdge.x, leftTopEdge.y, leftTopEdge.z);
            GL.Vertex3(leftBottomEdge.x, leftBottomEdge.y, leftBottomEdge.z);
            GL.Vertex3(rightBottomEdge.x, rightBottomEdge.y, rightBottomEdge.z);

            GL.End();
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
        var startX = (int)Mathf.Max(0, x - size / 2);
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
