// NativeDllTest.cpp : Defines the exported functions for the DLL application.
//

#ifdef _WIN32
	#define NativeAPI _declspec(dllexport)
#else
	#define	NativeAPI
#endif

#ifdef __cplusplus
extern "C"
{
#endif
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
#ifdef __cplusplus
}
#endif