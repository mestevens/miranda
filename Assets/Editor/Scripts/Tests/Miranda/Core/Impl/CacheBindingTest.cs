using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace Mestevens.Injection.Core.Impl
{

	[TestFixture]
	public class CacheBindingTest {

		public const string DEFAULT_MESSAGE = "Default Constructor";
		public const string PARAM_MESSAGE = "String Constructor";
		public const int DEFAULT_NUM = 1876;
		public const int PARAM_NUM = 9863;
		public const bool BINDING_VALUE = false;

		public CachedBinding cachedBinding;
		public TestModel testModel;

		public class TestModel {

			public string Message { get; set; }
			public int Num { get; set; }

			public TestModel() {
				this.Message = DEFAULT_MESSAGE;
				this.Num = DEFAULT_NUM;
			}

			public TestModel(string message) {
				this.Message = message;
				this.Num = DEFAULT_NUM;
			}

			public TestModel(string message, int num) {
				this.Message = message;
				this.Num = num;
			}

		}

		[SetUp]
		public void SetUp() {

		}

		[TearDown]
		public void TearDown() {
			cachedBinding = null;
			testModel = null;
		}

		[Test]
		public void ActivateWithNoParamsTest() {
			cachedBinding = new CachedBinding(typeof(TestModel));
			testModel = (TestModel)cachedBinding.Activate();

			Assert.AreEqual(DEFAULT_MESSAGE, testModel.Message);
			Assert.AreEqual(DEFAULT_NUM, testModel.Num);
		}

		[Test]
		public void ActivateWithParamsTest() {
			object[] paramArray = new object[1];
			paramArray[0] = PARAM_MESSAGE;
			cachedBinding = new CachedBinding(typeof(TestModel), paramArray, "");
			testModel = (TestModel)cachedBinding.Activate();

			Assert.AreEqual(PARAM_MESSAGE, testModel.Message);
			Assert.AreEqual(DEFAULT_NUM, testModel.Num);
		}

		[Test]
		public void ActivateWithMultipleParamsTest() {
			object[] paramArray = new object[2];
			paramArray[0] = PARAM_MESSAGE;
			paramArray[1] = PARAM_NUM;
			cachedBinding = new CachedBinding(typeof(TestModel), paramArray, "");
			testModel = (TestModel)cachedBinding.Activate();

			Assert.AreEqual(PARAM_MESSAGE, testModel.Message);
			Assert.AreEqual(PARAM_NUM, testModel.Num);
		}

		[Test]
		public void ActivateWithValue() {
			object value = BINDING_VALUE;
			cachedBinding = new CachedBinding(typeof(TestModel), value);

			Assert.AreEqual(BINDING_VALUE, cachedBinding.Activate());
		}

	}

}