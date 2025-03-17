using System;
using UnityEngine;

namespace Runtime.MonoBehaviours
{
	public interface IInjectable
	{
	}

	[Obsolete("Use " + nameof(IInjectable) + " instead")]
	public abstract class InjectedBehaviour : MonoBehaviour, IInjectable
	{
	}
}
