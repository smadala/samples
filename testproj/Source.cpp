#include "Header.h"
#include <set>

using namespace std;

wchar_t const* msdia_library_name = L"msdia140.dll";

#define HIDDEN_LINE 0xFEEFEE

void ErrorLog(const char *msg)
{
	printf(msg);
	printf("\n");
	exit(-1);
}

HRESULT create_dia_instance(CComPtr<IDiaDataSource>& source)
{
	wstring directory_name;
	// directory_name.assign(L"C:\\Users\\maban\\Desktop\\Bug\\PortablePDB\\x86ret\\");
	directory_name.assign(L"D:\\bug\\MSDIA-PortablePDB\\");

	return NoRegCoCreate((directory_name + msdia_library_name).c_str(), CLSID_DiaSource, IID_IDiaDataSource, reinterpret_cast<void**>(&source));
}

void dumpFunction(IDiaSymbol* pSymbol, IDiaSession* pSession, char* className)
{
	ULONGLONG length = 0;
	DWORD isect = 0;
	
	pSymbol->get_addressSection(&isect);
	pSymbol->get_length(&length);
	if (isect != 0 && length > 0) 
	{
		HRESULT hr = S_OK;
		BSTR name;
		pSymbol->get_name(&name);

		char* namestr = _com_util::ConvertBSTRToString(name);
		if ((className!=NULL) && strstr(namestr, className) == NULL)
			return;

		CComPtr<IDiaEnumLineNumbers> pLines;
		DWORD rva;
		length = 0;
		
		/*if (pSymbol->get_relativeVirtualAddress(&rva) == S_OK)
		{
			pSymbol->get_length(&length);
			if (rva == 0)
			{
				ULONGLONG va;
				if (pSymbol->get_virtualAddress(&va) == S_OK)
				{
					hr = pSession->findLinesByVA(va, static_cast<DWORD>(length), &pLines);
				}
			}
			else
			{
				hr = pSession->findLinesByRVA(rva, static_cast<DWORD>(length), &pLines);
			}
		}*/

		DWORD addSec;
		if (pSymbol->get_addressSection(&addSec) == S_OK)
		{
			DWORD addOffSet;
			if (pSymbol->get_addressOffset(&addOffSet) == S_OK)
			{
				pSymbol->get_length(&length);

				hr = pSession->findLinesByAddr(addSec, addOffSet, static_cast<DWORD>(length), &pLines);
			}
		}

		DWORD token;
		hr = pSymbol->get_token(&token);
		
		if (SUCCEEDED(hr)) 
		{
			CComPtr<IDiaLineNumber> pLine;
			ULONG celt = 0;
			LONG lines_count;
			
			if(SUCCEEDED(pLines->get_Count(&lines_count)))
				printf("Func Name: %ls , line_count: %d, Token: %d \n", name, lines_count, token);
			else
				printf("Func Name: %ls , line_count: 0, Token: %d \n", name, token);

			while (SUCCEEDED(pLines->Next(1, &pLine, &celt) == S_OK) && celt == 1)
			{
				CComPtr<IDiaSourceFile> source_file;
				CComBSTR file_name;
				DWORD linenum;
				
				if (FAILED(pLine->get_lineNumber(&linenum)))
					continue;
				
				if (FAILED(pLine->get_sourceFile(&source_file)))
					continue;

				if (SUCCEEDED(source_file->get_fileName(&file_name)) && file_name)
				{
					printf("\tfound line %d in source %ls \n", linenum, file_name.m_str);
				}

				pLine.Release();
			}
		}
	}
}

struct SequencePointInfo
{
	wstring name;	//seriously with every seq point?
	ULONG LineBegin;
	ULONG ColumnBegin;
	ULONG LineEnd;
	ULONG ColumnEnd;
	ULONG OffSets;
	ULONG AsyncOffSets;
};

struct AsyncStepInfo
{
	ULONG32 yieldOffset;        // The IL offset of the return instruction of an async yield
	mdToken breakpointMethod;   // The method where the async method will continue
	ULONG32 breakpointOffset;   // The offset in the continuation method that ends the async context
};

typedef HRESULT __stdcall DLLGETCLASSOBJECT(REFCLSID rclsid,
	REFIID   riid,
	void   **ppv);

HRESULT STDMETHODCALLTYPE NoRegCoCreate(LPCWSTR szDllPath,
	REFCLSID   rclsid,
	REFIID     riid,
	void     **ppv)
{
	HMODULE hmod = LoadLibrary(szDllPath);
	if (hmod == NULL) {
		return HRESULT_FROM_WIN32(GetLastError());
	}

	DLLGETCLASSOBJECT *pfnDllGetClassObject;
	pfnDllGetClassObject = (DLLGETCLASSOBJECT*)GetProcAddress(hmod, "DllGetClassObject");
	if (pfnDllGetClassObject == NULL) {
		return HRESULT_FROM_WIN32(GetLastError());
	}
	
	IClassFactory *pcf;
	HRESULT hr = (pfnDllGetClassObject)(rclsid, IID_PPV_ARGS(&pcf));
	if (SUCCEEDED(hr)) {
		
		hr = pcf->CreateInstance(NULL, riid, ppv);
		pcf->Release();
	}
	return hr;
}

void GetMethods(ISymUnmanagedReader* pSymReader, vector<pair<DWORD, wstring>> const& tokenVec)
{
	for (vector<pair<DWORD, wstring>>::const_iterator it = tokenVec.begin(); it != tokenVec.end(); ++it)
	{
		CComPtr<ISymUnmanagedMethod> pSymMethod = nullptr;

		HRESULT hr = pSymReader->GetMethod(it->first, &pSymMethod);

		if (SUCCEEDED(hr))
		{
			ULONG32 lineCount = 0;
			
			pSymMethod->GetSequencePointCount(&lineCount);
			
			if (lineCount != 0)
			{
				SequencePointInfo* seqPointInfo = NULL;
				SequencePointInfo* seqPointInfoIterator = NULL;

				seqPointInfo = new SequencePointInfo[lineCount];
				memset(seqPointInfo, 0, sizeof(SequencePointInfo) * (lineCount));
				
				seqPointInfoIterator = &seqPointInfo[0];
				UINT actualCount;

				if (lineCount)
				{
					ULONG32 *offsets = new ULONG32[lineCount], *lines = new ULONG32[lineCount], *columns = new ULONG32[lineCount];
					ULONG32 *endlines = new ULONG32[lineCount], *endcolumns = new ULONG32[lineCount];
					
					pSymMethod->GetSequencePoints(lineCount, &actualCount, offsets, NULL, lines, columns, endlines, endcolumns);

					if (lineCount > 0)
					{
						wprintf(L"\n\nMethodName: %s\n", it->second.c_str());
						wprintf(L"StartLine\t\tEndLine\t\tStartCol\t\tEndCol\t\tOffset\n");
					}

					for (ULONG i = 0; i < lineCount; i++)
					{
						//Hidden sequence point? is it async state machine
						if (lines[i] != HIDDEN_LINE)
						{
							seqPointInfoIterator->name.assign(it->second);		// Nice memory usage!
							seqPointInfoIterator->LineBegin = lines[i];
							seqPointInfoIterator->ColumnBegin = columns[i];
							seqPointInfoIterator->LineEnd = endlines[i];
							seqPointInfoIterator->ColumnEnd = endcolumns[i];
							seqPointInfoIterator->OffSets = offsets[i];
							seqPointInfoIterator++;
							wprintf(L"%d\t\t\t\t\t %d\t\t %d\t\t\t\t  %d\t\t 0x%x\n", lines[i], endlines[i], columns[i], endcolumns[i], offsets[i]);
						}
						else
						{
							wprintf(L"0x%x\t\t 0x%x\t 0x%x\t\t\t 0x%x\t\t 0x%x\n", lines[i], endlines[i], columns[i], endcolumns[i], offsets[i]);
						}
						
					}
				}
			}

			/*Doesn't give anything*/
			CComPtr<ISymUnmanagedAsyncMethod> pAsyncMethod = nullptr;
			BOOL isAsyncMethod;

			hr = pSymMethod->QueryInterface(__uuidof(ISymUnmanagedAsyncMethod), (void**)&pAsyncMethod);
			if (SUCCEEDED(hr))
			{
				pAsyncMethod->IsAsyncMethod(&isAsyncMethod);

				if (isAsyncMethod)
				{
					wprintf(L"\n\nMethodName: %s\n", it->second.c_str());
					wprintf(L"OffSet\t\tBreakPointOffSet\t\tBreakPointToken\n");

					UINT numSteps;
					(pAsyncMethod)->GetAsyncStepInfoCount(&numSteps);

					AsyncStepInfo* asyncPointInfo = NULL;
					AsyncStepInfo* asyncPointInfoIterator = NULL;

					asyncPointInfo = new AsyncStepInfo[numSteps];
					memset(asyncPointInfo, 0, sizeof(SequencePointInfo) * (numSteps));

					ULONG32 *poffsets = new ULONG32[numSteps];
					ULONG32 *breakpointOffset = new ULONG32[numSteps];
					mdToken *breakpointMethod = new mdToken[numSteps];

					(pAsyncMethod)->GetAsyncStepInfo(numSteps, &numSteps, poffsets, breakpointOffset, breakpointMethod);

					asyncPointInfoIterator = &asyncPointInfo[0];

					for (UINT i = 0; i < numSteps; ++i)
					{
						asyncPointInfoIterator->yieldOffset = poffsets[i];
						asyncPointInfoIterator->breakpointMethod = breakpointMethod[i];
						asyncPointInfoIterator->breakpointOffset = breakpointOffset[i];

						wprintf(L"0x%x\t\t 0x%x\t\t 0x%x\n", poffsets[i], breakpointOffset[i], breakpointMethod[i]);

						asyncPointInfoIterator++;
					}
				}				
			}
		}
	}
}

void init(vector<pair<DWORD, wstring>> const& tokenVec)
{
	CComPtr<IMetaDataDispenser> pMetaDataDispenser2 = nullptr;

	HRESULT hr2 = NoRegCoCreate(L"C:\\Users\\maban\\Desktop\\Bug\\testproj\\Microsoft.DiaSymReader.Native.x86.dll", CLSID_CorMetaDataDispenser, IID_IMetaDataDispenser, (void**)&pMetaDataDispenser2);

	CoInitialize(NULL);
	CComPtr<IMetaDataDispenser> pMetaDataDispenser = nullptr;
	

	HRESULT hr = CoCreateInstance(CLSID_CorMetaDataDispenser, 0, CLSCTX_INPROC_SERVER, IID_IMetaDataDispenser, (LPVOID*)&pMetaDataDispenser);
	if (SUCCEEDED(hr))
	{
		
		CComPtr<IMetaDataImport2> pMetadata = nullptr;
		// wstring path = L"D:\\tmp\\UnitTestProject53.dll";
		// wstring pdbpath = L"D:\\tmp\\UnitTestProject53.pdb";
		wstring path = L"C:\\Users\\samadala\\src\\tmp\\test-msdia\\bin\\Debug\\net46\\test-msdia.dll";
		wstring pdbpath = L"C:\\Users\\samadala\\src\\tmp\\test-msdia\\bin\\Debug\\net46\\test-msdia.pdb";
		
		wstring searchpath = L"C:\\Users\\samadala\\src\\tmp\\test-msdia\\bin\\Debug\\net46\\";

		hr = pMetaDataDispenser->OpenScope(path.c_str(), ofRead, IID_IMetaDataImport2, reinterpret_cast<IUnknown**>(&pMetadata));

		if (SUCCEEDED(hr))
		{
			CComPtr<ISymUnmanagedBinder> pBinder = nullptr;
			hr = CoCreateInstance(CLSID_CorSymBinder_SxS, NULL, CLSCTX_INPROC_SERVER, IID_ISymUnmanagedBinder, (void**)&pBinder);

			if (SUCCEEDED(hr))
			{
				CComPtr<ISymUnmanagedReader> pSymReader;
				hr = pBinder->GetReaderForFile(pMetadata, path.c_str(), searchpath.c_str(), &pSymReader);

				if (FAILED(hr))
				{					
					CComPtr<IStream> pPdbStream;
					hr = SHCreateStreamOnFileEx(pdbpath.c_str(), STGM_READ | STGM_SHARE_DENY_WRITE, /*dwAttributes:*/0, /*fCreate:*/ FALSE, /*pStreamTemplate:*/nullptr, &pPdbStream);

					if (SUCCEEDED(hr))
					{
						CComPtr<ISymUnmanagedReader> pSymReader2;
						hr = pBinder->GetReaderFromStream(pMetadata, pPdbStream, &pSymReader2);
					}
				}
			}
		}		
	}

	CoUninitialize();
}

int main(int argc, char* argv[])
{
	char *szFilename = argv[1];
	char *szFilepath = argv[2];
	wchar_t wszFilename[_MAX_PATH], wszFilepath[_MAX_PATH];
	
	mbstowcs(wszFilename, szFilename, sizeof(wszFilename) / sizeof(wszFilename[0]));	
	mbstowcs(wszFilepath, szFilepath, sizeof(wszFilepath) / sizeof(wszFilepath[0]));

	CComPtr<IDiaDataSource> pSource = nullptr;
	HRESULT hr = create_dia_instance(pSource);

	if (FAILED(hr) || !pSource)
	{
		return hr;
	}

	if (FAILED(pSource->loadDataForExe(wszFilename, wszFilepath, NULL)))
		ErrorLog("loadDataFromPdb/Exe");

	CComPtr<IDiaSession> pSession;
	hr = pSource->openSession(&pSession);
	if (FAILED(hr) || !pSession)
		return 0;

	CComPtr<IDiaSymbol> pGlobalScope = nullptr;
	hr = pSession->get_globalScope(&pGlobalScope);
	if (FAILED(hr))
		return 0;

	
	// We're going to enumerate all functions
	CComPtr<IDiaEnumSymbols> pSymbolsEnum = nullptr;
	hr = pGlobalScope->findChildren(SymTagFunction, nullptr, nsNone, &pSymbolsEnum);
	if (FAILED(hr))
		return 0;

	CComPtr<IDiaSymbol> pSymbol;
	ULONG celt = 0;
	vector<pair<DWORD, wstring>> tokenVec;

	while (SUCCEEDED(hr = pSymbolsEnum->Next(1, &pSymbol, &celt)) && celt == 1)
	{
		dumpFunction(pSymbol, pSession, NULL);

		/* for managed
		CComPtr<IDiaSymbol> pCompilandSymbol;
		pSymbol->get_lexicalParent(&pCompilandSymbol);

		if (pCompilandSymbol != nullptr)
		{
			enum SymTagEnum st;
			if ((S_OK == pCompilandSymbol->get_symTag((DWORD*)&st)) && (st == SymTagCompiland))
			{
				// Got the compiland symbol; now look for the compiland details symbol, which exposes the language.
				CComPtr<IDiaEnumSymbols> pEnum;
				if (S_OK == pCompilandSymbol->findChildren(SymTagCompilandDetails, nullptr, nsNone, &pEnum))
				{
					CComPtr<IDiaSymbol> pCompilandDetailsSym;
					ULONG celtFetched = 0;
					if ((pEnum != nullptr) && (S_OK == pEnum->Next(1, &pCompilandDetailsSym, &celtFetched)) && (celtFetched == 1) && (pCompilandDetailsSym != nullptr))
					{
						CComBSTR bstrCompilerName;
						if (S_OK == pCompilandDetailsSym->get_compilerName(&bstrCompilerName))
						{
							return S_OK;
						}
					}
				}
			}
		}
		
		DWORD token = 0;
		BSTR name;
		pSymbol->get_name(&name);
		char* namestr = _com_util::ConvertBSTRToString(name);
		pSymbol->get_token(&token);
		tokenVec.push_back(make_pair(token, name));
		*/
		pSymbol = NULL;
	}

	/*vector<pair<DWORD, wstring>> tokenVec;
	init(tokenVec);*/

	return 0;
}
