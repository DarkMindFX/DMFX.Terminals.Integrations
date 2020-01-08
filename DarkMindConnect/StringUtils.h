
#pragma once

#include "stdafx.h"
#include "StringDefs.h"

using namespace System;
using namespace System::Runtime::InteropServices;


#pragma pack(push,1)
struct MqlStringStruct
{
   int      size;       // 32-������ �����, �������� ������ ��������������� ��� ������ ������
   LPWSTR   buffer;     // 32-��������� ����� ������, ����������� ������
   int      reserved;   // 32-������ �����, ���������������, �� ������������
};
#pragma pack(pop,1)

std::string ToStdString(const MqlString& mqlString)
{
	std::string result;
	if(mqlString != NULL)
	{
		result.assign(mqlString, mqlString + lstrlenW(mqlString));
	}

	return result;
}

String^ ToManagedString(const MqlString& mqlString)
{
	String^ result = gcnew String(mqlString);
	return result;
}

MqlString ToMqlString(const std::string& stdString)
{
	MqlString result;

	result = new WCHAR[stdString.length() + 1];
	std::copy(stdString.begin(), stdString.end(), result);
	result[stdString.length()] = 0; 

	return result;

}

MqlString ToMqlString(String^ cliString)
{
	MqlString result;
	result = new WCHAR[cliString->Length + 1];
	IntPtr p = Marshal::StringToHGlobalUni(cliString);
	wchar_t *pNewCharStr = static_cast<wchar_t*>(p.ToPointer());

	std::copy(pNewCharStr, pNewCharStr + cliString->Length, result);
	result[cliString->Length] = 0;
	
	Marshal::FreeHGlobal(p);

	return result;
}