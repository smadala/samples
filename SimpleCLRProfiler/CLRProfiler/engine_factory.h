#pragma once

#include "unknwn.h"
#include <atomic>

class engine_factory : public IClassFactory
{
private:
	std::atomic<int> refCount;
public:
	engine_factory();
	virtual ~engine_factory();
	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void **ppvObject) override;
	ULONG   STDMETHODCALLTYPE AddRef(void) override;
	ULONG   STDMETHODCALLTYPE Release(void) override;
	HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown *pUnkOuter, REFIID riid, void **ppvObject) override;
	HRESULT STDMETHODCALLTYPE LockServer(BOOL fLock) override;
};