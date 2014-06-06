using UnityEngine;
using System.Collections;

using NUnit.Framework;

using Mestevens.Injection.Core.Impl;
using Mestevens.Injection.Core.Exceptions.Impl;

using Mestevens.Injection.Extensions;

namespace Mestevens.Injection.Core
{

	[TestFixture]
	public class MirandaTest {

		#region Constants

		private const string PRINTER_STRING = "Printer String.";
		private const int PRINTER_INT = 8372;
		private const string WRITER_STRING = "Writer String.";
		private const int WRITER_INT = 7537;
		private const string READER_STRING = "Reader String.";
		private const int READER_INT = 4528;
		private const bool READER_BOOL = false;

		private const string STRING_BINDING = "Here is a string binding";
		private const int INT_BINDING = 2034;

		private const string SINGLETON_STRING = "Singleton String.";

		#endregion

		#region Members

		private Context context;

		#endregion

		#region Test Interfaces

		public interface IPrinter {

			string ReturnString();

			int ReturnInt();

		}

		public interface IWriter {

			string WriteString();

			int WriteInt();

		}

		public interface IReader {

			string ReadString();

			int ReadInt();

			bool ReadBool();

		}

		public interface ISingleton {

			void ChangeString(string newString);

			string GetString();

		}

		public interface ITest {

			void AssertPassOrFail();

		}

		#endregion

		#region Test Interface Implementations

		public class Printer : IPrinter {

			public string ReturnString() {
				return PRINTER_STRING;
			}

			public int ReturnInt() {
				return PRINTER_INT;
			}

		}

		public class Writer : IWriter {
			
			public string WriteString() {
				return WRITER_STRING;
			}
			
			public int WriteInt() {
				return WRITER_INT;
			}
			
		}

		public class Reader : IReader {
			
			public string ReadString() {
				return READER_STRING;
			}
			
			public int ReadInt() {
				return READER_INT;
			}
			
			public bool ReadBool() {
				return READER_BOOL;
			}
			
		}

		public class Singleton : ISingleton {

			public string singletonString;

			public Singleton() {
				singletonString = SINGLETON_STRING;
			}

			public void ChangeString(string newString) {
				singletonString = newString;
			}

			public string GetString() {
				return singletonString;
			}

		}

		#endregion

		#region Signals and Commands

		public class TestSignal : Signal {

		}

		public class TestCommand : Command {

			public IReader reader;
			public ITest test;

			[Inject]
			public IWriter Writer { get; set; }

			[Inject]
			public TestCommand(IReader reader, [Named("constructor.inject.class")] ITest test) {
				this.reader = reader;
				this.test = test;
			}

			public override void MapParameters(params object[] parameters) {

			}
			
			public override void Execute() {
				Assert.AreEqual(WRITER_STRING, this.Writer.WriteString());
				Assert.AreEqual(WRITER_INT, this.Writer.WriteInt());
				Assert.AreEqual(READER_STRING, this.reader.ReadString());
				Assert.AreEqual(READER_INT, this.reader.ReadInt());
				Assert.AreEqual(READER_BOOL, this.reader.ReadBool());
				Assert.NotNull(test);
				test.AssertPassOrFail();
				Assert.Pass();
			}

		}

		#endregion

		#region Injection Classes

		public class EmptyClass {

			public string className = "EmptyClass";

		}

		public class EmptyClassWithDefaultConstructor {

			public string className;

			public EmptyClassWithDefaultConstructor() {
				className = "EmptyClassWithDefaultConstructor";
			}

		}

		public class NoBindingInjectConstructor {

			public IWriter writer;

			[Inject]
			public NoBindingInjectConstructor(IWriter writer) {
				this.writer = writer;
			}

		}

		public class PropertiesInjectClass : ITest {

			[Inject]
			public IPrinter Printer { get; set; }

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.Printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.Printer.ReturnInt());
			}

		}

		public class MembersInjectClass : ITest {

			[Inject]
			public IPrinter printer;

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.printer.ReturnInt());
			}

		}

		public class PropertiesMembersInjectClass : ITest {

			[Inject]
			public IPrinter printer;

			[Inject]
			public IWriter Writer { get; set; }

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.printer.ReturnInt());
				Assert.AreEqual(WRITER_STRING, this.Writer.WriteString());
				Assert.AreEqual(WRITER_INT, this.Writer.WriteInt());
			}

		}

		public class ConstructorInjectClass : ITest {

			private IPrinter printer;

			[Inject]
			public ConstructorInjectClass(IPrinter printer) {
				this.printer = printer;
			}

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.printer.ReturnInt());
			}

		}

		public class ConstructorPropertiesInjectClass : ITest {

			public IWriter writer;

			[Inject]
			public IPrinter Printer { get; set; }

			[Inject]
			public ConstructorPropertiesInjectClass(IWriter writer) {
				this.writer = writer;
			}

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.Printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.Printer.ReturnInt());
				Assert.AreEqual(WRITER_STRING, this.writer.WriteString());
				Assert.AreEqual(WRITER_INT, this.writer.WriteInt());
			}

		}

		public class ConstructorMembersInjectClass : ITest {

			[Inject]
			public IWriter writer;

			public IPrinter printer;

			[Inject]
			public ConstructorMembersInjectClass(IPrinter printer) {
				this.printer = printer;
			}

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.printer.ReturnInt());
				Assert.AreEqual(WRITER_STRING, this.writer.WriteString());
				Assert.AreEqual(WRITER_INT, this.writer.WriteInt());
			}

		}

		public class ConstructorPropertiesMembersInjectClass : ITest {

			[Inject]
			public IWriter writer;

			public IReader reader;

			[Inject]
			public IPrinter Printer { get; set; }

			[Inject]
			public ConstructorPropertiesMembersInjectClass(IReader reader) {
				this.reader = reader;
			}

			public void AssertPassOrFail() {
				Assert.AreEqual(PRINTER_STRING, this.Printer.ReturnString());
				Assert.AreEqual(PRINTER_INT, this.Printer.ReturnInt());
				Assert.AreEqual(WRITER_STRING, this.writer.WriteString());
				Assert.AreEqual(WRITER_INT, this.writer.WriteInt());
				Assert.AreEqual(READER_STRING, this.reader.ReadString());
				Assert.AreEqual(READER_INT, this.reader.ReadInt());
				Assert.AreEqual(READER_BOOL, this.reader.ReadBool());
			}

		}

		#endregion

		#region Contexts

		public class MasterContext : Context {

			public MasterContext() : base() {

			}

			public override void MapBindings() {
				Bind<IPrinter>().To<Printer>();
				Bind<IWriter>().To<Writer>();
				Bind<IReader>().To<Reader>();

				//Named Binding
				Bind<ITest>().To<PropertiesInjectClass>().Named("properties.inject.class");
				Bind<ITest>().To<MembersInjectClass>().Named("members.inject.class");
				Bind<ITest>().To<PropertiesMembersInjectClass>().Named("properties.members.inject.class");
				Bind<ITest>().To<ConstructorInjectClass>().Named("constructor.inject.class");
				Bind<ITest>().To<ConstructorPropertiesInjectClass>().Named("constructor.properties.inject.class");
				Bind<ITest>().To<ConstructorMembersInjectClass>().Named("constructor.members.inject.class");
				Bind<ITest>().To<ConstructorPropertiesMembersInjectClass>().Named("constructor.properties.members.inject.class");

				//Value Bindings
				Bind<string>().To(STRING_BINDING).Named("string.binding");
				Bind<int>().To(INT_BINDING).Named("int.binding");

				//Singletons
				Bind<ISingleton>().To<Singleton>().ToSingleton();

				//Signals to commands
				BindSignal<TestSignal>().ToCommand<TestCommand>();
			}

		}

		public class SingletonClass {
			public string value;

			[Inject]
			public SingletonClass([Named("string.binding")] string value) {
				this.value = value;
			}
		}

		public class SingletonContext : Context {

			public SingletonContext() : base() {

			}

			public override void MapBindings() {
				Bind<SingletonClass>().ToSingleton();
			}

		}

		public class TestObject {

			public string Name { get; set; }

			public TestObject() {
				Name = "Default constructor";
			}

		}

		public class ObjectContext : Context {

			public ObjectContext() : base() {

			}

			public override void MapBindings() {
				TestObject tO = new TestObject ();
				tO.Name = "A changed name";
				Bind<TestObject>().To(tO);
			}

		}

		public class BadContext : Context {

			public BadContext() : base() {

			}

			public override void MapBindings() {
				//Not binding printer, writer, or reader should cause an initialization exception

				//Named Binding
				Bind<ITest>().To<PropertiesInjectClass>().Named("properties.inject.class");
				Bind<ITest>().To<MembersInjectClass>().Named("members.inject.class");
				Bind<ITest>().To<PropertiesMembersInjectClass>().Named("properties.members.inject.class");
				Bind<ITest>().To<ConstructorInjectClass>().Named("constructor.inject.class");
				Bind<ITest>().To<ConstructorPropertiesInjectClass>().Named("constructor.properties.inject.class");
				Bind<ITest>().To<ConstructorMembersInjectClass>().Named("constructor.members.inject.class");
				Bind<ITest>().To<ConstructorPropertiesMembersInjectClass>().Named("constructor.properties.members.inject.class");
				
				//Value Bindings
				Bind<string>().To(STRING_BINDING).Named("string.binding");
				Bind<int>().To(INT_BINDING).Named("int.binding");
			}

		}

		#endregion

		[SetUp]
		public void SetUp() {
			context = Miranda.Init(new MasterContext());
		}

		[TearDown]
		public void TearDown() {
			context = null;
		}

		[Test]
		public void MirandaGetWithNoBindingAndNoConstructorTest() {
			EmptyClass emptyClass = context.Get<EmptyClass>();
			Assert.NotNull(emptyClass);
			Assert.AreEqual("EmptyClass", emptyClass.className);
		}

		[Test]
		public void MirandaGetWithNoBindingAndAConstructorTest() {
			EmptyClassWithDefaultConstructor emptyClass = context.Get<EmptyClassWithDefaultConstructor>();
			Assert.NotNull(emptyClass);
			Assert.AreEqual("EmptyClassWithDefaultConstructor", emptyClass.className);
		}

		[Test]
		public void MirandaGetWithNoBindingInjectConstructorTest() {
			NoBindingInjectConstructor noBindingClass = context.Get<NoBindingInjectConstructor>();
			Assert.NotNull(noBindingClass);
			Assert.NotNull(noBindingClass.writer);
			Assert.AreEqual(WRITER_STRING, noBindingClass.writer.WriteString());
			Assert.AreEqual(WRITER_INT, noBindingClass.writer.WriteInt());
		}

		[Test]
		public void MirandaGetNoConstructorNoPropertiesNoMembersTest() {
			IPrinter printer = context.Get<IPrinter>();
			Assert.NotNull(printer);
			Assert.AreEqual(PRINTER_STRING, printer.ReturnString());
			Assert.AreEqual(PRINTER_INT, printer.ReturnInt());
		}
		
		[Test]
		public void MirandaGetNoConstructorAndPropertiesNoMembersTest() {
			ITest test = context.Get<ITest>("properties.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}
		
		[Test]
		public void MirandaGetNoConstructorAndMembersNoPropertiesTest() {
			ITest test = context.Get<ITest>("members.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}

		[Test]
		public void MirandaGetNoConstructorWithMembersAndPropertiesTest() {
			ITest test = context.Get<ITest>("properties.members.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}

		[Test]
		public void MirandaGetWithConstructorNoPropertiesOrMembersTest() {
			ITest test = context.Get<ITest>("constructor.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}

		[Test]
		public void MirandaGetWithConstructorAndPropertiesNoMembersTest() {
			ITest test = context.Get<ITest>("constructor.properties.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}

		[Test]
		public void MirandaGetWithConstructorAndMembersNoPropertiesTest() {
			ITest test = context.Get<ITest>("constructor.members.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}

		[Test]
		public void MirandaGetWithConstructorPropertiesMembersTest() {
			ITest test = context.Get<ITest>("constructor.properties.members.inject.class");
			Assert.NotNull(test);
			test.AssertPassOrFail();
		}

		[Test]
		public void MirandaGetSingletonTest() {
			ISingleton singleton = context.Get<ISingleton>();
			Assert.AreEqual(SINGLETON_STRING, singleton.GetString());
			string anotherString = "Change to this string.";
			singleton.ChangeString(anotherString);

			ISingleton anotherInstance = context.Get<ISingleton>();
			Assert.AreNotEqual(SINGLETON_STRING, anotherInstance.GetString());
			Assert.AreEqual(anotherString, anotherInstance.GetString());
		}

		[Test]
		public void MirandaGetValueTest() {
			string stringValue = context.Get<string>("string.binding");
			Assert.AreEqual(STRING_BINDING, stringValue);
			int intValue = context.Get<int>("int.binding");
			Assert.AreEqual(INT_BINDING, intValue);
		}

		[Test]
		public void MirandaGetSignalTest() {
			TestSignal testSignal = context.Get<TestSignal>();
			Assert.NotNull(testSignal);
		}

		[Test]
		public void MirandaGetCommandTest() {
			TestSignal testSignal = context.Get<TestSignal>();
			Assert.NotNull(testSignal);
			testSignal.Dispatch();
			Assert.Fail();
		}

		[Test]
		[ExpectedException(typeof(BindingNotFoundException))]
		public void MirandaInitializeFailure() {
			Miranda.Init(new BadContext());
		}

		[Test]
		public void MirandaSingletonBindingTest() {
			context = Miranda.Init(new MasterContext(), new SingletonContext());
			SingletonClass singletonClass = context.Get<SingletonClass>();
			Assert.AreEqual(STRING_BINDING, singletonClass.value);
			singletonClass.value = "New Value";
			SingletonClass newSingleton = context.Get<SingletonClass>();
			Assert.AreEqual("New Value", newSingleton.value);
		}

		//Should be the same as binding to Singleton (i.e. binding to an instance is the same)
		[Test]
		public void MirandaObjectBindingTest() {
			context = Miranda.Init(new ObjectContext());
			TestObject testObject = context.Get<TestObject>();
			Assert.AreEqual("A changed name", testObject.Name);
			testObject.Name = "Changed again";
			TestObject newTestObject = context.Get<TestObject>();
			Assert.AreEqual("Changed again", newTestObject.Name);
		}

	}

}