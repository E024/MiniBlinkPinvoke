using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniBlinkPinvoke
{
    /// <summary>
    /// fastcall协议 dll 调用
    ///      callbackProvider = new UnmanagedCodeSupplier(jsnav, IntPtr.Size == sizeof(int) ? x86CodeForFastcallWrapperForExecutionDelegate : x64CodeForFastcallWrapperForExecutionDelegate, IntPtr.Size == sizeof(int) ? new UIntPtr(0xF0F0F0F0) : new UIntPtr(0xF0F0F0F0F0F0F0F0));
    ///      
    /// https://blogs.msdn.microsoft.com/winsdk/2015/02/09/c-and-fastcall-how-to-make-them-work-together-without-ccli-shellcode/
    /// </summary>
    public class UnmanagedCodeSupplier : IDisposable
    {
        public static byte[] x86CodeForFastcallWrapperForExecutionDelegate = new byte[] {
    0x55,   //push  ebp
    0x8b, 0xec,  // mov  ebp, esp
    0x83, 0xec, 0x4c, // sub  esp, 76   ; 0000004cH
    0x53,   //push  ebx
    0x56,   //push  esi
    0x57,   //push  edi
    0x89, 0x55, 0xf8,  //mov  DWORD PTR _Argument2$[ebp], edx
    0x89, 0x4d, 0xfc,  //mov  DWORD PTR _Argument1$[ebp], ecx
    0xc7, 0x45, 0xf4, 0xf0, 0xf0, 0xf0, 0xf0,   //mov  DWORD PTR _protoFunction$[ebp], -252645136 ; f0f0f0f0H
    0x8b, 0x45, 0x0c,  //mov  eax, DWORD PTR _Argument4$[ebp]
    0x50,  // push  eax
    0x8b, 0x4d, 0x08, // mov  ecx, DWORD PTR _Argument3$[ebp]
    0x51,  // push  ecx
    0x8b, 0x55, 0xf8, // mov  edx, DWORD PTR _Argument2$[ebp]
    0x52,  // push  edx
    0x8b, 0x45, 0xfc, // mov  eax, DWORD PTR _Argument1$[ebp]
    0x50,  // push  eax
    0xff, 0x55, 0xf4, // call  DWORD PTR _protoFunction$[ebp]
    0x5f,  // pop  edi
    0x5e,  // pop  esi
    0x5b,  // pop  ebx
    0x8b, 0xe5,  // mov  esp, ebp
    0x5d,  // pop  ebp
    0xc2, 0x08, 0x00 // ret  8
};
        public static byte[] x64CodeForFastcallWrapperForExecutionDelegate = new byte[] {
    0x4c, 0x89, 0x4c, 0x24, 0x20, // mov  QWORD PTR [rsp+32], r9
    0x4c, 0x89, 0x44, 0x24, 0x18, // mov  QWORD PTR [rsp+24], r8
    0x89, 0x54, 0x24, 0x10, // mov  DWORD PTR [rsp+16], edx
    0x48, 0x89, 0x4c, 0x24, 0x08, // mov  QWORD PTR [rsp+8], rcx
    0x48, 0x83, 0xec, 0x38, // sub  rsp, 56   ; 00000038H
    0x48, 0xb8, 0xf0, 0xf0, 0xf0, 0xf0, 0xf0, 0xf0, 0xf0, 0xf0,//  mov  rax, -1085102592571150096 ; f0f0f0f0f0f0f0f0H
    0x48, 0x89, 0x44, 0x24, 0x20, // mov  QWORD PTR protoFunction$[rsp], rax
    0x4c, 0x8b, 0x4c, 0x24, 0x58, // mov  r9, QWORD PTR Argument4$[rsp]
    0x4c, 0x8b, 0x44, 0x24, 0x50, // mov  r8, QWORD PTR Argument3$[rsp]
    0x8b, 0x54, 0x24, 0x48,  //mov  edx, DWORD PTR Argument2$[rsp]
    0x48, 0x8b, 0x4c, 0x24, 0x40,//  mov  rcx, QWORD PTR Argument1$[rsp]
    0xff, 0x54, 0x24, 0x20,  //call  QWORD PTR protoFunction$[rsp]
    0x48, 0x83, 0xc4, 0x38,  //add  rsp, 56   ; 00000038H
    0xc3  // ret  0
};

        private Delegate delegateInstance;
        private IntPtr delegatePointer;
        private IntPtr newFunctionAddress;
        public IntPtr WrappedDelegateFunction { get { return newFunctionAddress; } }
        public UnmanagedCodeSupplier(Delegate actualDelegate, byte[] codeBytes, UIntPtr addressToReplace)
        {
            newFunctionAddress = VirtualAlloc(IntPtr.Zero, new IntPtr(codeBytes.Length), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
            if (newFunctionAddress == IntPtr.Zero)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            List<byte> newcode = new List<byte>();
            List<byte> currentcodequeued = new List<byte>();
            delegateInstance = actualDelegate;
            delegatePointer = Marshal.GetFunctionPointerForDelegate(delegateInstance);
            //Get the original address to replace as bytes
            byte[] cBytes = IntPtr.Size == sizeof(int) ? BitConverter.GetBytes(addressToReplace.ToUInt32()) : BitConverter.GetBytes(addressToReplace.ToUInt64()); //Account for the size difference
            //Get the new bytes to use
            byte[] nBytes = IntPtr.Size == sizeof(int) ? BitConverter.GetBytes(delegatePointer.ToInt32()) : BitConverter.GetBytes(delegatePointer.ToInt64()); //Account for the size difference
            int currentMatchNumber = 0; bool matched = false;
            //Loop through the code, find the matching address, replace it with the address from the delegate
            for (int i = 0; i < codeBytes.Length; i++)
            {
                if (matched)
                {
                    newcode.Add(codeBytes[i]);
                }
                else if (codeBytes[i] == cBytes[currentMatchNumber])
                {
                    currentMatchNumber++;
                    if (currentMatchNumber == cBytes.Length)
                    {//Add the real address instead of the fake
                        newcode.AddRange(nBytes);
                        currentcodequeued.Clear();
                        matched = true;
                    }
                    else
                    {
                        currentcodequeued.Add(codeBytes[i]);
                    }
                }
                else
                {
                    if (currentcodequeued.Count > 0)
                    {
                        newcode.AddRange(currentcodequeued);
                        currentcodequeued.Clear();
                    }
                    newcode.Add(codeBytes[i]);
                }
            }
            if (!matched)
            {
                Dispose();
                //cleanup - this happens to be implemented in such a way that just calling dispose can be used
                throw new ArgumentException("Invalid addressToReplace specified for the specified codeBytes");
            }
            //Now just copy that executable code over to where it should be
            Marshal.Copy(newcode.ToArray(), 0, newFunctionAddress, codeBytes.Length);
        }
        #region Native Interop
        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint MEM_RELEASE = 0x8000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;
        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr startAddress, IntPtr size, uint allocationType, uint protectionType);
        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr VirtualFree(IntPtr address, IntPtr size, uint freeType);
        #endregion
        ~UnmanagedCodeSupplier()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool calledDispose)
        {
            if (calledDispose)
            {
                //Do managed cleanup
                delegateInstance = null;
            }
            //Do native cleanup
            if (newFunctionAddress != IntPtr.Zero)
            {
                VirtualFree(newFunctionAddress, IntPtr.Zero, MEM_RELEASE);
                newFunctionAddress = IntPtr.Zero;
            }
        }
    }

}
