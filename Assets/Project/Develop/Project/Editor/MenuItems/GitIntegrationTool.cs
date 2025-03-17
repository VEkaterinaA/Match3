using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HiddenLightEditor.MenuItems
{
    [InitializeOnLoad]
    internal sealed class GitIntegrationTool
    {
        private const Int32 _version = 1;

        private static readonly String _versionKey = $"{_version}_{Application.unityVersion}";

        static GitIntegrationTool()
        {
            var versionKey = EditorPrefs.GetString("GitIntegrator");

            if (versionKey != _versionKey)
            {
                SmartMergeRegister();
            }
        }

        [MenuItem("Tools/Git/Set UnityYAMLMerge")]
        private static void SmartMergeRegister()
        {
            try
            {
                return;
                var UnityYAMLMergePath = EditorApplication.applicationContentsPath + "/Tools" + "/UnityYAMLMerge.exe";
                ExecuteGitWithParams("config merge.unityyamlmerge.name \"Unity SmartMerge (UnityYamlMerge)\"");
                ExecuteGitWithParams($"config merge.unityyamlmerge.driver \"\\\"{UnityYAMLMergePath}\\\" merge -h -p --force --fallback none %O %B %A %A\"");
                ExecuteGitWithParams("config merge.unityyamlmerge.recursive binary");
                EditorPrefs.SetString("GitIntegrator", _versionKey);
                Debug.Log($"[{nameof(GitIntegrationTool)}] Successfully registered UnityYAMLMerge with path {UnityYAMLMergePath}");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[{nameof(GitIntegrationTool)}] Fail to register UnityYAMLMerge with error: {exception}");
            }
        }

        private static void ExecuteGitWithParams(String param)
        {
            var processInfo = new ProcessStartInfo("git");

            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = Environment.CurrentDirectory;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;

            var process = new Process();
            process.StartInfo = processInfo;
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = param;

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(process.StandardError.ReadLine());
            }

            process.StandardOutput.ReadLine();
        }
    }
}