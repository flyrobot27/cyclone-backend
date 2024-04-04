namespace CYCLONE.Template
{
	using CYCLONE.Template.Model.Element;
	using Simphony.Mathematics;
	public class Combi(string id, string description, Distribution duration, IList<IElement> followers, IList<Queue> preceders) : Normal(id, description, duration, followers)
	{
		private IList<Queue> preceders = preceders;

		public bool TryExecute()
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
