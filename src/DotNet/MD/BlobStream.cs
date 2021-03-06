/*
    Copyright (C) 2012-2014 de4dot@gmail.com

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
    CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
    SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

﻿using dnlib.IO;

namespace dnlib.DotNet.MD {
	/// <summary>
	/// Represents the #Blob stream
	/// </summary>
	public sealed class BlobStream : HeapStream {
		static readonly byte[] noData = new byte[0];

		/// <inheritdoc/>
		public BlobStream() {
		}

		/// <inheritdoc/>
		public BlobStream(IImageStream imageStream, StreamHeader streamHeader)
			: base(imageStream, streamHeader) {
		}

		/// <summary>
		/// Reads data
		/// </summary>
		/// <param name="offset">Offset of data</param>
		/// <returns>The data or <c>null</c> if invalid offset</returns>
		public byte[] Read(uint offset) {
			// The CLR has a special check for offset 0. It always interprets it as
			// 0-length data, even if that first byte isn't 0 at all.
			if (offset == 0)
				return noData;
			IImageStream reader;
			int size = GetReader(offset, out reader);
			if (size < 0)
				return null;
			return reader.ReadBytes(size);
		}

		/// <summary>
		/// Reads data just like <see cref="Read"/>, but returns an empty array if
		/// offset is invalid
		/// </summary>
		/// <param name="offset">Offset of data</param>
		/// <returns>The data</returns>
		public byte[] ReadNoNull(uint offset) {
			return Read(offset) ?? noData;
		}

		/// <summary>
		/// Creates a new sub stream of the #Blob stream that can access one blob
		/// </summary>
		/// <param name="offset">Offset of blob</param>
		/// <returns>A new stream</returns>
		public IImageStream CreateStream(uint offset) {
			IImageStream reader;
			int size = GetReader(offset, out reader);
			if (size < 0)
				return MemoryImageStream.CreateEmpty();
			return reader.Create((FileOffset)reader.Position, size);
		}

		int GetReader(uint offset, out IImageStream reader) {
			reader = null;
			if (!IsValidOffset(offset))
				return -1;
			reader = GetReader(offset);
			uint length;
			if (!reader.ReadCompressedUInt32(out length))
				return -1;
			if (reader.Position + length < length || reader.Position + length > reader.Length)
				return -1;

			return (int)length;	// length <= 0x1FFFFFFF so this cast does not make it negative
		}
	}
}
