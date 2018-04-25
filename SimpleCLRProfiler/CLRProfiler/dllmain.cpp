// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "engine_factory.h"

const IID IID_IUnknown = { 0x00000000, 0x0000, 0x0000,{ 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46 } };

const IID IID_IClassFactory = { 0x00000001, 0x0000, 0x0000,{ 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46 } };

BOOL STDMETHODCALLTYPE DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	return TRUE;
}

extern "C" HRESULT STDMETHODCALLTYPE DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
	// {27AD456B-386A-499F-A0D9-165CDD7A2C31}
	const GUID CLSID_CorProfiler = { 0x27ad456b, 0x386a, 0x499f, 0xa0, 0xd9, 0x16, 0x5c, 0xdd, 0x7a, 0x2c, 0x31 };

	if (ppv == nullptr || rclsid != CLSID_CorProfiler)
	{
		return E_FAIL;
	}

	auto factory = new engine_factory;
	if (factory == nullptr)
	{
		return E_FAIL;
	}

	return factory->QueryInterface(riid, ppv);
}

extern "C" HRESULT STDMETHODCALLTYPE DllCanUnloadNow()
{
	return S_OK;
}