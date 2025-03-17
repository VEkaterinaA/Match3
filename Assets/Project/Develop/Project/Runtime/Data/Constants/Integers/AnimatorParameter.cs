using System;
using UnityEngine;

namespace Runtime.Constants.Integers
{
	public static class AnimatorParameter
	{
		public static readonly Int32 NormalizedClimbSpeed = Animator.StringToHash(nameof(NormalizedClimbSpeed));
		public static readonly Int32 MovementSpeed = Animator.StringToHash("MovementSpeed");
		public static readonly Int32 IsWallSliding = Animator.StringToHash("IsWallSliding");
		public static readonly Int32 VelocityX = Animator.StringToHash("VelocityX");
		public static readonly Int32 VelocityY = Animator.StringToHash("VelocityY");
		public static readonly Int32 CanClimgLedge = Animator.StringToHash("CanClimbLedge");
		public static readonly Int32 HasLantern = Animator.StringToHash("HasLantern");

		public static readonly Int32 IsGrounded = Animator.StringToHash("IsGrounded");
		public static readonly Int32 IsWalking = Animator.StringToHash("IsWalking");

		public static readonly Int32 OnHit = Animator.StringToHash("OnHit");
	}
}