using System;
using System.IO;
using UnityEditor;

namespace HiddenLightEditor.Extensions
{
    internal static class AssetDatabaseExtensions
    {
        internal static Boolean TryCreateFolder(String[] assetPath)
        {
            if (assetPath.Length > 2)
            {
                if (!TryCreateFolder(assetPath[0..^1]))
                {
                    return false;
                }
            }

            return (AssetDatabase.CreateFolder(Path.Combine(assetPath[0..^1]), assetPath[^1]) != null);
        }
    }
}