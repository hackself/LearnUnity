// NativeDllTest.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#define NativeAPI _declspec(dllexport)

extern "C"
{
	NativeAPI double add(double a, double b)
	{
		return a + b;
	}

	NativeAPI double sub(double a, double b)
	{
		return a - b;
	}

	NativeAPI double mul(double a, double b)
	{
		return a * b;
	}

	NativeAPI double div(double a, double b)
	{
		return a / b;
	}

}