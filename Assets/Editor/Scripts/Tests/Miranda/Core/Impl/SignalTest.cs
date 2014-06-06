using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Mestevens.Injection.Core.Api;

namespace Mestevens.Injection.Extensions
{

	[TestFixture]
	public class SignalTest {

		private const string PARAM_STRING = "Param String";
		private const int PARAM_INT = 32;

		private TestSignal testSignal;
		private CommandTest testCommand;
		private IBinder testBinder;

		public class TestSignal : Signal {

		}

		public class CommandTest : Command {

			public string paramString;
			public int paramNum;

			public override void MapParameters(params object[] parameters) {
				if (parameters.Length == 2) {
					if (parameters[0].GetType().Equals(typeof(string)) &&
					    parameters[1].GetType().Equals(typeof(int))) {
						paramString = (string)parameters[0];
						paramNum = (int)parameters[1];
					}
				}
			}
			
			public override void Execute() {
				Assert.AreEqual(PARAM_STRING, paramString);
				Assert.AreEqual(PARAM_INT, paramNum);
				Assert.Pass("Execute was called successfully.");
			}

		}

		[SetUp]
		public void SetUp() {
			testCommand = Substitute.For<CommandTest>();
			testBinder = Substitute.For<IBinder>();

			testSignal = new TestSignal();
			testSignal.injector = testBinder;
		}

		[TearDown]
		public void TearDown() {
			testSignal = null;
			testBinder = null;
			testCommand = null;
		}

		[Test]
		public void DispatchTest() {
			testSignal.AddCommand(typeof(CommandTest));
			testBinder.Get(typeof(CommandTest)).Returns(testCommand);

			testSignal.Dispatch();

			testBinder.Received().Get(typeof(CommandTest));
			testCommand.ReceivedWithAnyArgs().MapParameters(null);
			testCommand.Received().Execute();
		}

		[Test]
		public void DispatchAddOnceTest() {
			int count = 0;

			testSignal.AddOnce((x) => {
				count++;
			});

			testSignal.Dispatch();
			testSignal.Dispatch();

			Assert.True (count == 1);
		}

		[Test]
		public void DispatchWithParamsTest() {
			testCommand = new CommandTest();
			testSignal.AddCommand(typeof(CommandTest));
			testBinder.Get(typeof(CommandTest)).Returns(testCommand);
			
			testSignal.Dispatch(PARAM_STRING, PARAM_INT);
			
			Assert.Fail();
		}

	}

}