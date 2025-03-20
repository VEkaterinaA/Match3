using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.Infrastructure.Services.SaveProgressServices.Core
{
	internal interface IFileSystemService
	{
		internal TObject LoadFromJSON<TObject>(String fullPath, String encryptionKey = null) where TObject : class;

		internal Texture2D LoadTexture2D(String fullPath);

		internal UniTask SaveAsync(String data, String fullPath);

		internal void Save(String data, String fullPath);

		internal void Save(Byte[] data, String fullPath);

		internal void Delete(String fullPath);

		internal UniTask<Boolean> ExistsAsync(String fullPath);

		internal UniTask<String> ReadAllTextAsync(String fullPath);

		internal UniTask WriteAllTextAsync(String data, String fullPath);
	}
}
