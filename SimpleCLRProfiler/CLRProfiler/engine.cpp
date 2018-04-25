#include "engine.h"

engine::engine() : refCount(0), corProfilerInfo(nullptr)
{
}

engine::~engine()
{
	if (this->corProfilerInfo != nullptr)
	{
		this->corProfilerInfo->Release();
		this->corProfilerInfo = nullptr;
	}
}

HRESULT engine::Initialize(IUnknown *pICorProfilerInfoUnk)
{
	HRESULT queryInterfaceResult = pICorProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo3), reinterpret_cast<void **>(&this->corProfilerInfo));

	if (FAILED(queryInterfaceResult))
	{
		return E_FAIL;
	}

	DWORD eventMask = COR_PRF_MONITOR_JIT_COMPILATION |
		COR_PRF_DISABLE_TRANSPARENCY_CHECKS_UNDER_FULL_TRUST | /* helps the case where this profiler is used on Full CLR */
		COR_PRF_DISABLE_INLINING;

	auto hr = this->corProfilerInfo->SetEventMask(eventMask);

	return S_OK;

}

HRESULT engine::Shutdown()
{
	return S_OK;
}

STDMETHODIMP engine::AppDomainCreationStarted(AppDomainID /*appDomainId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AppDomainCreationFinished(AppDomainID /*appDomainId*/, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AppDomainShutdownStarted(AppDomainID /*appDomainId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AppDomainShutdownFinished(AppDomainID /*appDomainId*/, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AssemblyLoadStarted(AssemblyID /*assemblyId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AssemblyLoadFinished(AssemblyID /*assemblyId*/, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AssemblyUnloadStarted(AssemblyID /*assemblyId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::AssemblyUnloadFinished(AssemblyID /*assemblyId*/, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ModuleLoadStarted(ModuleID /*moduleId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ModuleUnloadStarted(ModuleID /*moduleId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ModuleLoadFinished(ModuleID moduleId, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ModuleUnloadFinished(ModuleID moduleId, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ModuleAttachedToAssembly(ModuleID /*moduleId*/, AssemblyID /*AssemblyId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ClassLoadStarted(ClassID /*classId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ClassLoadFinished(ClassID /*classId*/, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ClassUnloadStarted(ClassID /*classId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ClassUnloadFinished(ClassID /*classId*/, HRESULT /*hrStatus*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::FunctionUnloadStarted(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::JITCompilationStarted(FunctionID functionId, BOOL /*fIsSafeToBlock*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::JITCompilationFinished(FunctionID /*functionId*/, HRESULT /*hrStatus*/, BOOL /*fIsSafeToBlock*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::JITCachedFunctionSearchStarted(FunctionID /*functionId*/, BOOL * /*pbUseCachedFunction*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::JITCachedFunctionSearchFinished(FunctionID /*functionId*/, COR_PRF_JIT_CACHE /*result*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::JITFunctionPitched(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::JITInlining(FunctionID /*callerId*/, FunctionID /*calleeId*/, BOOL * /*pfShouldInline*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ThreadCreated(ThreadID /*threadId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ThreadDestroyed(ThreadID /*threadId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ThreadAssignedToOSThread(ThreadID /*managedThreadId*/, DWORD /*osThreadId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingClientInvocationStarted()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingClientSendingMessage(GUID * /*pCookie*/, BOOL /*fIsAsync*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingClientReceivingReply(GUID * /*pCookie*/, BOOL /*fIsAsync*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingClientInvocationFinished()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingServerReceivingMessage(GUID * /*pCookie*/, BOOL /*fIsAsync*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingServerInvocationStarted()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingServerInvocationReturned()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RemotingServerSendingReply(GUID * /*pCookie*/, BOOL /*fIsAsync*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::UnmanagedToManagedTransition(FunctionID /*functionId*/, COR_PRF_TRANSITION_REASON /*reason*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ManagedToUnmanagedTransition(FunctionID /*functionId*/, COR_PRF_TRANSITION_REASON /*reason*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON /*suspendReason*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeSuspendFinished()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeSuspendAborted()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeResumeStarted()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeResumeFinished()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeThreadSuspended(ThreadID /*threadId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RuntimeThreadResumed(ThreadID /*threadId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::MovedReferences(ULONG /*cMovedObjectIDRanges*/, ObjectID /*oldObjectIDRangeStart*/[], ObjectID /*newObjectIDRangeStart*/[], ULONG /*cObjectIDRangeLength*/[])
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ObjectAllocated(ObjectID /*objectId*/, ClassID /*classId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ObjectsAllocatedByClass(ULONG /*cClassCount*/, ClassID /*classIds*/[], ULONG /*cObjects*/[])
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ObjectReferences(ObjectID /*objectId*/, ClassID /*classId*/, ULONG /*cObjectRefs*/, ObjectID /*objectRefIds*/[])
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RootReferences(ULONG /*cRootRefs*/, ObjectID /*rootRefIds*/[])
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionThrown(ObjectID /*thrownObjectId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionSearchFunctionEnter(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionSearchFunctionLeave()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionSearchFilterEnter(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionSearchFilterLeave()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionSearchCatcherFound(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionOSHandlerEnter(UINT_PTR /*__unused*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionOSHandlerLeave(UINT_PTR /*__unused*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionUnwindFunctionEnter(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionUnwindFunctionLeave()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionUnwindFinallyEnter(FunctionID /*functionId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionUnwindFinallyLeave()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionCatcherEnter(FunctionID /*functionId*/, ObjectID /*objectId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionCatcherLeave()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::COMClassicVTableCreated(ClassID /*wrappedClassId*/, REFGUID /*implementedIID*/, void* /*pVTable*/, ULONG /*cSlots*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::COMClassicVTableDestroyed(ClassID /*wrappedClassId*/, REFGUID /*implementedIID*/, void* /*pVTable*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionCLRCatcherFound()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ExceptionCLRCatcherExecute()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ThreadNameChanged(ThreadID /*threadId*/, ULONG /*cchName*/, WCHAR /*name*/[])
{
	return E_NOTIMPL;
}


STDMETHODIMP engine::GarbageCollectionStarted(int /*cGenerations*/, BOOL /*generationCollected*/[], COR_PRF_GC_REASON /*reason*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::SurvivingReferences(ULONG /*cSurvivingObjectIDRanges*/, ObjectID /*objectIDRangeStart*/[], ULONG /*cObjectIDRangeLength*/[])
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::GarbageCollectionFinished()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::FinalizeableObjectQueued(DWORD /*finalizerFlags*/, ObjectID /*objectID*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::RootReferences2(ULONG /*cRootRefs*/, ObjectID /*rootRefIds*/[], COR_PRF_GC_ROOT_KIND /*rootKinds*/[], COR_PRF_GC_ROOT_FLAGS /*rootFlags*/[], UINT_PTR /*rootIds*/[])
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::HandleCreated(GCHandleID /*handleId*/, ObjectID /*initialObjectId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::HandleDestroyed(GCHandleID /*handleId*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::InitializeForAttach(IUnknown * /*pCorProfilerInfoUnk*/, void * /*pvClientData*/, UINT /*cbClientData*/)
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ProfilerAttachComplete()
{
	return E_NOTIMPL;
}

STDMETHODIMP engine::ProfilerDetachSucceeded()
{
	return E_NOTIMPL;
}

HRESULT engine::QueryInterface(REFIID riid, void ** ppvObject)
{
	if (riid == IID_IUnknown)
	{
		*ppvObject = static_cast<IUnknown*>(static_cast<ICorProfilerCallback*>(this));
		AddRef();
		return S_OK;
	}
	else if (riid == IID_ICorProfilerCallback)
	{
		*ppvObject = static_cast<ICorProfilerCallback*>(this);
		AddRef();
		return S_OK;
	}
	else if (riid == IID_ICorProfilerCallback2)
	{
		*ppvObject = static_cast<ICorProfilerCallback2*>(this);
		AddRef();
		return S_OK;
	}
	else if (riid == IID_ICorProfilerCallback3)
	{
		*ppvObject = static_cast<ICorProfilerCallback3*>(this);
		AddRef();
		return S_OK;
	}

	return E_NOINTERFACE;
}
