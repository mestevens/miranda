namespace Mestevens.Injection.Extensions
{

	public abstract class Command
	{

		public abstract void MapParameters(params object[] parameters);

		public abstract void Execute();

	}

}