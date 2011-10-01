using System;
namespace Dukascopy.com.scalper.util
{

    public class ByteBuffer
    {

        /// <param name="args">
        /// </param>
        [STAThread]
        public static void Main(System.String[] args)
        {
            // TODO Auto-generated method stub
        }

        internal sbyte[] data;
        internal int curPos = -1;
        internal int end = -1;
        internal int mark_Renamed_Field;


        public static ByteBuffer allocate(int capacity)
        {
            return new ByteBuffer(capacity);
        }

        public static ByteBuffer allocateDirect(int capacity)
        {
            return allocate(capacity);
        }

        private static void trace(System.String method, System.String msg)
        {
            //System.Console.Out.WriteLine("@@@@@@@[" + method + "]" + msg);
        }

        private ByteBuffer(int capacity)
        {
            //trace("ByteBuffer(int)", "Creating our own byte buffer");
            data = new sbyte[capacity];
            curPos = 0;
            end = capacity;
        }

        private ByteBuffer(sbyte[] data)
        {
            //trace("ByteBuffer(byte[]", "Creating our own byte buffer:" + new System.String(SupportClass.ToCharArray(SupportClass.ToByteArray(data))));
            this.data = data;
            curPos = 0;
            end = data.Length;
        }

        //    Returns the byte array that backs this buffer  (optional operation).
        public virtual sbyte[] array()
        {
            //trace("array()", "Returning array");
            return data;
        }

        internal virtual int compareTo(ByteBuffer that)
        {
            //trace("compareTo()", "LATER");
            //Compares this buffer to another.
            sbyte[] thatData = that.array();
            for (int i = 0; i < end; i++)
            {
                if (data[i] != thatData[i])
                    return (data[i] - thatData[i]);
            }
            if (end != thatData.Length)
                return (end - thatData.Length);
            return 0;
        }

        //    Creates a new byte buffer that shares this buffer's content.
        public virtual ByteBuffer duplicate()
        {
            //trace("duplicate()", "LATER");
            return new ByteBuffer(data);
        }

        public override bool Equals(System.Object ob)
        {
            //trace("equals()", "LATER");
            //Tells whether or not this buffer is equal to another object.
            if (ob is ByteBuffer)
            {
                int i = compareTo((ByteBuffer)ob);
                if (i == 0)
                    return true;
            }
            return false;
        }

        public virtual sbyte get_Renamed()
        {
            //trace("get()", "LATER");
            //Relative get method.
            if (end > curPos)
                return data[curPos++];
            else
                throw new System.SystemException("Index out of bound");
        }

        public virtual ByteBuffer get_Renamed(sbyte[] dst)
        {
            //trace("get(byte[])", "LATER");
            //Relative bulk get method.
            return get_Renamed(dst, 0, dst.Length);
        }

        public virtual ByteBuffer get_Renamed(sbyte[] dst, int offset, int length)
        {
            //trace("get(byte[], int, int)", "LATER");
            //Relative bulk get method.
            if ((end - curPos) < length)
                throw new System.SystemException("Buffer underflow");
            Array.Copy(data, curPos, dst, offset, dst.Length);
            curPos += length;
            return this;
        }

        public virtual sbyte get_Renamed(int index)
        {
            //trace("get(int)", "LATER");
            //Absolute get method.
            if (index < end)
                return data[index];
            else
                throw new System.SystemException("Buffer overflow");
        }

        public override int GetHashCode()
        {
            //trace("hashCode()", "LATER");
            int h = 1;
            int p = position();
            for (int i = limit() - 1; i >= p; i--)
                h = 31 * h + (int)get_Renamed(i);
            return h;
        }

        public virtual void put(sbyte b)
        {
            //trace("put(byte)", "LATER");
            if (curPos >= end)
                throw new System.SystemException("Buffer Overflow");
            data[curPos++] = b;
        }

        public virtual ByteBuffer put(sbyte[] src)
        {
            //trace("put(byte[])", "LATER");
            //Relative bulk put method  (optional operation).
            return put(src, 0, src.Length);
        }

        public virtual ByteBuffer put(sbyte[] src, int offset, int length)
        {
            //trace("put(byte[], int, int)", "LATER");
            //Relative bulk put method  (optional operation).
            if (length > (end - curPos))
                throw new System.SystemException("Buffer overflow");
            Array.Copy(src, offset, data, curPos, length);
            curPos += length;
            return this;
        }

        public virtual ByteBuffer put(ByteBuffer src)
        {
            //trace("put(ByteBuffer)", "LATER");
            //Relative bulk put method  (optional operation).
            return put(src.array(), src.position(), src.remaining());
        }

        public virtual ByteBuffer put(int index, sbyte b)
        {
            //trace("put(int, byte)", "LATER");
            //Absolute put method  (optional operation).
            if (index >= end)
                throw new System.SystemException("Buffer overflow");
            data[index] = b;
            return this;
        }

        public override System.String ToString()
        {
            //Returns a string summarizing the state of this buffer.
            return "Position:" + position() + ", Remaining:" + remaining() + ", Data:" + new System.String(SupportClass.ToCharArray(SupportClass.ToByteArray(data)));
        }

        public static ByteBuffer wrap(sbyte[] array)
        {
            //trace("wrap(byte[])", "LATER");
            //Wraps a byte array into a buffer.
            return new ByteBuffer(array);
        }

        public static ByteBuffer wrap(sbyte[] array, int offset, int length)
        {
            //trace("wrap(byte[], int, int)", "LATER");
            //Wraps a byte array into a buffer.
            sbyte[] newData = new sbyte[length];
            Array.Copy(array, offset, newData, 0, length);
            return wrap(newData);
        }

        public virtual int capacity()
        {
            //trace("capacity()", "LATER");
            //Returns this buffer's capacity.
            return data.Length;
        }

        public virtual ByteBuffer clear()
        {
            //trace("clear()", "LATER");
            //Clears this buffer.
            curPos = 0;
            return this;
        }

        public virtual ByteBuffer flip()
        {
            //trace("flip()", "LATER");
            //Flips this buffer.
            end = curPos;
            curPos = 0;
            return this;
        }

        public virtual bool hasRemaining()
        {
            //trace("hasRemaining()", "LATER");
            //Tells whether there are any elements between the current position and the limit.
            if ((end - curPos) > 0)
                return true;
            else
                return false;
        }

        public virtual int limit()
        {
            //trace("limit()", "LATER");
            //Returns this buffer's limit.
            return end;
        }

        public virtual ByteBuffer limit(int newLimit)
        {
            //trace("limit(int)", "LATER");
            //Sets this buffer's limit.
            end = newLimit;
            return this;
        }

        public virtual int position()
        {
            //trace("position()", "LATER");
            //Returns this buffer's position.
            return curPos;
        }

        public virtual ByteBuffer position(int newPosition)
        {
            //trace("position(int)", "LATER");
            //Sets this buffer's position.
            if (newPosition >= end)
                throw new System.SystemException("Index outof bound");
            curPos = newPosition;
            return this;
        }

        public virtual int remaining()
        {
            //trace("remaining()", "LATER");
            //Returns the number of elements between the current position and the limit.
            return (end - curPos);
        }

        public virtual ByteBuffer mark()
        {
            //trace("mark()", "LATER");
            mark_Renamed_Field = curPos;
            return this;
        }

        public virtual ByteBuffer reset()
        {
            //trace("reset()", "LATER");
            //Resets this buffer's position to the previously-marked position.
            curPos = mark_Renamed_Field;
            return this;
        }

        public virtual ByteBuffer rewind()
        {
            //trace("rewind()", "LATER");
            //Rewinds this buffer.
            curPos = 0;
            return this;
        }

        public virtual sbyte[] limitArray()
        {
            //returns the array contents between position() and limt()
            sbyte[] lArray = new sbyte[end - curPos];
            Array.Copy(data, curPos, lArray, 0, lArray.Length);
            return lArray;
        }
    }
}