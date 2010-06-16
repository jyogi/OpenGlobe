﻿#region License
//
// (C) Copyright 2009 Patrick Cozzi and Deron Ohlarik
//
// Distributed under the Boost Software License, Version 1.0.
// See License.txt or http://www.boost.org/LICENSE_1_0.txt.
//
#endregion

using System.Drawing;
using System.Diagnostics;
using OpenGlobe.Renderer;
using OpenTK.Graphics.OpenGL;
using ImagingPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenGlobe.Renderer.GL3x
{
    internal class ReadPixelBufferGL3x : ReadPixelBuffer
    {
        public ReadPixelBufferGL3x(ReadPixelBufferHint usageHint, int sizeInBytes)
        {
            _bufferObject = new BufferGL3x(BufferTarget.PixelPackBuffer, ToBufferHint(usageHint), sizeInBytes);
            _usageHint = usageHint;
        }

        internal void Bind()
        {
            _bufferObject.Bind();
        }

        #region ReadPixelBuffer Members

        public override void CopyFromSystemMemory<T>(
            T[] bufferInSystemMemory,
            int destinationOffsetInBytes,
            int lengthInBytes)
        {
            _bufferObject.CopyFromSystemMemory(bufferInSystemMemory, destinationOffsetInBytes, lengthInBytes);
        }

        public override void CopyFromBitmap(Bitmap bitmap)
        {
            _bufferObject.CopyFromBitmap(bitmap);
        }

        public override T[] CopyToSystemMemory<T>(int offsetInBytes, int sizeInBytes)
        {
            return _bufferObject.CopyToSystemMemory<T>(offsetInBytes, sizeInBytes);
        }

        public override Bitmap CopyToBitmap(int width, int height, ImagingPixelFormat pixelFormat)
        {
            return _bufferObject.CopyToBitmap(width, height, pixelFormat);
        }

        public override int SizeInBytes
        {
            get { return _bufferObject.SizeInBytes; }
        }

        public override ReadPixelBufferHint UsageHint
        {
            get { return _usageHint; }
        }

        #endregion

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bufferObject.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        static BufferHint ToBufferHint(ReadPixelBufferHint usageHint)
        {
            Debug.Assert(
                (usageHint == ReadPixelBufferHint.StreamRead) ||
                (usageHint == ReadPixelBufferHint.StaticRead) ||
                (usageHint == ReadPixelBufferHint.DynamicRead));

            return _bufferHints[(int)usageHint];
        }

        private BufferGL3x _bufferObject;
        private ReadPixelBufferHint _usageHint;

        private static readonly BufferHint[] _bufferHints = new[]
        {
            BufferHint.StreamRead,
            BufferHint.StaticRead,
            BufferHint.DynamicRead
        };
    }
}
