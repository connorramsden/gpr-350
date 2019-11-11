#include "MyUnityPlugin.h"

#include "Foo.h"

Foo* inst = 0;

int initFoo(int f_new)
{
	if (!inst)
	{
		inst = new Foo(f_new);
		return 1;
	}

	return 0;
}

int doFoo(int bar)
{
	if (inst)
	{
		int result = inst->foo(bar);
		return result;
	}

	return 0;
}

int termFoo()
{
	if (inst)
	{
		delete inst;
		inst = 0;
		return 1;
	}

	return 0;
}
