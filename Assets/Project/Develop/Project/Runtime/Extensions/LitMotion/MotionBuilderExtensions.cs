using System;
using LitMotion;

namespace Runtime.Extensions.LitMotion
{
	internal static class MotionBuilderExtensions
	{
		internal static (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) AddDelay<TObject, TValue, TOptions, TAdapter>(this (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) builder, Single delay, DelayType delayType = DelayType.FirstLoop, Boolean isSkipValuesDuringDelay = true) where TValue : unmanaged where TOptions : unmanaged, IMotionOptions where TAdapter : unmanaged, IMotionAdapter<TValue, TOptions>
		{
			builder.MotionBuilder.WithDelay(delay, delayType, isSkipValuesDuringDelay);

			return builder;
		}

		internal static (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) InvokeAfterCompletion<TObject, TValue, TOptions, TAdapter>(this (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) builder, Action completionAction) where TValue : unmanaged where TOptions : unmanaged, IMotionOptions where TAdapter : unmanaged, IMotionAdapter<TValue, TOptions>
		{
			builder.MotionBuilder.WithOnComplete(completionAction);

			return builder;
		}

		internal static (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) AddEase<TObject, TValue, TOptions, TAdapter>(this (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) builder, Ease ease) where TValue : unmanaged where TOptions : unmanaged, IMotionOptions where TAdapter : unmanaged, IMotionAdapter<TValue, TOptions>
		{
			builder.MotionBuilder.WithEase(ease);

			return builder;
		}
		internal static (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) WithOnComplete<TObject, TValue, TOptions, TAdapter>(this (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) builder, Action completionAction) where TValue : unmanaged where TOptions : unmanaged, IMotionOptions where TAdapter : unmanaged, IMotionAdapter<TValue, TOptions>
		{
			builder.MotionBuilder.WithOnComplete(completionAction);

			return builder;
		}
		
		internal static (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) WithLoops<TObject, TValue, TOptions, TAdapter>(this (TObject SystemObject, MotionBuilder<TValue, TOptions, TAdapter> MotionBuilder) builder, Int32 countRepeat = 1, LoopType loopType = LoopType.Restart) where TValue : unmanaged where TOptions : unmanaged, IMotionOptions where TAdapter : unmanaged, IMotionAdapter<TValue, TOptions>
		{
			builder.MotionBuilder.WithLoops(countRepeat,loopType);

			return builder;
		}

	}
}