namespace Runtime.Infrastructure.Core
{
	public interface IPrototype<out TObject>
	{
		public TObject Clone();
	}
}