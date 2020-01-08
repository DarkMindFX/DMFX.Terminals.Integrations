// DMFX.Test.DarkMindConnect.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <windows.h> 
#include <iostream>

typedef int(_cdecl* fnDMFXAccountsInitSession)(LPCWSTR host, LPCWSTR accountKey, LPCWSTR folder);

#define MqlString LPWSTR

MqlString ToMqlString(const std::string& stdString)
{
	MqlString result;

	result = new WCHAR[stdString.length() + 1];
	std::copy(stdString.begin(), stdString.end(), result);
	result[stdString.length()] = 0;

	return result;

}

int main()
{
    std::cout << "DarkMindConnect test\n";

	HINSTANCE hinstLib;
	fnDMFXAccountsInitSession funDMFXAccountsInitSession;

	hinstLib = LoadLibrary(TEXT("D:\\Projects\\RoiFX\\DarkMindFX\\DarkMindFX.Terminals.Integrations\\DarkMindConnect\\bin\\DarkMindConnect.dll"));

	if (hinstLib != NULL)
	{
		std::cout << "Dll loaded\n";

		funDMFXAccountsInitSession = (fnDMFXAccountsInitSession)GetProcAddress(hinstLib, "DMFXAccountsInitSession");

		funDMFXAccountsInitSession(ToMqlString("http://localhost/"), ToMqlString("AcountKey"), ToMqlString("folder"));

	}
}


