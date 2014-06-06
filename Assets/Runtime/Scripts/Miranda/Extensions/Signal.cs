using System;
using System.Threading;
using System.Collections.Generic;

using Mestevens.Injection.Core;
using Mestevens.Injection.Core.Api;

namespace Mestevens.Injection.Extensions
{

	public delegate void CommandAction(params object[] parameters);

	public abstract class Signal
	{

		private IList<Type> commands;
		private IList<CommandAction> onceMethods;

		[Inject]
		public IBinder injector;

		public Signal()
		{
			commands = new List<Type>();
			onceMethods = new List<CommandAction>();
		}

		public void AddOnce(CommandAction action)
		{
			onceMethods.Add(action);
		}

		public void AddCommand(Type type)
		{
			//Check if type is command
			if (type.BaseType.Equals(typeof(Command)))
			{
				commands.Add(type);
			}
		}

		public void Dispatch(params object[] parameters)
		{
			foreach(Type type in commands)
			{
				Command command = (Command)injector.Get(type);
				command.MapParameters(parameters);
				command.Execute();
			}
			foreach(CommandAction action in onceMethods)
			{
				action(parameters);
			}
			onceMethods.Clear();
		}

		public void DispatchAsync(params object[] parameters)
		{
			foreach(Type type in commands)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object t)
				                                              {
					Command command = (Command)injector.Get(type);
					command.MapParameters(parameters);
					command.Execute();
				}));
			}
			foreach(CommandAction action in onceMethods)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object t)
				                                              {
					action(parameters);
				}));
			}
			onceMethods.Clear();
		}
	}

}