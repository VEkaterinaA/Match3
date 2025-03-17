using System;
using System.IO;
using UnityEngine;

namespace Runtime.Extensions.System.IO
{
    internal static class DirectoryInfoExtensions
    {
        internal static Boolean TryCreateDirectory(this DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Create();
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"[{nameof(DirectoryInfoExtensions)}] Failed to create {directoryInfo.Name} directory:\n{exception}");
                }
            }

            return true;
        }
    }
}