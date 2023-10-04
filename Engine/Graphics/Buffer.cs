﻿using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine.Graphics
{
    public class Buffer
    {
        internal uint ID {  get; private set; }
        public uint Size { get; protected set; }

        public Buffer()
        {
            ID = Graphics.GL.CreateBuffer();
        }
    }

    public class VertexBuffer : Buffer, IDisposable
    {
        private bool _disposed;

        public VertexBuffer(uint size)
        {
            Size = size;

            unsafe
            {
                Graphics.GL.NamedBufferStorage(ID, size, null, 0U);
            }
        }

        public VertexBuffer(float[] data)
        {
            SetData(data);
        }

        public VertexBuffer(List<float> data)
        {
            SetData(data.ToArray());
        }

        public VertexBuffer(uint[] data)
        {
            SetData(data);
        }

        public VertexBuffer(List<uint> data)
        {
            SetData(data.ToArray());
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            GC.SuppressFinalize(this);
        }

        public void SetData(float[] data)
        {
            Size = (uint)data.Length * 4U;

            unsafe
            {
                fixed(void* d = &data[0])
                {
                    Graphics.GL.NamedBufferStorage(ID, Size, d, BufferStorageMask.DynamicStorageBit);
                }
            }
        }

        public void SetData(uint[] data)
        {
            Size = (uint)data.Length * 4U;

            unsafe
            {
                fixed (void* d = &data[0])
                {
                    Graphics.GL.NamedBufferStorage(ID, Size, d, BufferStorageMask.DynamicStorageBit);
                }
            }
        }
    }

    public class IndexBuffer : Buffer, IDisposable
    {
        private bool _disposed;

        public IndexBuffer(uint count)
        {
            Size = count * 4U;

            unsafe
            {
                Graphics.GL.NamedBufferStorage(ID, Size, null, 0U);
            }
        }

        public IndexBuffer(uint[] data)
        {
            SetData(data);
        }

        public IndexBuffer(List<uint> data)
        {
            SetData(data.ToArray());
        }

        public void Dispose()
        {
            if(_disposed) return;

            _disposed = true;

            GC.SuppressFinalize(this);
        }

        public void SetData(uint[] data)
        {
            Size = (uint)data.Length * 4U;

            unsafe
            {
                fixed(void* d = &data[0])
                {
                    Graphics.GL.NamedBufferStorage(ID, Size, d, BufferStorageMask.DynamicStorageBit);
                }
            }
        }
    }

    public class BufferLayout
    {
        public struct BufferElement
        {
            public string Name;
            public VertexAttribType Type;
            public uint Size;
            public uint Count;
            public uint Offset;
            public bool Normalized;
        }

        public uint Size { get; private set; }

        public List<BufferElement> Elements { get; private set; } = new List<BufferElement>();

        public void Push<T>(string name, uint count = 1, bool normalized = false)
        {
            Type t = typeof(T);

            if (typeof(uint) == t)
            {
                Push(name, VertexAttribType.UnsignedInt, sizeof(uint), count, normalized);
            }
            else if (typeof(float) == t)
            {
                Push(name, VertexAttribType.Float, sizeof(float), count, normalized);
            }
            else if (typeof(byte) == t)
            {
                Push(name, VertexAttribType.Byte, sizeof(byte), count, normalized);
            }
            else if (typeof(int) == t)
            {
                Push(name, VertexAttribType.Int, sizeof(int), count, normalized);
            }
            else if (typeof(short) == t)
            {
                Push(name, VertexAttribType.Short, sizeof(short), count, normalized);
            }
            else if (typeof(ushort) == t)
            {
                Push(name, VertexAttribType.UnsignedShort, sizeof(ushort), count, normalized);
            }
            else if (typeof(Vector2D<float>) == t)
            {
                Push(name, VertexAttribType.Float, sizeof(float), count * 2, normalized);
            }
            else if (typeof(Vector2D<int>) == t)
            {
                Push(name, VertexAttribType.Int, sizeof(int), count * 2, normalized);
            }
            else if (typeof(Vector2D<uint>) == t)
            {
                Push(name, VertexAttribType.UnsignedInt, sizeof(uint), count * 2, normalized);
            }
            else if (typeof(Vector3D<float>) == t)
            {
                Push(name, VertexAttribType.Float, sizeof(float), count * 3, normalized);
            }
            else if (typeof(Vector3D<int>) == t)
            {
                Push(name, VertexAttribType.Int, sizeof(int), count * 3, normalized);
            }
            else if (typeof(Vector3D<uint>) == t)
            {
                Push(name, VertexAttribType.UnsignedInt, sizeof(uint), count * 3, normalized);
            }
            else if (typeof(Vector4D<float>) == t)
            {
                Push(name, VertexAttribType.Float, sizeof(float), count * 4, normalized);
            }
            else if (typeof(Vector4D<int>) == t)
            {
                Push(name, VertexAttribType.Int, sizeof(int), count * 4, normalized);
            }
            else if (typeof(Vector4D<uint>) == t)
            {
                Push(name, VertexAttribType.UnsignedInt, sizeof(uint), count * 4, normalized);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Push(string name, VertexAttribType type, uint size, uint count, bool normalized)
        {
            BufferElement element = new BufferElement()
            {
                Name = name,
                Type = type,
                Size = size,
                Count = count,
                Offset = Size,
                Normalized = normalized
            };

            Elements.Add(element);
            Size += size * count;
        }
    }
}
