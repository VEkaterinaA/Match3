using Cysharp.Threading.Tasks;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using System.IO;
using UnityEngine;

namespace Runtime.Infrastructure.Services.SaveProgressServices
{
	internal sealed class FileSystemService : IFileSystemService
	{
		async UniTask IFileSystemService.SaveAsync(String data, String fullPath)
		{
			try
			{
				var directoryPath = Path.GetDirectoryName(fullPath);

				if (directoryPath != null)
				{
					Directory.CreateDirectory(directoryPath);
				}
				else
				{
					throw new IOException($"[{GetType().Name}] Invalid directory path: {fullPath}.");
				}

				await using var fileStream = new FileStream(fullPath, FileMode.Create);
				await using var streamWriter = new StreamWriter(fileStream);

				await streamWriter.WriteAsync(data);
			}
			catch (Exception exception)
			{
				Debug.LogError($"[{GetType().Name}] Error occured when trying to save data to file {fullPath}\n{exception}");
			}
		}

		void IFileSystemService.Save(String data, String fullPath)
		{
			try
			{
				var directoryPath = Path.GetDirectoryName(fullPath);

				if (directoryPath != null)
				{
					Directory.CreateDirectory(directoryPath);
				}
				else
				{
					throw new IOException($"[{GetType().Name}] Invalid directory path: {fullPath}.");
				}

				using var fileStream = new FileStream(fullPath, FileMode.Create);
				using var streamWriter = new StreamWriter(fileStream);

				streamWriter.Write(data);
			}
			catch (Exception exception)
			{
				Debug.LogError($"[{GetType().Name}] Error occured when trying to save data to file {fullPath}\n{exception}");
			}
		}

		void IFileSystemService.Save(Byte[] data, String fullPath)
		{
			try
			{
				var directoryPath = Path.GetDirectoryName(fullPath);

				if (directoryPath != null)
				{
					Directory.CreateDirectory(directoryPath);
				}
				else
				{
					throw new IOException($"[{GetType().Name}] Invalid directory path: {fullPath}.");
				}

				File.WriteAllBytes(fullPath, data);
			}
			catch (Exception exception)
			{
				Debug.LogError($"[{GetType().Name}] Error occured when trying to save data to file {fullPath}\n{exception}");
			}
		}

		void IFileSystemService.Delete(String fullPath)
		{
			try
			{
				var directoryPath = Path.GetDirectoryName(fullPath);

				if (directoryPath != null)
				{
					Directory.CreateDirectory(directoryPath);
				}
				else
				{
					throw new IOException($"[{GetType().Name}] Invalid directory path: {fullPath}.");
				}

				File.Delete(fullPath);
			}
			catch (Exception exception)
			{
				Debug.LogError($"[{GetType().Name}] Error occured when trying to delete file {fullPath}\n{exception}");
			}
		}

		Texture2D IFileSystemService.LoadTexture2D(String fullPath)
		{
			try
			{
				var texture2D = new Texture2D(960, 540, TextureFormat.ARGB32, false);

				texture2D.LoadRawTextureData(File.ReadAllBytes(fullPath));
				texture2D.Apply();

				return texture2D;
			}
			catch (Exception exception)
			{
				Debug.LogError($"[{GetType().Name}] Error occured when trying to load data from file {fullPath}\n{exception}");
			}

			return null;
		}

		TObject IFileSystemService.LoadFromJSON<TObject>(String fullPath, String encryptionKey)
		{
			if (File.Exists(fullPath))
			{
				try
				{
					using var fileStream = new FileStream(fullPath, FileMode.Open);
					using var streamReader = new StreamReader(fileStream);

					var data = streamReader.ReadToEnd();

					return JsonUtility.FromJson<TObject>(encryptionKey.IsNullOrEmpty() ? data : data.Encrypt(encryptionKey));
				}
				catch (Exception exception)
				{
					Debug.LogError($"[{GetType().Name}] Error occured when trying to load data from file {fullPath}\n{exception}");
				}
			}

			return null;
		}
	}
}
