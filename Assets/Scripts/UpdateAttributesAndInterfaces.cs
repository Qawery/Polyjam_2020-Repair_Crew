using System;


namespace BaseProject
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class UpdateFunction : Attribute
	{
		public UpdatePhases UpdatePhase { get; private set; }


		public UpdateFunction(UpdatePhases phase)
		{
			UpdatePhase = phase;
		}
	}


	public interface IUpdateBroadcaster
	{
		void AddUpdateSubscriber(object subscriberObject);
		void RemoveUpdateSubscriber(object subscriberObject);
	}
}