using System;
using System.Runtime.InteropServices;
namespace nslib
{
    public class Class1
    {
        
        
    }
    [ComImport, ComVisible(false), Guid("00000001-0000-0000-C000-000000000046"),  InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IClassFactory
        {
            void CreateInstance(
                [MarshalAs(UnmanagedType.Interface)] object aggregator,
                ref Guid refiid,
                [MarshalAs(UnmanagedType.Interface)] out object createdObject);

            void LockServer(bool incrementRefCount);
        }
        [ComImport, Guid("00000001-0000-0000-C000-000000000046")]
    public class ClassFactory{

        }
}
