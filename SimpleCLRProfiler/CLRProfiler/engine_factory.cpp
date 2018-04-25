#include "engine_factory.h"
#include "engine.h"

engine_factory::engine_factory() : refCount(0)
{
}

engine_factory::~engine_factory()
{
}

HRESULT engine_factory::QueryInterface(REFIID riid, void ** ppvObject)
{
	if (riid == IID_IUnknown || riid == IID_IClassFactory)
	{
		*ppvObject = this;
		this->AddRef();
		return S_OK;
	}

	*ppvObject = nullptr;
	return E_NOINTERFACE;
}

ULONG engine_factory::AddRef(void)
{
	return std::atomic_fetch_add(&this->refCount, 1);
}

ULONG engine_factory::Release(void)
{
	int count = std::atomic_fetch_sub(&this->refCount, 1);

	if (count <= 0)
	{
		delete this;
	}

	return count;
}

HRESULT engine_factory::CreateInstance(IUnknown * pUnkOuter, REFIID riid, void ** ppvObject)
{
	if (pUnkOuter)
	{
		return CLASS_E_NOAGGREGATION;
	}

	engine *eng = new engine();
	if (eng == nullptr)
	{
		return E_FAIL;
	}

	return eng->QueryInterface(riid, ppvObject);
}

HRESULT engine_factory::LockServer(BOOL fLock)
{
	return S_OK;
}
