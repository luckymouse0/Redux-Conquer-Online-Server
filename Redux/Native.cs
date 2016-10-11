using System;
using System.Runtime.InteropServices;

namespace Redux
{
    public unsafe class MSVCRT
    {

        #region memcpy
        public static void* memcpy(byte* dst, byte* src, int length)
        {
            for (var i = 0; i < length; i++)
                *(dst + i) = *(src + i);
            return dst;
        }
        public static void* memcpy(sbyte* dst, sbyte* src, int length)
        {
            for (var i = 0; i < length; i++)
                *(dst + i) = *(src + i);
            return dst;
        }

        public static void* memcpy(sbyte* dst, byte* src, int length)
        {
            for (var i = 0; i < length; i++)
                *(dst + i) = *((sbyte*)(src + i)); 
            return dst;
        }

        public static void* memcpy(byte* dst, sbyte* src, int length)
        {
            for (var i = 0; i < length; i++)
                *(dst + i) = *((byte*)(src + i));
            return dst;
        }

        private const string MSVCRT_DLL = @"C:\Windows\system32\msvcrt.dll";
        private const string MSVCRT_DLL_alt = @"D:\Windows\system32\msvcrt.dll";
        [DllImport(MSVCRT_DLL, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void* _memcpy(void* dst, void* src, int length);

        [DllImport(MSVCRT_DLL_alt, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void* _memcpy_alt(void* dst, void* src, int length);


        public static void* memcpy(uint* dst, byte* src, int length)
        {
            if (Environment.SystemDirectory.StartsWith("D"))
                return _memcpy_alt(dst, src, length);
            return _memcpy(dst, src, length);
        }

        #endregion


        #region memset
        public static void* memset(byte* dst, byte fill, int length)
        {
            for (var i = 0; i < length; i++)
                *(dst + i) = fill;
            return dst;
        }
        public static void* memset(sbyte* dst, sbyte fill, int length)
        {
            for (var i = 0; i < length; i++)
                *(dst + i) = fill;
            return dst;
        }
        #endregion


    }

    public static unsafe class NativeExtensions
    {
        public static void CopyTo(this string str, void* pDest)
        {
            var dest = (byte*) pDest;
            for (var i = 0; i < str.Length; i++)
            {
                dest[i] = (byte) str[i];
            }
        }

        public static byte[] UnsafeClone(this byte[] buffer)
        {
            var bufCopy = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, bufCopy, 0, buffer.Length);
            return bufCopy;
        }
    }
}