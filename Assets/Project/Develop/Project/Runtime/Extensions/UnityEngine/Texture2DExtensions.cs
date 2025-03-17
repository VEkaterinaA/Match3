using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
    internal static class Texture2DExtensions
    {
        internal static Texture2D Copy(this Texture2D texture2D)
        {
            var renderTexture = RenderTexture.GetTemporary(texture2D.width, texture2D.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);

            Graphics.Blit(texture2D, renderTexture);
            var previous = RenderTexture.active;

            RenderTexture.active = renderTexture;

            var readableTexture = new Texture2D(texture2D.width, texture2D.height);
            readableTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            readableTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);

            return readableTexture;
        }
    }
}