using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Mestevens.Injection.Core;

public class OtherContext : Context {
	
	public OtherContext() : base() {
	}
	
	public override void MapBindings() {

		Bind<ITest>().To<TestImpl>();
		Bind<ITest>().To<AnotherTestImpl>().Named("aname");
		Bind<int>().To(5);
		Bind<float>().To(6.5f);
		BindSignal<TestSignal>().ToCommand<AnotherTestCommand>();
	}
	
}