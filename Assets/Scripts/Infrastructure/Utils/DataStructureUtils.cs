using System;
using System.Collections.Generic;
using Unity.Collections;

namespace FormForge.Utils
{
    /// <summary>
    /// Provides utility functions for copying objects, resizing buffers, and sorting collections.
    /// </summary>
    public static class DataStructureUtils
    {
        /// <summary>
        /// Copies properties and fields from one object to another.
        /// </summary>
        public static void CopyObject(this System.Object dst, object src)
        {
            var srcT = src.GetType();
            var dstT = dst.GetType();
            foreach (var f in srcT.GetFields())
            {
                var dstF = dstT.GetField(f.Name);
                if (dstF == null)
                    continue;
                try
                {
                    dstF.SetValue(dst, f.GetValue(src));
                }
                catch { }
            }


            foreach (var f in srcT.GetProperties())
            {
                var dstF = dstT.GetProperty(f.Name);
                if (dstF == null)
                    continue;
                try
                {
                    dstF.SetValue(dst, f.GetValue(src, null), null);
                }
                catch { }
            }

        }

        /// <summary>
        /// Resizes a NativeArray buffer if its capacity is less than the specified minimum size.
        /// </summary>
        public static void ResizeBuffer<T>(ref NativeArray<T> buffer, int minBufferSize) where T : unmanaged
        {
            if (buffer.Length < minBufferSize)
            {
                var newBuffer = new NativeArray<T>(minBufferSize * 2, Allocator.Persistent);
                NativeArray<T>.Copy(buffer, newBuffer, buffer.Length);
                buffer.Dispose();
                buffer = newBuffer;
            }
        }

        /// <summary>
        /// Resizes a List buffer if its capacity is less than the specified minimum size.
        /// </summary>
        public static void ResizeBuffer<T>(List<T> buffer, int minBufferSize)
        {
            if (buffer.Capacity < minBufferSize)
            {
                buffer.Capacity = minBufferSize * 2;
            }
        }

        /// <summary>
        /// Resizes an array buffer if its capacity is less than the specified minimum size.
        /// </summary
        public static void ResizeBuffer<T>(ref T[] buffer, int minBufferSize)
        {
            if (buffer.Length < minBufferSize)
            {
                buffer = new T[minBufferSize * 2];
            }
        }

        /// <summary>
        /// Sorts an array using the insertion sort algorithm.
        /// </summary>
        public static void InsertionSort<T>(T[] array, Comparison<T> comparison)
        {
            for (int i = 1; i < array.Length; i++)
            {
                T value = array[i];

                int j = i - 1;
                while (j >= 0 && comparison(value, array[j]) < 0)
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = value;
            }
        }

        /// <summary>
        /// Sorts a list using the insertion sort algorithm.
        /// </summary>
        public static void InsertionSort<T>(List<T> list, Comparison<T> comparison)
        {
            int listCount = list.Count;
            for (int i = 1; i < listCount; i++)
            {
                T value = list[i];

                int j = i - 1;
                while (j >= 0 && comparison(value, list[j]) < 0)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = value;
            }
        }
    }
}