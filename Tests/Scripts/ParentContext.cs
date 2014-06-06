using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Mestevens.Injection.Core;

public class ParentContext : Context {

	public ParentContext() : base() {
	}

	public override void MapBindings() {

		Bind<TestClass>().To<TestAnnotation>().ToSingleton();
		Bind<IObjectInject>().To<ObjectInjectImpl>();

		Bind<string>().To("here is a string").Named("string.name");
		Bind<string>().To("here is another named string").Named("another.string");

		Bind<string>().To("Injeception").Named("inject.inject");

		//Bind<TestCommand>().To<TestCommand>();
		BindSignal<TestSignal>().ToCommand<TestCommand>();
		Bind<ExceptionSignal>().To<ExceptionSignal>().ToSingleton();
		//Bind<TestSignal>().To<TestSignal>().ToSingleton();
	}

}