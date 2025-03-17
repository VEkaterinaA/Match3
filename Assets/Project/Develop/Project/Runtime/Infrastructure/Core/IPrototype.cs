namespace Runtime.Infrastructure.Core
{
	internal interface IPrototype<out TObject>
	{
		internal TObject Clone();
	}
}