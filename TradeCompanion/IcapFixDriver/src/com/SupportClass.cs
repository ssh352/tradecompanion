//

// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;

	/// <summary>
	/// This interface should be implemented by any class whose instances are intended 
	/// to be executed by a thread.
	/// </summary>
	public interface IThreadRunnable
	{
		/// <summary>
		/// This method has to be implemented in order that starting of the thread causes the object's 
		/// run method to be called in that separately executing thread.
		/// </summary>
		void Run();
	}

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
public class SupportClass
{
	/// <summary>
	/// Writes the exception stack trace to the received stream
	/// </summary>
	/// <param name="throwable">Exception to obtain information from</param>
	/// <param name="stream">Output sream used to write to</param>
	public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
	{
		stream.Write(throwable.StackTrace);
		stream.Flush();
	}

	/*******************************/
	/// <summary>
	/// Converts an array of sbytes to an array of bytes
	/// </summary>
	/// <param name="sbyteArray">The array of sbytes to be converted</param>
	/// <returns>The new array of bytes</returns>
	public static byte[] ToByteArray(sbyte[] sbyteArray)
	{
		byte[] byteArray = null;

		if (sbyteArray != null)
		{
			byteArray = new byte[sbyteArray.Length];
			for(int index=0; index < sbyteArray.Length; index++)
				byteArray[index] = (byte) sbyteArray[index];
		}
		return byteArray;
	}

	/// <summary>
	/// Converts a string to an array of bytes
	/// </summary>
	/// <param name="sourceString">The string to be converted</param>
	/// <returns>The new array of bytes</returns>
	public static byte[] ToByteArray(System.String sourceString)
	{
		return System.Text.UTF8Encoding.UTF8.GetBytes(sourceString);
	}

	/// <summary>
	/// Converts a array of object-type instances to a byte-type array.
	/// </summary>
	/// <param name="tempObjectArray">Array to convert.</param>
	/// <returns>An array of byte type elements.</returns>
    //public static byte[] ToByteArray(System.Object[] tempObjectArray)
    //{
    //    byte[] byteArray = null;
    //    if (tempObjectArray != null)
    //    {
    //        byteArray = new byte[tempObjectArray.Length];
    //        for (int index = 0; index < tempObjectArray.Length; index++)
    //            byteArray[index] = (byte)tempObjectArray[index];
    //    }
    //    return byteArray;
    //}

	/*******************************/
	/// <summary>
	/// Support class used to handle threads
	/// </summary>
	public class ThreadClass : IThreadRunnable
	{
		/// <summary>
		/// The instance of System.Threading.Thread
		/// </summary>
		private System.Threading.Thread threadField;
	      
		/// <summary>
		/// Initializes a new instance of the ThreadClass class
		/// </summary>
		public ThreadClass()
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.String Name)
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
			this.Name = Name;
		}
	      
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		public ThreadClass(System.Threading.ThreadStart Start)
		{
			threadField = new System.Threading.Thread(Start);
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.Threading.ThreadStart Start, System.String Name)
		{
			threadField = new System.Threading.Thread(Start);
			this.Name = Name;
		}
	      
		/// <summary>
		/// This method has no functionality unless the method is overridden
		/// </summary>
		public virtual void Run()
		{
		}
	      
		/// <summary>
		/// Causes the operating system to change the state of the current thread instance to ThreadState.Running
		/// </summary>
		public virtual void Start()
		{
			threadField.Start();
		}
	      
		/// <summary>
		/// Interrupts a thread that is in the WaitSleepJoin thread state
		/// </summary>
		public virtual void Interrupt()
		{
			threadField.Interrupt();
		}
	      
		/// <summary>
		/// Gets the current thread instance
		/// </summary>
		public System.Threading.Thread Instance
		{
			get
			{
				return threadField;
			}
			set
			{
				threadField = value;
			}
		}
	      
		/// <summary>
		/// Gets or sets the name of the thread
		/// </summary>
		public System.String Name
		{
			get
			{
				return threadField.Name;
			}
			set
			{
				if (threadField.Name == null)
					threadField.Name = value; 
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating the scheduling priority of a thread
		/// </summary>
		public System.Threading.ThreadPriority Priority
		{
			get
			{
				return threadField.Priority;
			}
			set
			{
				threadField.Priority = value;
			}
		}
	      
		/// <summary>
		/// Gets a value indicating the execution status of the current thread
		/// </summary>
		public bool IsAlive
		{
			get
			{
				return threadField.IsAlive;
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating whether or not a thread is a background thread.
		/// </summary>
		public bool IsBackground
		{
			get
			{
				return threadField.IsBackground;
			} 
			set
			{
				threadField.IsBackground = value;
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates
		/// </summary>
		public void Join()
		{
			threadField.Join();
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		public void Join(long MiliSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000));
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		/// <param name="NanoSeconds">Time of wait in nanoseconds</param>
		public void Join(long MiliSeconds, int NanoSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000 + NanoSeconds * 100));
			}
		}
	      
		/// <summary>
		/// Resumes a thread that has been suspended
		/// </summary>
		public void Resume()
		{
			threadField.Resume();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread. Calling this method 
		/// usually terminates the thread
		/// </summary>
		public void Abort()
		{
			threadField.Abort();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread while also providing
		/// exception information about the thread termination. 
		/// Calling this method usually terminates the thread.
		/// </summary>
		/// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted</param>
		public void Abort(System.Object stateInfo)
		{
			lock(this)
			{
				threadField.Abort(stateInfo);
			}
		}
	      
		/// <summary>
		/// Suspends the thread, if the thread is already suspended it has no effect
		/// </summary>
		public void Suspend()
		{
			threadField.Suspend();
		}
	      
		/// <summary>
		/// Obtain a String that represents the current Object
		/// </summary>
		/// <returns>A String that represents the current Object</returns>
		public override System.String ToString()
		{
			return "Thread[" + Name + "," + Priority.ToString() + "," + "" + "]";
		}
	     
		/// <summary>
		/// Gets the currently running thread
		/// </summary>
		/// <returns>The currently running thread</returns>
		public static ThreadClass Current()
		{
			ThreadClass CurrentThread = new ThreadClass();
			CurrentThread.Instance = System.Threading.Thread.CurrentThread;
			return CurrentThread;
		}
	}


	/*******************************/
	/// <summary>
	/// Represents a collection ob objects that contains no duplicate elements.
	/// </summary>	
	public interface SetSupport : System.Collections.ICollection, System.Collections.IList
	{
		/// <summary>
		/// Adds a new element to the Collection if it is not already present.
		/// </summary>
		/// <param name="obj">The object to add to the collection.</param>
		/// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
		new bool Add(System.Object obj);

		/// <summary>
		/// Adds all the elements of the specified collection to the Set.
		/// </summary>
		/// <param name="c">Collection of objects to add.</param>
		/// <returns>true</returns>
		bool AddAll(System.Collections.ICollection c);
	}


	/*******************************/
	/// <summary>
	/// SupportClass for the HashSet class.
	/// </summary>
	[Serializable]
	public class HashSetSupport : System.Collections.ArrayList, SetSupport
	{
		public HashSetSupport() : base()
		{	
		}

		public HashSetSupport(System.Collections.ICollection c) 
		{
			this.AddAll(c);
		}

		public HashSetSupport(int capacity) : base(capacity)
		{
		}

		/// <summary>
		/// Adds a new element to the ArrayList if it is not already present.
		/// </summary>		
		/// <param name="obj">Element to insert to the ArrayList.</param>
		/// <returns>Returns true if the new element was inserted, false otherwise.</returns>
		new public virtual bool Add(System.Object obj)
		{
			bool inserted;

			if ((inserted = this.Contains(obj)) == false)
			{
				base.Add(obj);
			}

			return !inserted;
		}

		/// <summary>
		/// Adds all the elements of the specified collection that are not present to the list.
		/// </summary>
		/// <param name="c">Collection where the new elements will be added</param>
		/// <returns>Returns true if at least one element was added, false otherwise.</returns>
		public bool AddAll(System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
			bool added = false;

			while (e.MoveNext() == true)
			{
				if (this.Add(e.Current) == true)
					added = true;
			}

			return added;
		}
		
		/// <summary>
		/// Returns a copy of the HashSet instance.
		/// </summary>		
		/// <returns>Returns a shallow copy of the current HashSet.</returns>
		public override System.Object Clone()
		{
			return base.MemberwiseClone();
		}
	}


	/*******************************/
	/// <summary>
	/// Converts an array of sbytes to an array of chars
	/// </summary>
	/// <param name="sByteArray">The array of sbytes to convert</param>
	/// <returns>The new array of chars</returns>
	public static char[] ToCharArray(sbyte[] sByteArray) 
	{
		return System.Text.UTF8Encoding.UTF8.GetChars(ToByteArray(sByteArray));
	}

	/// <summary>
	/// Converts an array of bytes to an array of chars
	/// </summary>
	/// <param name="byteArray">The array of bytes to convert</param>
	/// <returns>The new array of chars</returns>
	public static char[] ToCharArray(byte[] byteArray) 
	{
		return System.Text.UTF8Encoding.UTF8.GetChars(byteArray);
	}

	/*******************************/
	/// <summary>Reads a number of characters from the current source Stream and writes the data to the target array at the specified index.</summary>
	/// <param name="sourceStream">The source Stream to read from.</param>
	/// <param name="target">Contains the array of characteres read from the source Stream.</param>
	/// <param name="start">The starting index of the target array.</param>
	/// <param name="count">The maximum number of characters to read from the source Stream.</param>
	/// <returns>The number of characters read. The number will be less than or equal to count depending on the data available in the source Stream. Returns -1 if the end of the stream is reached.</returns>
	public static System.Int32 ReadInput(System.IO.Stream sourceStream, sbyte[] target, int start, int count)
	{
		// Returns 0 bytes if not enough space in target
		if (target.Length == 0)
			return 0;

		byte[] receiver = new byte[target.Length];
		int bytesRead   = sourceStream.Read(receiver, start, count);

		// Returns -1 if EOF
		if (bytesRead == 0)	
			return -1;
                
		for(int i = start; i < start + bytesRead; i++)
			target[i] = (sbyte)receiver[i];
                
		return bytesRead;
	}

	/// <summary>Reads a number of characters from the current source TextReader and writes the data to the target array at the specified index.</summary>
	/// <param name="sourceTextReader">The source TextReader to read from</param>
	/// <param name="target">Contains the array of characteres read from the source TextReader.</param>
	/// <param name="start">The starting index of the target array.</param>
	/// <param name="count">The maximum number of characters to read from the source TextReader.</param>
	/// <returns>The number of characters read. The number will be less than or equal to count depending on the data available in the source TextReader. Returns -1 if the end of the stream is reached.</returns>
    //public static System.Int32 ReadInput(System.IO.TextReader sourceTextReader, sbyte[] target, int start, int count)
    //{
    //    // Returns 0 bytes if not enough space in target
    //    if (target.Length == 0) return 0;

    //    char[] charArray = new char[target.Length];
    //    int bytesRead = sourceTextReader.Read(charArray, start, count);

    //    // Returns -1 if EOF
    //    if (bytesRead == 0) return -1;

    //    for(int index=start; index<start+bytesRead; index++)
    //        target[index] = (sbyte)charArray[index];

    //    return bytesRead;
    //}

	/*******************************/
	/// <summary>
	/// Copies an array of chars obtained from a String into a specified array of chars
	/// </summary>
	/// <param name="sourceString">The String to get the chars from</param>
	/// <param name="sourceStart">Position of the String to start getting the chars</param>
	/// <param name="sourceEnd">Position of the String to end getting the chars</param>
	/// <param name="destinationArray">Array to return the chars</param>
	/// <param name="destinationStart">Position of the destination array of chars to start storing the chars</param>
	/// <returns>An array of chars</returns>
	public static void GetCharsFromString(string sourceString, int sourceStart, int sourceEnd, char[] destinationArray, int destinationStart)
	{	
		int sourceCounter;
		int destinationCounter;
		sourceCounter = sourceStart;
		destinationCounter = destinationStart;
		while (sourceCounter < sourceEnd)
		{
			destinationArray[destinationCounter] = (char) sourceString[sourceCounter];
			sourceCounter++;
			destinationCounter++;
		}
	}

	/*******************************/
	/// <summary>
	/// This class provides functionality not found in .NET collection-related interfaces.
	/// </summary>
	public class ICollectionSupport
	{
		/// <summary>
		/// Adds a new element to the specified collection.
		/// </summary>
		/// <param name="c">Collection where the new element will be added.</param>
		/// <param name="obj">Object to add.</param>
		/// <returns>true</returns>
		public static bool Add(System.Collections.ICollection c, System.Object obj)
		{
			bool added = false;
			//Reflection. Invoke either the "add" or "Add" method.
			System.Reflection.MethodInfo method;
			try
			{
				//Get the "add" method for proprietary classes
				method = c.GetType().GetMethod("Add");
				if (method == null)
					method = c.GetType().GetMethod("add");
				int index = (int) method.Invoke(c, new System.Object[] {obj});
				if (index >=0)	
					added = true;
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return added;
		}

		/// <summary>
		/// Adds all of the elements of the "c" collection to the "target" collection.
		/// </summary>
		/// <param name="target">Collection where the new elements will be added.</param>
		/// <param name="c">Collection whose elements will be added.</param>
		/// <returns>Returns true if at least one element was added, false otherwise.</returns>
		public static bool AddAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
			bool added = false;

			//Reflection. Invoke "addAll" method for proprietary classes
			System.Reflection.MethodInfo method;
			try
			{
				method = target.GetType().GetMethod("addAll");

				if (method != null)
					added = (bool) method.Invoke(target, new System.Object[] {c});
				else
				{
					method = target.GetType().GetMethod("Add");
					while (e.MoveNext() == true)
					{
						bool tempBAdded =  (int) method.Invoke(target, new System.Object[] {e.Current}) >= 0;
						added = added ? added : tempBAdded;
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			return added;
		}

		/// <summary>
		/// Removes all the elements from the collection.
		/// </summary>
		/// <param name="c">The collection to remove elements.</param>
		public static void Clear(System.Collections.ICollection c)
		{
			//Reflection. Invoke "Clear" method or "clear" method for proprietary classes
			System.Reflection.MethodInfo method;
			try
			{
				method = c.GetType().GetMethod("Clear");

				if (method == null)
					method = c.GetType().GetMethod("clear");

				method.Invoke(c, new System.Object[] {});
			}
			catch (System.Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Determines whether the collection contains the specified element.
		/// </summary>
		/// <param name="c">The collection to check.</param>
		/// <param name="obj">The object to locate in the collection.</param>
		/// <returns>true if the element is in the collection.</returns>
		public static bool Contains(System.Collections.ICollection c, System.Object obj)
		{
			bool contains = false;

			//Reflection. Invoke "contains" method for proprietary classes
			System.Reflection.MethodInfo method;
			try
			{
				method = c.GetType().GetMethod("Contains");

				if (method == null)
					method = c.GetType().GetMethod("contains");

				contains = (bool)method.Invoke(c, new System.Object[] {obj});
			}
			catch (System.Exception e)
			{
				throw e;
			}

			return contains;
		}

		/// <summary>
		/// Determines whether the collection contains all the elements in the specified collection.
		/// </summary>
		/// <param name="target">The collection to check.</param>
		/// <param name="c">Collection whose elements would be checked for containment.</param>
		/// <returns>true id the target collection contains all the elements of the specified collection.</returns>
		public static bool ContainsAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{						
			System.Collections.IEnumerator e =  c.GetEnumerator();

			bool contains = false;

			//Reflection. Invoke "containsAll" method for proprietary classes or "Contains" method for each element in the collection
			System.Reflection.MethodInfo method;
			try
			{
				method = target.GetType().GetMethod("containsAll");

				if (method != null)
					contains = (bool)method.Invoke(target, new Object[] {c});
				else
				{					
					method = target.GetType().GetMethod("Contains");
					while (e.MoveNext() == true)
					{
						if ((contains = (bool)method.Invoke(target, new Object[] {e.Current})) == false)
							break;
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}

			return contains;
		}

		/// <summary>
		/// Removes the specified element from the collection.
		/// </summary>
		/// <param name="c">The collection where the element will be removed.</param>
		/// <param name="obj">The element to remove from the collection.</param>
		public static bool Remove(System.Collections.ICollection c, System.Object obj)
		{
			bool changed = false;

			//Reflection. Invoke "remove" method for proprietary classes or "Remove" method
			System.Reflection.MethodInfo method;
			try
			{
				method = c.GetType().GetMethod("remove");

				if (method != null)
					method.Invoke(c, new System.Object[] {obj});
				else
				{
					method = c.GetType().GetMethod("Contains");
					changed = (bool)method.Invoke(c, new System.Object[] {obj});
					method = c.GetType().GetMethod("Remove");
					method.Invoke(c, new System.Object[] {obj});
				}
			}
			catch (System.Exception e)
			{
				throw e;
			}

			return changed;
		}

		/// <summary>
		/// Removes all the elements from the specified collection that are contained in the target collection.
		/// </summary>
		/// <param name="target">Collection where the elements will be removed.</param>
		/// <param name="c">Elements to remove from the target collection.</param>
		/// <returns>true</returns>
		public static bool RemoveAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{
			System.Collections.ArrayList al = ToArrayList(c);
			System.Collections.IEnumerator e = al.GetEnumerator();

			//Reflection. Invoke "removeAll" method for proprietary classes or "Remove" for each element in the collection
			System.Reflection.MethodInfo method;
			try
			{
				method = target.GetType().GetMethod("removeAll");

				if (method != null)
					method.Invoke(target, new System.Object[] {al});
				else
				{
					method = target.GetType().GetMethod("Remove");
					System.Reflection.MethodInfo methodContains = target.GetType().GetMethod("Contains");

					while (e.MoveNext() == true)
					{
						while ((bool) methodContains.Invoke(target, new System.Object[] {e.Current}) == true)
							method.Invoke(target, new System.Object[] {e.Current});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			return true;
		}

		/// <summary>
		/// Retains the elements in the target collection that are contained in the specified collection
		/// </summary>
		/// <param name="target">Collection where the elements will be removed.</param>
		/// <param name="c">Elements to be retained in the target collection.</param>
		/// <returns>true</returns>
		public static bool RetainAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(target).GetEnumerator();
			System.Collections.ArrayList al = new System.Collections.ArrayList(c);

			//Reflection. Invoke "retainAll" method for proprietary classes or "Remove" for each element in the collection
			System.Reflection.MethodInfo method;
			try
			{
				method = c.GetType().GetMethod("retainAll");

				if (method != null)
					method.Invoke(target, new System.Object[] {c});
				else
				{
					method = c.GetType().GetMethod("Remove");

					while (e.MoveNext() == true)
					{
						if (al.Contains(e.Current) == false)
							method.Invoke(target, new System.Object[] {e.Current});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}

			return true;
		}

		/// <summary>
		/// Returns an array containing all the elements of the collection.
		/// </summary>
		/// <returns>The array containing all the elements of the collection.</returns>
		public static System.Object[] ToArray(System.Collections.ICollection c)
		{	
			int index = 0;
			System.Object[] objects = new System.Object[c.Count];
			System.Collections.IEnumerator e = c.GetEnumerator();

			while (e.MoveNext())
				objects[index++] = e.Current;

			return objects;
		}

		/// <summary>
		/// Obtains an array containing all the elements of the collection.
		/// </summary>
		/// <param name="objects">The array into which the elements of the collection will be stored.</param>
		/// <returns>The array containing all the elements of the collection.</returns>
		public static System.Object[] ToArray(System.Collections.ICollection c, System.Object[] objects)
		{	
			int index = 0;

			System.Type type = objects.GetType().GetElementType();
			System.Object[] objs = (System.Object[]) Array.CreateInstance(type, c.Count );

			System.Collections.IEnumerator e = c.GetEnumerator();

			while (e.MoveNext())
				objs[index++] = e.Current;

			//If objects is smaller than c then do not return the new array in the parameter
			if (objects.Length >= c.Count)
				objs.CopyTo(objects, 0);

			return objs;
		}

		/// <summary>
		/// Converts an ICollection instance to an ArrayList instance.
		/// </summary>
		/// <param name="c">The ICollection instance to be converted.</param>
		/// <returns>An ArrayList instance in which its elements are the elements of the ICollection instance.</returns>
		public static System.Collections.ArrayList ToArrayList(System.Collections.ICollection c)
		{
			System.Collections.ArrayList tempArrayList = new System.Collections.ArrayList();
			System.Collections.IEnumerator tempEnumerator = c.GetEnumerator();
			while (tempEnumerator.MoveNext())
				tempArrayList.Add(tempEnumerator.Current);
			return tempArrayList;
		}
	}


	/*******************************/
	/// <summary>
	/// The class performs token processing in strings
	/// </summary>
	public class Tokenizer: System.Collections.IEnumerator
	{
		/// Position over the string
		private long currentPos = 0;

		/// Include demiliters in the results.
		private bool includeDelims = false;

		/// Char representation of the String to tokenize.
		private char[] chars = null;
			
		//The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character and the form-feed character
		private string delimiters = " \t\n\r\f";		

		/// <summary>
		/// Initializes a new class instance with a specified string to process
		/// </summary>
		/// <param name="source">String to tokenize</param>
		public Tokenizer(System.String source)
		{			
			this.chars = source.ToCharArray();
		}

		/// <summary>
		/// Initializes a new class instance with a specified string to process
		/// and the specified token delimiters to use
		/// </summary>
		/// <param name="source">String to tokenize</param>
		/// <param name="delimiters">String containing the delimiters</param>
		public Tokenizer(System.String source, System.String delimiters):this(source)
		{			
			this.delimiters = delimiters;
		}


		/// <summary>
		/// Initializes a new class instance with a specified string to process, the specified token 
		/// delimiters to use, and whether the delimiters must be included in the results.
		/// </summary>
		/// <param name="source">String to tokenize</param>
		/// <param name="delimiters">String containing the delimiters</param>
		/// <param name="includeDelims">Determines if delimiters are included in the results.</param>
		public Tokenizer(System.String source, System.String delimiters, bool includeDelims):this(source,delimiters)
		{
			this.includeDelims = includeDelims;
		}	


		/// <summary>
		/// Returns the next token from the token list
		/// </summary>
		/// <returns>The string value of the token</returns>
		public System.String NextToken()
		{				
			return NextToken(this.delimiters);
		}

		/// <summary>
		/// Returns the next token from the source string, using the provided
		/// token delimiters
		/// </summary>
		/// <param name="delimiters">String containing the delimiters to use</param>
		/// <returns>The string value of the token</returns>
		public System.String NextToken(System.String delimiters)
		{
			//According to documentation, the usage of the received delimiters should be temporary (only for this call).
			//However, it seems it is not true, so the following line is necessary.
			this.delimiters = delimiters;

			//at the end 
			if (this.currentPos == this.chars.Length)
				throw new System.ArgumentOutOfRangeException();
			//if over a delimiter and delimiters must be returned
			else if (   (System.Array.IndexOf(delimiters.ToCharArray(),chars[this.currentPos]) != -1)
				     && this.includeDelims )                	
				return "" + this.chars[this.currentPos++];
			//need to get the token wo delimiters.
			else
				return nextToken(delimiters.ToCharArray());
		}

		//Returns the nextToken wo delimiters
		private System.String nextToken(char[] delimiters)
		{
			string token="";
			long pos = this.currentPos;

			//skip possible delimiters
			while (System.Array.IndexOf(delimiters,this.chars[currentPos]) != -1)
				//The last one is a delimiter (i.e there is no more tokens)
				if (++this.currentPos == this.chars.Length)
				{
					this.currentPos = pos;
					throw new System.ArgumentOutOfRangeException();
				}
			
			//getting the token
			while (System.Array.IndexOf(delimiters,this.chars[this.currentPos]) == -1)
			{
				token+=this.chars[this.currentPos];
				//the last one is not a delimiter
				if (++this.currentPos == this.chars.Length)
					break;
			}
			return token;
		}

				
		/// <summary>
		/// Determines if there are more tokens to return from the source string
		/// </summary>
		/// <returns>True or false, depending if there are more tokens</returns>
		public bool HasMoreTokens()
		{
			//keeping the current pos
			long pos = this.currentPos;
			
			try
			{
				this.NextToken();
			}
			catch (System.ArgumentOutOfRangeException)
			{				
				return false;
			}
			finally
			{
				this.currentPos = pos;
			}
			return true;
		}

		/// <summary>
		/// Remaining tokens count
		/// </summary>
		public int Count
		{
			get
			{
				//keeping the current pos
				long pos = this.currentPos;
				int i = 0;
			
				try
				{
					while (true)
					{
						this.NextToken();
						i++;
					}
				}
				catch (System.ArgumentOutOfRangeException)
				{				
					this.currentPos = pos;
					return i;
				}
			}
		}

		/// <summary>
		///  Performs the same action as NextToken.
		/// </summary>
		public System.Object Current
		{
			get
			{
				return (Object) this.NextToken();
			}		
		}		
		
		/// <summary>
		//  Performs the same action as HasMoreTokens.
		/// </summary>
		/// <returns>True or false, depending if there are more tokens</returns>
		public bool MoveNext()
		{
			return this.HasMoreTokens();
		}
		
		/// <summary>
		/// Does nothing.
		/// </summary>
		public void  Reset()
		{
			;
		}			
	}
	/*******************************/
	/// <summary>
	/// SupportClass for the BitArray class.
	/// </summary>
	public class BitArraySupport
	{
		/// <summary>
		/// Sets the specified bit to true.
		/// </summary>
		/// <param name="bits">The BitArray to modify.</param>
		/// <param name="index">The bit index to set to true.</param>
		public static void Set(System.Collections.BitArray bits, System.Int32 index)
		{
			for ( int increment = 0; index >= bits.Length; increment =+ 64)
			{
				bits.Length += increment;
			}

			bits.Set(index, true);
		}

		/// <summary>
		/// Returns a string representation of the BitArray object.
		/// </summary>
		/// <param name="bits">The BitArray object to convert to string.</param>
		/// <returns>A string representation of the BitArray object.</returns>
		public static System.String ToString(System.Collections.BitArray bits)
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			if (bits != null)
			{
				for(int i=0; i < bits.Length; i++)
				{
					if (bits[i] == true)
					{
						if (s.Length > 0)
							s.Append(", ");
						s.Append(i);					
					}
				}

				s.Insert(0, "{");
				s.Append("}");
			}
			else
				s.Insert(0, "null");

			return s.ToString();
		}
	}


	/*******************************/
	/// <summary>
	/// Receives a byte array and returns it transformed in an sbyte array
	/// </summary>
	/// <param name="byteArray">Byte array to process</param>
	/// <returns>The transformed array</returns>
	public static sbyte[] ToSByteArray(byte[] byteArray)
	{
		sbyte[] sbyteArray = null;
		if (byteArray != null)
		{
			sbyteArray = new sbyte[byteArray.Length];
			for(int index=0; index < byteArray.Length; index++)
				sbyteArray[index] = (sbyte) byteArray[index];
		}
		return sbyteArray;
	}

	/*******************************/
	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	public static int URShift(int number, int bits)
	{
		if ( number >= 0)
			return number >> bits;
		else
			return (number >> bits) + (2 << ~bits);
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	public static int URShift(int number, long bits)
	{
		return URShift(number, (int)bits);
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	public static long URShift(long number, int bits)
	{
		if ( number >= 0)
			return number >> bits;
		else
			return (number >> bits) + (2L << ~bits);
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	public static long URShift(long number, long bits)
	{
		return URShift(number, (int)bits);
	}

	/*******************************/
	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	public static long Identity(long literal)
	{
		return literal;
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	public static ulong Identity(ulong literal)
	{
		return literal;
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	public static float Identity(float literal)
	{
		return literal;
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	public static double Identity(double literal)
	{
		return literal;
	}

	/*******************************/
	/// <summary>
	/// Implements number format functions
	/// </summary>
    //[Serializable]
    //public class TextNumberFormat
    //{

    //    //Current localization number format infomation
    //    private System.Globalization.NumberFormatInfo numberFormat;
    //    //Enumeration of format types that can be used
    //    private enum formatTypes { General, Number, Currency, Percent };
    //    //Current format type used in the instance
    //    private int numberFormatType;
    //    //Indicates if grouping is being used
    //    private bool groupingActivated;
    //    //Current separator used
    //    private System.String separator;
    //    //Number of maximun digits in the integer portion of the number to represent the number
    //    private int maxIntDigits;
    //    //Number of minimum digits in the integer portion of the number to represent the number
    //    private int minIntDigits;
    //    //Number of maximun digits in the fraction portion of the number to represent the number
    //    private int maxFractionDigits;
    //    //Number of minimum digits in the integer portion of the number to represent the number
    //    private int minFractionDigits;

    //    /// <summary>
    //    /// Initializes a new instance of the object class with the default values
    //    /// </summary>
    //    public TextNumberFormat()
    //    {
    //        this.numberFormat      = new System.Globalization.NumberFormatInfo();
    //        this.numberFormatType  = (int)TextNumberFormat.formatTypes.General;
    //        this.groupingActivated = true;
    //        this.separator = this.GetSeparator( (int)TextNumberFormat.formatTypes.General );
    //        this.maxIntDigits = 127;
    //        this.minIntDigits = 1;
    //        this.maxFractionDigits = 3;
    //        this.minFractionDigits = 0;
    //    }

    //    /// <summary>
    //    /// Sets the Maximum integer digits value. 
    //    /// </summary>
    //    /// <param name="newValue">the new value for the maxIntDigits field</param>
    //    public void setMaximumIntegerDigits(int newValue)
    //    {
    //        maxIntDigits = newValue;
    //        if (newValue <= 0)
    //        {
    //            maxIntDigits = 0;
    //            minIntDigits = 0;
    //        }
    //        else if (maxIntDigits < minIntDigits)
    //        {
    //            minIntDigits = maxIntDigits;
    //        }
    //    }

    //    /// <summary>
    //    /// Sets the minimum integer digits value. 
    //    /// </summary>
    //    /// <param name="newValue">the new value for the minIntDigits field</param>
    //    public void setMinimumIntegerDigits(int newValue)
    //    {
    //        minIntDigits = newValue;
    //        if (newValue <= 0)
    //        {
    //            minIntDigits = 0;
    //        }
    //        else if (maxIntDigits < minIntDigits)
    //        {
    //            maxIntDigits = minIntDigits;
    //        }
    //    }

    //    /// <summary>
    //    /// Sets the maximum fraction digits value. 
    //    /// </summary>
    //    /// <param name="newValue">the new value for the maxFractionDigits field</param>
    //    public void setMaximumFractionDigits(int newValue)
    //    {
    //        maxFractionDigits = newValue;
    //        if (newValue <= 0)
    //        {
    //            maxFractionDigits = 0;
    //            minFractionDigits = 0;
    //        }
    //        else if (maxFractionDigits < minFractionDigits)
    //        {
    //            minFractionDigits = maxFractionDigits;
    //        }
    //    }
		
    //    /// <summary>
    //    /// Sets the minimum fraction digits value. 
    //    /// </summary>
    //    /// <param name="newValue">the new value for the minFractionDigits field</param>
    //    public void setMinimumFractionDigits(int newValue)
    //    {
    //        minFractionDigits = newValue;
    //        if (newValue <= 0)
    //        {
    //            minFractionDigits = 0;
    //        }
    //        else if (maxFractionDigits < minFractionDigits)
    //        {
    //            maxFractionDigits = minFractionDigits;
    //        }
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the class with the specified number format
    //    /// and the amount of fractional digits to use
    //    /// </summary>
    //    /// <param name="theType">Number format</param>
    //    /// <param name="digits">Number of fractional digits to use</param>
    //    private TextNumberFormat(TextNumberFormat.formatTypes theType, int digits)
    //    {
    //        this.numberFormat      = System.Globalization.NumberFormatInfo.CurrentInfo;
    //        this.numberFormatType  = (int)theType;
    //        this.groupingActivated = true;
    //        this.separator = this.GetSeparator( (int)theType );
    //        this.maxIntDigits = 127;
    //        this.minIntDigits = 1;
    //        this.maxFractionDigits = 3;
    //        this.minFractionDigits = 0;
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the class with the specified number format,
    //    /// uses the system's culture information,
    //    /// and assigns the amount of fractional digits to use
    //    /// </summary>
    //    /// <param name="theType">Number format</param>
    //    /// <param name="cultureNumberFormat">Represents information about a specific culture including the number formatting</param>
    //    /// <param name="digits">Number of fractional digits to use</param>
    //    private TextNumberFormat(TextNumberFormat.formatTypes theType, System.Globalization.CultureInfo cultureNumberFormat, int digits)
    //    {
    //        this.numberFormat      = cultureNumberFormat.NumberFormat;
    //        this.numberFormatType  = (int)theType;
    //        this.groupingActivated = true;
    //        this.separator = this.GetSeparator( (int)theType );
    //        this.maxIntDigits = 127;
    //        this.minIntDigits = 1;
    //        this.maxFractionDigits = 3;
    //        this.minFractionDigits = 0;
    //    }

    //    /// <summary>
    //    /// Returns an initialized instance of the TextNumberFormat object
    //    /// using number representation.
    //    /// </summary>
    //    /// <returns>The object instance</returns>
    //    public static TextNumberFormat getTextNumberInstance()
    //    {
    //        TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Number, 3);
    //        return instance;
    //    }

    //    /// <summary>
    //    /// Returns an initialized instance of the TextNumberFormat object
    //    /// using currency representation.
    //    /// </summary>
    //    /// <returns>The object instance</returns>
    //    public static TextNumberFormat getTextNumberCurrencyInstance()
    //    {
    //        TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Currency, 3);
    //        return instance.setToCurrencyNumberFormatDefaults(instance);
    //    }

    //    /// <summary>
    //    /// Returns an initialized instance of the TextNumberFormat object
    //    /// using percent representation.
    //    /// </summary>
    //    /// <returns>The object instance</returns>
    //    public static TextNumberFormat getTextNumberPercentInstance()
    //    {
    //        TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Percent, 3);
    //        return instance.setToPercentNumberFormatDefaults(instance);
    //    }

    //    /// <summary>
    //    /// Returns an initialized instance of the TextNumberFormat object
    //    /// using number representation, it uses the culture format information provided.
    //    /// </summary>
    //    /// <param name="culture">Represents information about a specific culture</param>
    //    /// <returns>The object instance</returns>
    //    public static TextNumberFormat getTextNumberInstance(System.Globalization.CultureInfo culture)
    //    {
    //        TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Number, culture, 3);
    //        return instance;
    //    }

    //    /// <summary>
    //    /// Returns an initialized instance of the TextNumberFormat object
    //    /// using currency representation, it uses the culture format information provided.
    //    /// </summary>
    //    /// <param name="culture">Represents information about a specific culture</param>
    //    /// <returns>The object instance</returns>
    //    public static TextNumberFormat getTextNumberCurrencyInstance(System.Globalization.CultureInfo culture)
    //    {
    //        TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Currency, culture, 3);
    //        return instance.setToCurrencyNumberFormatDefaults(instance);
    //    }

    //    /// <summary>
    //    /// Returns an initialized instance of the TextNumberFormat object
    //    /// using percent representation, it uses the culture format information provided.
    //    /// </summary>
    //    /// <param name="culture">Represents information about a specific culture</param>
    //    /// <returns>The object instance</returns>
    //    public static TextNumberFormat getTextNumberPercentInstance(System.Globalization.CultureInfo culture)
    //    {
    //        TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Percent, culture, 3);
    //        return instance.setToPercentNumberFormatDefaults(instance);
    //    }

    //    /// <summary>
    //    /// Clones the object instance
    //    /// </summary>
    //    /// <returns>The cloned object instance</returns>
    //    public System.Object Clone()
    //    {
    //        return (System.Object)this;
    //    }

    //    /// <summary>
    //    /// Determines if the received object is equal to the
    //    /// current object instance
    //    /// </summary>
    //    /// <param name="textNumberObject">TextNumber instance to compare</param>
    //    /// <returns>True or false depending if the two instances are equal</returns>
    //    public override bool Equals(Object obj) 
    //    {
    //        // Check for null values and compare run-time types.
    //        if (obj == null || GetType() != obj.GetType()) 
    //            return false;
    //        SupportClass.TextNumberFormat param = (SupportClass.TextNumberFormat)obj;
    //        return (numberFormat == param.numberFormat) && (numberFormatType == param.numberFormatType) 
    //            && (groupingActivated == param.groupingActivated) && (separator == param.separator) 
    //            && (maxIntDigits == param.maxIntDigits)	&& (minIntDigits == param.minIntDigits) 
    //            && (maxFractionDigits == param.maxFractionDigits) && (minFractionDigits == param.minFractionDigits);
    //    }

		
    //    /// <summary>
    //    /// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
    //    /// </summary>
    //    /// <returns>A hash code for the current Object</returns>
    //    public override int GetHashCode()
    //    {
    //        return numberFormat.GetHashCode() ^ numberFormatType ^ groupingActivated.GetHashCode() 
    //             ^ separator.GetHashCode() ^ maxIntDigits ^ minIntDigits ^ maxFractionDigits ^ minFractionDigits;
    //    }

    //    /// <summary>
    //    /// Formats a number with the current formatting parameters
    //    /// </summary>
    //    /// <param name="number">Source number to format</param>
    //    /// <returns>The formatted number string</returns>
    //    public System.String FormatDouble(double number)
    //    {
    //        if (this.groupingActivated)
    //        {
    //            return SetIntDigits(number.ToString(this.GetCurrentFormatString() + this.GetNumberOfDigits( number ), this.numberFormat));
    //        }
    //        else
    //        {
    //            return SetIntDigits((number.ToString(this.GetCurrentFormatString() + this.GetNumberOfDigits( number ) , this.numberFormat)).Replace(this.separator,""));
    //        }
    //    }
		
    //    /// <summary>
    //    /// Formats a number with the current formatting parameters
    //    /// </summary>
    //    /// <param name="number">Source number to format</param>
    //    /// <returns>The formatted number string</returns>
    //    public System.String FormatLong(long number)
    //    {			
    //        if (this.groupingActivated)
    //        {
    //            return SetIntDigits(number.ToString(this.GetCurrentFormatString() + this.minFractionDigits , this.numberFormat));
    //        }
    //        else
    //        {
    //            return SetIntDigits((number.ToString(this.GetCurrentFormatString() + this.minFractionDigits , this.numberFormat)).Replace(this.separator,""));
    //        }
    //    }
		
		
    //    /// <summary>
    //    /// Formats the number according to the specified number of integer digits 
    //    /// </summary>
    //    /// <param name="number">The number to format</param>
    //    /// <returns></returns>
    //    private System.String SetIntDigits(String number)
    //    {			
    //        String decimals = "";
    //        String fraction = "";
    //        int i = number.IndexOf(this.numberFormat.NumberDecimalSeparator);
    //        if (i > 0)
    //        {
    //            fraction = number.Substring(i);
    //            decimals = number.Substring(0,i).Replace(this.numberFormat.NumberGroupSeparator,"");
    //        }
    //        else decimals = number.Replace(this.numberFormat.NumberGroupSeparator,"");
    //        decimals = decimals.PadLeft(this.MinIntDigits,'0');
    //        if ((i = decimals.Length - this.MaxIntDigits) > 0) decimals = decimals.Remove(0,i);
    //        if (this.groupingActivated) 
    //        {
    //            for (i = decimals.Length;i > 3;i -= 3)
    //            {
    //                decimals = decimals.Insert(i - 3,this.numberFormat.NumberGroupSeparator);
    //            }
    //        }
    //        decimals = decimals + fraction;
    //        if (decimals.Length == 0) return "0";
    //        else return decimals;
    //    }

    //    /// <summary>
    //    /// Gets the list of all supported cultures
    //    /// </summary>
    //    /// <returns>An array of type CultureInfo that represents the supported cultures</returns>
    //    public static System.Globalization.CultureInfo[] GetAvailableCultures()
    //    {
    //        return System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures);
    //    }

    //    /// <summary>
    //    /// Obtains the current format representation used
    //    /// </summary>
    //    /// <returns>A character representing the string format used</returns>
    //    private System.String GetCurrentFormatString()
    //    {
    //        System.String currentFormatString = "n";  //Default value
    //        switch (this.numberFormatType)
    //        {
    //            case (int)TextNumberFormat.formatTypes.Currency:
    //                currentFormatString = "c";
    //                break;

    //            case (int)TextNumberFormat.formatTypes.General:
    //                currentFormatString = "n";
    //                break;

    //            case (int)TextNumberFormat.formatTypes.Number:
    //                currentFormatString = "n";
    //                break;

    //            case (int)TextNumberFormat.formatTypes.Percent:
    //                currentFormatString = "p";
    //                break;
    //        }
    //        return currentFormatString;
    //    }

    //    /// <summary>
    //    /// Retrieves the separator used, depending on the format type specified
    //    /// </summary>
    //    /// <param name="numberFormatType">formatType enumarator value to inquire</param>
    //    /// <returns>The values of character separator used </returns>
    //    private System.String GetSeparator(int numberFormatType)
    //    {
    //        System.String separatorItem = " ";  //Default Separator

    //        switch (numberFormatType)
    //        {
    //            case (int)TextNumberFormat.formatTypes.Currency:
    //                separatorItem = this.numberFormat.CurrencyGroupSeparator;
    //                break;

    //            case (int)TextNumberFormat.formatTypes.General:
    //                separatorItem = this.numberFormat.NumberGroupSeparator;
    //                break;

    //            case (int)TextNumberFormat.formatTypes.Number:
    //                separatorItem = this.numberFormat.NumberGroupSeparator;
    //                break;

    //            case (int)TextNumberFormat.formatTypes.Percent:
    //                separatorItem = this.numberFormat.PercentGroupSeparator;
    //                break;
    //        }
    //        return separatorItem;
    //    }

    //    /// <summary>
    //    /// Boolean value stating if grouping is used or not
    //    /// </summary>
    //    public bool GroupingUsed
    //    {
    //        get
    //        {
    //            return (this.groupingActivated);
    //        }
    //        set
    //        {
    //            this.groupingActivated = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Minimum number of integer digits to use in the number format
    //    /// </summary>
    //    public int MinIntDigits
    //    {
    //        get
    //        {
    //            return this.minIntDigits;
    //        }
    //        set
    //        {
    //            this.minIntDigits = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Maximum number of integer digits to use in the number format
    //    /// </summary>
    //    public int MaxIntDigits
    //    {
    //        get
    //        {
    //            return this.maxIntDigits;
    //        }
    //        set
    //        {
    //            this.maxIntDigits = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Minimum number of fraction digits to use in the number format
    //    /// </summary>
    //    public int MinFractionDigits
    //    {
    //        get
    //        {
    //            return this.minFractionDigits;
    //        }
    //        set
    //        {
    //            this.minFractionDigits = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Maximum number of fraction digits to use in the number format
    //    /// </summary>
    //    public int MaxFractionDigits
    //    {
    //        get
    //        {
    //            return this.maxFractionDigits;
    //        }
    //        set
    //        {
    //            this.maxFractionDigits = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Sets the values of minFractionDigits and maxFractionDigits to the currency standard
    //    /// </summary>
    //    /// <param name="format">The TextNumberFormat instance to set</param>
    //    /// <returns>The TextNumberFormat with corresponding the default values</returns>
    //    private TextNumberFormat setToCurrencyNumberFormatDefaults( TextNumberFormat format )
    //    {
    //        format.maxFractionDigits = 2;
    //        format.minFractionDigits = 2;
    //        return format;
    //    }

    //    /// <summary>
    //    /// Sets the values of minFractionDigits and maxFractionDigits to the percent standard
    //    /// </summary>
    //    /// <param name="format">The TextNumberFormat instance to set</param>
    //    /// <returns>The TextNumberFormat with corresponding the default values</returns>
    //    private TextNumberFormat setToPercentNumberFormatDefaults( TextNumberFormat format )
    //    {
    //        format.maxFractionDigits = 0;
    //        format.minFractionDigits = 0;
    //        return format;
    //    }

    //    /// <summary>
    //    /// Gets the number of fraction digits thats must be used by the format methods
    //    /// </summary>
    //    /// <param name="number">The double number</param>
    //    /// <returns>The number of fraction digits to use</returns>
    //    private int GetNumberOfDigits( Double number )
    //    {
    //        int counter = 0;
    //        double temp = System.Math.Abs(number);
    //        while ( (temp % 1) > 0 )
    //        {
    //            temp *= 10;
    //            counter++;
    //        }
    //        return (counter < this.minFractionDigits) ? this.minFractionDigits : (( counter < this.maxFractionDigits ) ? counter : this.maxFractionDigits); 
    //    }
    //}
	/*******************************/
/// <summary>
/// Provides support for DateFormat
/// </summary>
//public class DateTimeFormatManager
//{
//    static public DateTimeFormatHashTable manager = new DateTimeFormatHashTable();

//    /// <summary>
//    /// Hashtable class to provide functionality for dateformat properties
//    /// </summary>
//    public class DateTimeFormatHashTable :System.Collections.Hashtable 
//    {
//        /// <summary>
//        /// Sets the format for datetime.
//        /// </summary>
//        /// <param name="format">DateTimeFormat instance to set the pattern</param>
//        /// <param name="newPattern">A string with the pattern format</param>
//        public void SetDateFormatPattern(System.Globalization.DateTimeFormatInfo format, System.String newPattern)
//        {
//            if (this[format] != null)
//                ((DateTimeFormatProperties) this[format]).DateFormatPattern = newPattern;
//            else
//            {
//                DateTimeFormatProperties tempProps = new DateTimeFormatProperties();
//                tempProps.DateFormatPattern  = newPattern;
//                Add(format, tempProps);
//            }
//        }

//        /// <summary>
//        /// Gets the current format pattern of the DateTimeFormat instance
//        /// </summary>
//        /// <param name="format">The DateTimeFormat instance which the value will be obtained</param>
//        /// <returns>The string representing the current datetimeformat pattern</returns>
//        public System.String GetDateFormatPattern(System.Globalization.DateTimeFormatInfo format)
//        {
//            if (this[format] == null)
//                return "d-MMM-yy";
//            else
//                return ((DateTimeFormatProperties) this[format]).DateFormatPattern;
//        }
		
//        /// <summary>
//        /// Sets the datetimeformat pattern to the giving format
//        /// </summary>
//        /// <param name="format">The datetimeformat instance to set</param>
//        /// <param name="newPattern">The new datetimeformat pattern</param>
//        public void SetTimeFormatPattern(System.Globalization.DateTimeFormatInfo format, System.String newPattern)
//        {
//            if (this[format] != null)
//                ((DateTimeFormatProperties) this[format]).TimeFormatPattern = newPattern;
//            else
//            {
//                DateTimeFormatProperties tempProps = new DateTimeFormatProperties();
//                tempProps.TimeFormatPattern  = newPattern;
//                Add(format, tempProps);
//            }
//        }

//        /// <summary>
//        /// Gets the current format pattern of the DateTimeFormat instance
//        /// </summary>
//        /// <param name="format">The DateTimeFormat instance which the value will be obtained</param>
//        /// <returns>The string representing the current datetimeformat pattern</returns>
//        public System.String GetTimeFormatPattern(System.Globalization.DateTimeFormatInfo format)
//        {
//            if (this[format] == null)
//                return "h:mm:ss tt";
//            else
//                return ((DateTimeFormatProperties) this[format]).TimeFormatPattern;
//        }

//        /// <summary>
//        /// Internal class to provides the DateFormat and TimeFormat pattern properties on .NET
//        /// </summary>
//        class DateTimeFormatProperties
//        {
//            public System.String DateFormatPattern = "d-MMM-yy";
//            public System.String TimeFormatPattern = "h:mm:ss tt";
//        }
//    }	
//}
	/*******************************/
	/// <summary>
	/// Gets the DateTimeFormat instance and date instance to obtain the date with the format passed
	/// </summary>
	/// <param name="format">The DateTimeFormat to obtain the time and date pattern</param>
	/// <param name="date">The date instance used to get the date</param>
    ///// <returns>A string representing the date with the time and date patterns</returns>
    //public static System.String FormatDateTime(System.Globalization.DateTimeFormatInfo format, System.DateTime date)
    //{
    //    System.String timePattern = DateTimeFormatManager.manager.GetTimeFormatPattern(format);
    //    System.String datePattern = DateTimeFormatManager.manager.GetDateFormatPattern(format);
    //    return date.ToString(datePattern + " " + timePattern, format);            
    //}

	/*******************************/
	/// <summary>
	/// Encapsulates the functionality of message digest algorithms such as SHA-1 or MD5.
	/// </summary>
    //public class MessageDigestSupport
    //{
    //    private System.Security.Cryptography.HashAlgorithm algorithm;
    //    private byte[] data = new byte[0];
    //    private int position;
    //    private System.String algorithmName;

    //    /// <summary>
    //    /// The HashAlgorithm instance that provide the cryptographic hash algorithm
    //    /// </summary>
    //    public System.Security.Cryptography.HashAlgorithm Algorithm
    //    {
    //        get
    //        {
    //            return this.algorithm;
    //        }
    //        set
    //        {
    //            this.algorithm  = value;
    //        }
    //    }

    //    /// <summary>
    //    /// The digest data
    //    /// </summary>
    //    public byte[] Data
    //    {
    //        get
    //        {
    //            return this.data;
    //        }
    //        set
    //        {
    //            this.data  = value;
    //        }
    //    }

    //    /// <summary>
    //    /// The name of the cryptographic hash algorithm used in the instance
    //    /// </summary>
    //    public System.String AlgorithmName
    //    {
    //        get
    //        {
    //            return this.algorithmName;
    //        }
    //    }

    //    /// <summary>
    //    /// Creates a message digest using the specified name to set Algorithm property.
    //    /// </summary>
    //    /// <param name="algorithm">The name of the algorithm to use</param>
    //    public MessageDigestSupport(System.String algorithm)
    //    {			
    //        if (algorithm.Equals("SHA-1"))
    //        {
    //            this.algorithmName = "SHA";
    //        }
    //        else 
    //        {
    //            this.algorithmName = algorithm;
    //        }
    //        this.Algorithm = (System.Security.Cryptography.HashAlgorithm) System.Security.Cryptography.CryptoConfig.CreateFromName(this.algorithmName);			
    //        this.data = new byte[0];
    //        this.position  = 0;
    //    }

    //    /// <summary>
    //    /// Computes the hash value for the internal data digest.
    //    /// </summary>
    //    /// <returns>The array of signed bytes with the resulting hash value</returns>
    //    public sbyte[] DigestData()
    //    {
    //        sbyte[] result = ToSByteArray(this.Algorithm.ComputeHash(this.data));
    //        this.Reset();
    //        return result;
    //    }

    //    /// <summary>
    //    /// Performs and update on the digest with the specified array and then completes the digest
    //    /// computation.
    //    /// </summary>
    //    /// <param name="newData">The array of bytes for final update to the digest</param>
    //    /// <returns>An array of signed bytes with the resulting hash value</returns>
    //    public sbyte[] DigestData(sbyte[] newData)
    //    {
    //        this.Update(ToByteArray(newData));
    //        return this.DigestData();
    //    }


    //    /// <summary>
    //    /// Computes the hash value for the internal digest and places the digest returned into the specified buffer
    //    /// </summary>
    //    /// <param name="buff">The buffer for the output digest</param>
    //    /// <param name="offset">Offset into the buffer for the beginning index</param>
    //    /// <param name="length">Total number of bytes for the digest</param>
    //    /// <returns>The number of bytes placed into the output buffer</returns>
    //    public int DigestData(sbyte[] buffer, int offset, int length)
    //    {
    //            byte[] result = this.Algorithm.ComputeHash(this.data);
    //        int count = 0;
    //        if ( length >= this.GetDigestLength() )
    //        {
    //            if ( buffer.Length >= (length + offset) )
    //            {
    //                for ( ; count < result.Length ; count++ )
    //                {
    //                    buffer[offset + count] = (sbyte)result[count];						
    //                }
    //            }
    //            else
    //            {
    //                throw new System.ArgumentException("output buffer too small for the specified offset and length");
    //            }
    //        }
    //        else
    //        {
    //            throw new System.Exception("Partial digests not returned");
    //        }
    //        return count;
    //    }

    //    /// <summary>
    //    /// Updates the digest data with the specified array of bytes by making an append
    //    /// operation in the internal array of data.
    //    /// </summary>
    //    /// <param name="newData">The array of bytes for the update operation</param>
    //    public void Update(byte[] newData)
    //    {
    //        if (position == 0)
    //        {
    //            this.Data = newData;
    //            this.position = this.Data.Length - 1;
    //        }
    //        else
    //        {
    //            byte[] oldData = this.Data;
    //            this.Data = new byte[newData.Length + position + 1];
    //            oldData.CopyTo(this.Data, 0);
    //            newData.CopyTo(this.Data, oldData.Length);
	            
    //            this.position = this.Data.Length - 1;
    //        }
    //    }
        
    //    /// <summary>
    //    /// Updates the digest data with the input byte by calling the method Update with an array.
    //    /// </summary>
    //    /// <param name="newData">The input byte for the update</param>
    //    public void Update(byte newData)
    //    {
    //        byte[] newDataArray = new byte[1];
    //        newDataArray[0] = newData;
    //        this.Update(newDataArray);
    //    }

    //    /// <summary>
    //    /// Updates the specified count of bytes with the input array of bytes starting at the
    //    /// input offset.
    //    /// </summary>
    //    /// <param name="newData">The array of bytes for the update operation</param>
    //    /// <param name="offset">The initial position to start from in the array of bytes</param>
    //    /// <param name="count">The number of bytes fot the update</param>
    //    public void Update(byte[] newData, int offset, int count)
    //    {
    //        byte[] newDataArray = new byte[count];
    //        System.Array.Copy(newData, offset, newDataArray, 0, count);
    //        this.Update(newDataArray);
    //    }
		
    //    /// <summary>
    //    /// Resets the digest data to the initial state.
    //    /// </summary>
    //    public void Reset()
    //    {
    //        this.data = null;
    //        this.position = 0;
    //    }

    //    /// <summary>
    //    /// Returns a string representation of the Message Digest
    //    /// </summary>
    //    /// <returns>A string representation of the object</returns>
    //    public override System.String ToString()
    //    {
    //        return this.Algorithm.ToString();
    //    }

    //    /// <summary>
    //    /// Generates a new instance of the MessageDigestSupport class using the specified algorithm
    //    /// </summary>
    //    /// <param name="algorithm">The name of the algorithm to use</param>
    //    /// <returns>A new instance of the MessageDigestSupport class</returns>
    //    public static MessageDigestSupport GetInstance(System.String algorithm)
    //    {
    //        return new MessageDigestSupport(algorithm);
    //    }
		
    //    /// <summary>
    //    /// Compares two arrays of signed bytes evaluating equivalence in digest data
    //    /// </summary>
    //    /// <param name="firstDigest">An array of signed bytes for comparison</param>
    //    /// <param name="secondDigest">An array of signed bytes for comparison</param>
    //    /// <returns>True if the input digest arrays are equal</returns>
    //    public static bool EquivalentDigest(System.SByte[] firstDigest, System.SByte[] secondDigest)
    //    {
    //        bool result = false;
    //        if (firstDigest.Length == secondDigest.Length)
    //        {
    //            int index = 0;
    //            result = true;
    //            while(result && index < firstDigest.Length)
    //            {
    //                result = firstDigest[index] == secondDigest[index];
    //                index++;
    //            }
    //        }
			
    //        return result;
    //    }


    //    /// <summary>
    //    /// Gets a number of bytes representing the length of the digest
    //    /// </summary>
    //    /// <returns>The length of the digest in bytes</returns>
    //    public int GetDigestLength( )
    //    {
    //        return this.algorithm.HashSize / 8;
    //    }
    //}
	/*******************************/
	/// <summary>
	/// Represents the methods to support some operations over files.
	/// </summary>
	public class FileSupport
	{
		/// <summary>
		/// Creates a new empty file with the specified pathname.
		/// </summary>
		/// <param name="path">The abstract pathname of the file</param>
		/// <returns>True if the file does not exist and was succesfully created</returns>
		public static bool CreateNewFile(System.IO.FileInfo path)
		{
			if (path.Exists)
			{
				return false;
			}
			else
			{
                System.IO.FileStream createdFile = path.Create();
                createdFile.Close();
				return true;
			}
		}

		/// <summary>
		/// Compares the specified object with the specified path
		/// </summary>
		/// <param name="path">An abstract pathname to compare with</param>
		/// <param name="file">An object to compare with the given pathname</param>
		/// <returns>A value indicating a lexicographically comparison of the parameters</returns>
		public static int CompareTo(System.IO.FileInfo path, System.Object file)
		{
			if( file is System.IO.FileInfo )
			{
				System.IO.FileInfo fileInfo = (System.IO.FileInfo)file;
				return path.FullName.CompareTo( fileInfo.FullName );
			}
			else
			{
				throw new System.InvalidCastException();
			}
		}

		/// <summary>
		/// Returns an array of abstract pathnames representing the files and directories of the specified path.
		/// </summary>
		/// <param name="path">The abstract pathname to list it childs.</param>
		/// <returns>An array of abstract pathnames childs of the path specified or null if the path is not a directory</returns>
		public static System.IO.FileInfo[] GetFiles(System.IO.FileInfo path)
		{
			if ( (path.Attributes & System.IO.FileAttributes.Directory) > 0 )
			{																 
				String[] fullpathnames = System.IO.Directory.GetFileSystemEntries(path.FullName);
				System.IO.FileInfo[] result = new System.IO.FileInfo[fullpathnames.Length];
				for(int i = 0; i < result.Length ; i++)
					result[i] = new System.IO.FileInfo(fullpathnames[i]);
				return result;
			}
			else return null;
		}

		/// <summary>
		/// Creates an instance of System.Uri class with the pech specified
		/// </summary>
		/// <param name="path">The abstract path name to create the Uri</param>
		/// <returns>A System.Uri instance constructed with the specified path</returns>
		public static System.Uri ToUri(System.IO.FileInfo path)
		{
			System.UriBuilder uri = new System.UriBuilder();
			uri.Path = path.FullName;
			uri.Host = String.Empty;
			uri.Scheme = System.Uri.UriSchemeFile;
			return uri.Uri;
		}

		/// <summary>
		/// Returns true if the file specified by the pathname is a hidden file.
		/// </summary>
		/// <param name="file">The abstract pathname of the file to test</param>
		/// <returns>True if the file is hidden, false otherwise</returns>
		public static bool IsHidden(System.IO.FileInfo file)
		{
			return ((file.Attributes & System.IO.FileAttributes.Hidden) > 0); 
		}

		/// <summary>
		/// Sets the read-only property of the file to true.
		/// </summary>
		/// <param name="file">The abstract path name of the file to modify</param>
		public static bool SetReadOnly(System.IO.FileInfo file)
		{
			try 
			{
				file.Attributes = file.Attributes | System.IO.FileAttributes.ReadOnly;
				return true;
			}
			catch (System.Exception exception)
			{
				String exceptionMessage = exception.Message;
				return false;
			}
		}

		/// <summary>
		/// Sets the last modified time of the specified file with the specified value.
		/// </summary>
		/// <param name="file">The file to change it last-modified time</param>
		/// <param name="date">Total number of miliseconds since January 1, 1970 (new last-modified time)</param>
		/// <returns>True if the operation succeeded, false otherwise</returns>
		public static bool SetLastModified(System.IO.FileInfo file, long date)
		{
			try 
			{
				long valueConstant = (new System.DateTime(1969, 12, 31, 18, 0, 0)).Ticks;
				file.LastWriteTime = new System.DateTime( (date * 10000L) + valueConstant );
				return true;
			}
			catch (System.Exception exception)
			{
				String exceptionMessage = exception.Message;
				return false;
			}
		}
	}
	/*******************************/
	/// <summary>
	/// This class manages different features for calendars.
	/// The different calendars are internally managed using a hashtable structure.
	/// </summary>
	public class CalendarManager
	{
		/// <summary>
		/// Field used to get or set the year.
		/// </summary>
		public const int YEAR = 1;

		/// <summary>
		/// Field used to get or set the month.
		/// </summary>
		public const int MONTH = 2;
		
		/// <summary>
		/// Field used to get or set the day of the month.
		/// </summary>
		public const int DATE = 5;
		
		/// <summary>
		/// Field used to get or set the hour of the morning or afternoon.
		/// </summary>
		public const int HOUR = 10;
		
		/// <summary>
		/// Field used to get or set the minute within the hour.
		/// </summary>
		public const int MINUTE = 12;
		
		/// <summary>
		/// Field used to get or set the second within the minute.
		/// </summary>
		public const int SECOND = 13;
		
		/// <summary>
		/// Field used to get or set the millisecond within the second.
		/// </summary>
		public const int MILLISECOND = 14;
		
		/// <summary>
		/// Field used to get or set the day of the year.
		/// </summary>
		public const int DAY_OF_YEAR = 4;
		
		/// <summary>
		/// Field used to get or set the day of the month.
		/// </summary>
		public const int DAY_OF_MONTH = 6;
		
		/// <summary>
		/// Field used to get or set the day of the week.
		/// </summary>
		public const int DAY_OF_WEEK = 7;
		
		/// <summary>
		/// Field used to get or set the hour of the day.
		/// </summary>
		public const int HOUR_OF_DAY = 11;
		
		/// <summary>
		/// Field used to get or set whether the HOUR is before or after noon.
		/// </summary>
		public const int AM_PM = 9;
		
		/// <summary>
		/// Field used to get or set the value of the AM_PM field which indicates the period of the day from midnight to just before noon.
		/// </summary>
		public const int AM = 0;
		
		/// <summary>
		/// Field used to get or set the value of the AM_PM field which indicates the period of the day from noon to just before midnight.
		/// </summary>
		public const int PM = 1;
		
		/// <summary>
		/// The hashtable that contains the calendars and its properties.
		/// </summary>
		static public CalendarHashTable manager = new CalendarHashTable();

		/// <summary>
		/// Internal class that inherits from HashTable to manage the different calendars.
		/// This structure will contain an instance of System.Globalization.Calendar that represents 
		/// a type of calendar and its properties (represented by an instance of CalendarProperties 
		/// class).
		/// </summary>
		public class CalendarHashTable:System.Collections.Hashtable 
		{
			/// <summary>
			/// Gets the calendar current date and time.
			/// </summary>
			/// <param name="calendar">The calendar to get its current date and time.</param>
			/// <returns>A System.DateTime value that indicates the current date and time for the 
			/// calendar given.</returns>
			public System.DateTime GetDateTime(System.Globalization.Calendar calendar)
			{
				if (this[calendar] != null)
					return ((CalendarProperties) this[calendar]).dateTime;
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					this.Add(calendar, tempProps);
					return this.GetDateTime(calendar);
				}
			}

			/// <summary>
			/// Sets the specified System.DateTime value to the specified calendar.
			/// </summary>
			/// <param name="calendar">The calendar to set its date.</param>
			/// <param name="date">The System.DateTime value to set to the calendar.</param>
			public void SetDateTime(System.Globalization.Calendar calendar, System.DateTime date)
			{
				if (this[calendar] != null)
				{
					((CalendarProperties) this[calendar]).dateTime = date;
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = date;
					this.Add(calendar, tempProps);
				}
			}

			/// <summary>
			/// Sets the corresponding field in an specified calendar with the value given.
			/// If the specified calendar does not have exist in the hash table, it creates a 
			/// new instance of the calendar with the current date and time and then assings it 
			/// the new specified value.
			/// </summary>
			/// <param name="calendar">The calendar to set its date or time.</param>
			/// <param name="field">One of the fields that composes a date/time.</param>
			/// <param name="fieldValue">The value to be set.</param>
			public void Set(System.Globalization.Calendar calendar, int field, int fieldValue)
			{
				if (this[calendar] != null)
				{
					System.DateTime tempDate = ((CalendarProperties) this[calendar]).dateTime;
					switch (field)
					{
						case CalendarManager.DATE:
							tempDate = tempDate.AddDays(fieldValue - tempDate.Day);
							break;
						case CalendarManager.HOUR:
							tempDate = tempDate.AddHours(fieldValue - tempDate.Hour);
							break;
						case CalendarManager.MILLISECOND:
							tempDate = tempDate.AddMilliseconds(fieldValue - tempDate.Millisecond);
							break;
						case CalendarManager.MINUTE:
							tempDate = tempDate.AddMinutes(fieldValue - tempDate.Minute);
							break;
						case CalendarManager.MONTH:
							//Month value is 0-based. e.g., 0 for January
							tempDate = tempDate.AddMonths((fieldValue + 1) - tempDate.Month);
							break;
						case CalendarManager.SECOND:
							tempDate = tempDate.AddSeconds(fieldValue - tempDate.Second);
							break;
						case CalendarManager.YEAR:
							tempDate = tempDate.AddYears(fieldValue - tempDate.Year);
							break;
						case CalendarManager.DAY_OF_MONTH:
							tempDate = tempDate.AddDays(fieldValue - tempDate.Day);
							break;
						case CalendarManager.DAY_OF_WEEK:
							tempDate = tempDate.AddDays((fieldValue - 1) - (int)tempDate.DayOfWeek);
							break;
						case CalendarManager.DAY_OF_YEAR:
							tempDate = tempDate.AddDays(fieldValue - tempDate.DayOfYear);
							break;
						case CalendarManager.HOUR_OF_DAY:
							tempDate = tempDate.AddHours(fieldValue - tempDate.Hour);
							break;

						default:
							break;
					}
					((CalendarProperties) this[calendar]).dateTime = tempDate;
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					this.Add(calendar, tempProps);
					this.Set(calendar, field, fieldValue);
				}
			}

			/// <summary>
			/// Sets the corresponding date (day, month and year) to the calendar specified.
			/// If the calendar does not exist in the hash table, it creates a new instance and sets 
			/// its values.
			/// </summary>
			/// <param name="calendar">The calendar to set its date.</param>
			/// <param name="year">Integer value that represent the year.</param>
			/// <param name="month">Integer value that represent the month.</param>
			/// <param name="day">Integer value that represent the day.</param>
			public void Set(System.Globalization.Calendar calendar, int year, int month, int day)
			{
				if (this[calendar] != null)
				{
					this.Set(calendar, CalendarManager.YEAR, year);
					this.Set(calendar, CalendarManager.MONTH, month);
					this.Set(calendar, CalendarManager.DATE, day);
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					this.Add(calendar, tempProps);
					this.Set(calendar, year, month, day);
				}
			}

			/// <summary>
			/// Sets the corresponding date (day, month and year) and hour (hour and minute) 
			/// to the calendar specified.
			/// If the calendar does not exist in the hash table, it creates a new instance and sets 
			/// its values.
			/// </summary>
			/// <param name="calendar">The calendar to set its date and time.</param>
			/// <param name="year">Integer value that represent the year.</param>
			/// <param name="month">Integer value that represent the month.</param>
			/// <param name="day">Integer value that represent the day.</param>
			/// <param name="hour">Integer value that represent the hour.</param>
			/// <param name="minute">Integer value that represent the minutes.</param>
			public void Set(System.Globalization.Calendar calendar, int year, int month, int day, int hour, int minute)
			{
				if (this[calendar] != null)
				{
					this.Set(calendar, CalendarManager.YEAR, year);
					this.Set(calendar, CalendarManager.MONTH, month);
					this.Set(calendar, CalendarManager.DATE, day);
					this.Set(calendar, CalendarManager.HOUR, hour);
					this.Set(calendar, CalendarManager.MINUTE, minute);
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					this.Add(calendar, tempProps);
					this.Set(calendar, year, month, day, hour, minute);
				}
			}

			/// <summary>
			/// Sets the corresponding date (day, month and year) and hour (hour, minute and second) 
			/// to the calendar specified.
			/// If the calendar does not exist in the hash table, it creates a new instance and sets 
			/// its values.
			/// </summary>
			/// <param name="calendar">The calendar to set its date and time.</param>
			/// <param name="year">Integer value that represent the year.</param>
			/// <param name="month">Integer value that represent the month.</param>
			/// <param name="day">Integer value that represent the day.</param>
			/// <param name="hour">Integer value that represent the hour.</param>
			/// <param name="minute">Integer value that represent the minutes.</param>
			/// <param name="second">Integer value that represent the seconds.</param>
			public void Set(System.Globalization.Calendar calendar, int year, int month, int day, int hour, int minute, int second)
			{
				if (this[calendar] != null)
				{
					this.Set(calendar, CalendarManager.YEAR, year);
					this.Set(calendar, CalendarManager.MONTH, month);
					this.Set(calendar, CalendarManager.DATE, day);
					this.Set(calendar, CalendarManager.HOUR, hour);
					this.Set(calendar, CalendarManager.MINUTE, minute);
					this.Set(calendar, CalendarManager.SECOND, second);
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					this.Add(calendar, tempProps);
					this.Set(calendar, year, month, day, hour, minute, second);
				}
			}

			/// <summary>
			/// Gets the value represented by the field specified.
			/// </summary>
			/// <param name="calendar">The calendar to get its date or time.</param>
			/// <param name="field">One of the field that composes a date/time.</param>
			/// <returns>The integer value for the field given.</returns>
			public int Get(System.Globalization.Calendar calendar, int field)
			{
				if (this[calendar] != null)
				{
					int tempHour;
					switch (field)
					{
						case CalendarManager.DATE:
							return ((CalendarProperties) this[calendar]).dateTime.Day;
						case CalendarManager.HOUR:
							tempHour = ((CalendarProperties) this[calendar]).dateTime.Hour;
							return tempHour > 12 ? tempHour - 12 : tempHour;
						case CalendarManager.MILLISECOND:
							return ((CalendarProperties) this[calendar]).dateTime.Millisecond;
						case CalendarManager.MINUTE:
							return ((CalendarProperties) this[calendar]).dateTime.Minute;
						case CalendarManager.MONTH:
							//Month value is 0-based. e.g., 0 for January
							return ((CalendarProperties) this[calendar]).dateTime.Month - 1;
						case CalendarManager.SECOND:
							return ((CalendarProperties) this[calendar]).dateTime.Second;
						case CalendarManager.YEAR:
							return ((CalendarProperties) this[calendar]).dateTime.Year;
						case CalendarManager.DAY_OF_MONTH:
							return ((CalendarProperties) this[calendar]).dateTime.Day;
						case CalendarManager.DAY_OF_YEAR:							
							return (int)(((CalendarProperties) this[calendar]).dateTime.DayOfYear);
						case CalendarManager.DAY_OF_WEEK:
							return (int)(((CalendarProperties) this[calendar]).dateTime.DayOfWeek) + 1;
						case CalendarManager.HOUR_OF_DAY:
							return ((CalendarProperties) this[calendar]).dateTime.Hour;
						case CalendarManager.AM_PM:
							tempHour = ((CalendarProperties) this[calendar]).dateTime.Hour;
							return tempHour > 12 ? CalendarManager.PM : CalendarManager.AM;

						default:
							return 0;
					}
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					this.Add(calendar, tempProps);
					return this.Get(calendar, field);
				}
			}

			/// <summary>
			/// Sets the time in the specified calendar with the long value.
			/// </summary>
			/// <param name="calendar">The calendar to set its date and time.</param>
			/// <param name="milliseconds">A long value that indicates the milliseconds to be set to 
			/// the hour for the calendar.</param>
			public void SetTimeInMilliseconds(System.Globalization.Calendar calendar, long milliseconds)
			{
				if (this[calendar] != null)
				{
					((CalendarProperties) this[calendar]).dateTime = new System.DateTime(milliseconds);
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = new System.DateTime(System.TimeSpan.TicksPerMillisecond * milliseconds);
					this.Add(calendar, tempProps);
				}
			}
				
			/// <summary>
			/// Gets what the first day of the week is; e.g., Sunday in US, Monday in France.
			/// </summary>
			/// <param name="calendar">The calendar to get its first day of the week.</param>
			/// <returns>A System.DayOfWeek value indicating the first day of the week.</returns>
			public System.DayOfWeek GetFirstDayOfWeek(System.Globalization.Calendar calendar)
			{
				if (this[calendar] != null)
				{
					if (((CalendarProperties)this[calendar]).dateTimeFormat == null)
					{
						((CalendarProperties)this[calendar]).dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
						((CalendarProperties)this[calendar]).dateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Sunday;
					}
					return ((CalendarProperties) this[calendar]).dateTimeFormat.FirstDayOfWeek;
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					tempProps.dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
					tempProps.dateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Sunday;
					this.Add(calendar, tempProps);
					return this.GetFirstDayOfWeek(calendar);
				}
			}

			/// <summary>
			/// Sets what the first day of the week is; e.g., Sunday in US, Monday in France.
			/// </summary>
			/// <param name="calendar">The calendar to set its first day of the week.</param>
			/// <param name="firstDayOfWeek">A System.DayOfWeek value indicating the first day of the week
			/// to be set.</param>
			public void SetFirstDayOfWeek(System.Globalization.Calendar calendar, System.DayOfWeek  firstDayOfWeek)
			{
				if (this[calendar] != null)
				{
					if (((CalendarProperties)this[calendar]).dateTimeFormat == null)
						((CalendarProperties)this[calendar]).dateTimeFormat = new System.Globalization.DateTimeFormatInfo();

					((CalendarProperties) this[calendar]).dateTimeFormat.FirstDayOfWeek = firstDayOfWeek;
				}
				else
				{
					CalendarProperties tempProps = new CalendarProperties();
					tempProps.dateTime = System.DateTime.Now;
					tempProps.dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
					this.Add(calendar, tempProps);
					this.SetFirstDayOfWeek(calendar, firstDayOfWeek);
				}
			}

			/// <summary>
			/// Removes the specified calendar from the hash table.
			/// </summary>
			/// <param name="calendar">The calendar to be removed.</param>
			public void Clear(System.Globalization.Calendar calendar)
			{
				if (this[calendar] != null)
					this.Remove(calendar);
			}

			/// <summary>
			/// Removes the specified field from the calendar given.
			/// If the field does not exists in the calendar, the calendar is removed from the table.
			/// </summary>
			/// <param name="calendar">The calendar to remove the value from.</param>
			/// <param name="field">The field to be removed from the calendar.</param>
			public void Clear(System.Globalization.Calendar calendar, int field)
			{
				if (this[calendar] != null)
					this.Set(calendar, field, 0);
			}

			/// <summary>
			/// Internal class that represents the properties of a calendar instance.
			/// </summary>
			class CalendarProperties
			{
				/// <summary>
				/// The date and time of a calendar.
				/// </summary>
				public System.DateTime dateTime;
				
				/// <summary>
				/// The format for the date and time in a calendar.
				/// </summary>
				public System.Globalization.DateTimeFormatInfo dateTimeFormat;
			}
		}
	}
	/*******************************/
	/// <summary>
	/// Provides support functions to create read-write random acces files and write functions
	/// </summary>
	public class RandomAccessFileSupport
	{
		/// <summary>
		/// Creates a new random acces stream with read-write or read rights
		/// </summary>
		/// <param name="fileName">A relative or absolute path for the file to open</param>
		/// <param name="mode">Mode to open the file in</param>
		/// <returns>The new System.IO.FileStream</returns>
        //public static System.IO.FileStream CreateRandomAccessFile(System.String fileName, System.String mode) 
        //{
        //    System.IO.FileStream newFile = null;

        //    if (mode.CompareTo("rw") == 0)
        //        newFile =  new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite); 
        //    else if (mode.CompareTo("r") == 0 )
        //        newFile =  new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read); 
        //    else
        //        throw new System.ArgumentException();

        //    return newFile;
        //}

		/// <summary>
		/// Creates a new random acces stream with read-write or read rights
		/// </summary>
		/// <param name="fileName">File infomation for the file to open</param>
		/// <param name="mode">Mode to open the file in</param>
		/// <returns>The new System.IO.FileStream</returns>
        //public static System.IO.FileStream CreateRandomAccessFile(System.IO.FileInfo fileName, System.String mode)
        //{
        //    return CreateRandomAccessFile(fileName.FullName, mode);
        //}

		/// <summary>
		/// Writes the data to the specified file stream
		/// </summary>
		/// <param name="data">Data to write</param>
		/// <param name="fileStream">File to write to</param>
        //public static void WriteBytes(System.String data,System.IO.FileStream fileStream)
        //{
        //    int index = 0;
        //    int length = data.Length;

        //    while(index < length)
        //        fileStream.WriteByte((byte)data[index++]);	
        //}

		/// <summary>
		/// Writes the received string to the file stream
		/// </summary>
		/// <param name="data">String of information to write</param>
		/// <param name="fileStream">File to write to</param>
        //public static void WriteChars(System.String data,System.IO.FileStream fileStream)
        //{
        //    WriteBytes(data, fileStream);	
        //}

		/// <summary>
		/// Writes the received data to the file stream
		/// </summary>
		/// <param name="sByteArray">Data to write</param>
		/// <param name="fileStream">File to write to</param>
        //public static void WriteRandomFile(sbyte[] sByteArray,System.IO.FileStream fileStream)
        //{
        //    byte[] byteArray = ToByteArray(sByteArray);
        //    fileStream.Write(byteArray, 0, byteArray.Length);
        //}
	}

	/*******************************/
	/// <summary>
	/// Checks if the giving File instance is a directory or file, and returns his Length
	/// </summary>
	/// <param name="file">The File instance to check</param>
	/// <returns>The length of the file</returns>
    //public static long FileLength(System.IO.FileInfo file)
    //{
    //    if (file.Exists)
    //        return file.Length;
    //    else 
    //        return 0;
    //}

	/*******************************/
	/// <summary>
	/// Provides functionality for classes that implements the IList interface.
	/// </summary>
	public class IListSupport
	{
		/// <summary>
		/// Ensures the capacity of the list to be greater or equal than the specified.
		/// </summary>
		/// <param name="list">The list to be checked.</param>
		/// <param name="capacity">The expected capacity.</param>
        //public static void EnsureCapacity(System.Collections.ArrayList list, int capacity)
        //{
        //    if (list.Capacity < capacity) list.Capacity = 2 * list.Capacity;
        //    if (list.Capacity < capacity) list.Capacity = capacity;
        //}

		/// <summary>
		/// Adds all the elements contained into the specified collection, starting at the specified position.
		/// </summary>
		/// <param name="index">Position at which to add the first element from the specified collection.</param>
		/// <param name="list">The list used to extract the elements that will be added.</param>
		/// <returns>Returns true if all the elements were successfuly added. Otherwise returns false.</returns>
        //public static bool AddAll(System.Collections.IList list, int index, System.Collections.ICollection c)
        //{
        //    bool result = false;
        //    if (c != null)
        //    {
        //        System.Collections.IEnumerator tempEnumerator = new System.Collections.ArrayList(c).GetEnumerator();
        //        int tempIndex = index;

        //        while (tempEnumerator.MoveNext())
        //        {
        //            list.Insert(tempIndex++, tempEnumerator.Current);
        //            result = true;
        //        }
        //    }

        //    return result;
        //}

		/// <summary>
		/// Returns an enumerator of the collection starting at the specified position.
		/// </summary>
		/// <param name="index">The position to set the iterator.</param>
		/// <returns>An IEnumerator at the specified position.</returns>
		public static System.Collections.IEnumerator GetEnumerator(System.Collections.IList list, int index)
		{
			if ((index < 0) || (index > list.Count)) 
				throw new System.IndexOutOfRangeException();			

			System.Collections.IEnumerator tempEnumerator = list.GetEnumerator();
			if (index > 0)
			{
				int i=0;
				while ((tempEnumerator.MoveNext()) && (i < index - 1))
					i++;
			}
			return tempEnumerator;
		}
	}


	/*******************************/
	/// <summary>
	/// Converts the specified collection to its string representation.
	/// </summary>
	/// <param name="c">The collection to convert to string.</param>
	/// <returns>A string representation of the specified collection.</returns>
	public static System.String CollectionToString(System.Collections.ICollection c)
	{
		System.Text.StringBuilder s = new System.Text.StringBuilder();
		
		if (c != null)
		{
		
			System.Collections.ArrayList l = new System.Collections.ArrayList(c);

			bool isDictionary = (c is System.Collections.BitArray || c is System.Collections.Hashtable || c is System.Collections.IDictionary || c is System.Collections.Specialized.NameValueCollection || (l.Count > 0 && l[0] is System.Collections.DictionaryEntry));
			for (int index = 0; index < l.Count; index++) 
			{
				if (l[index] == null)
					s.Append("null");
				else if (!isDictionary)
					s.Append(l[index]);
				else
				{
					isDictionary = true;
					if (c is System.Collections.Specialized.NameValueCollection)
						s.Append(((System.Collections.Specialized.NameValueCollection)c).GetKey (index));
					else
						s.Append(((System.Collections.DictionaryEntry) l[index]).Key);
					s.Append("=");
					if (c is System.Collections.Specialized.NameValueCollection)
						s.Append(((System.Collections.Specialized.NameValueCollection)c).GetValues(index)[0]);
					else
						s.Append(((System.Collections.DictionaryEntry) l[index]).Value);

				}
				if (index < l.Count - 1)
					s.Append(", ");
			}
			
			if(isDictionary)
			{
				if(c is System.Collections.ArrayList)
					isDictionary = false;
			}
			if (isDictionary)
			{
				s.Insert(0, "{");
				s.Append("}");
			}
			else 
			{
				s.Insert(0, "[");
				s.Append("]");
			}
		}
		else
			s.Insert(0, "null");
		return s.ToString();
	}

	/// <summary>
	/// Tests if the specified object is a collection and converts it to its string representation.
	/// </summary>
	/// <param name="obj">The object to convert to string</param>
	/// <returns>A string representation of the specified object.</returns>
    //public static System.String CollectionToString(System.Object obj)
    //{
    //    System.String result = "";

    //    if (obj != null)
    //    {
    //        if (obj is System.Collections.ICollection)
    //            result = CollectionToString((System.Collections.ICollection)obj);
    //        else
    //            result = obj.ToString();
    //    }
    //    else
    //        result = "null";

    //    return result;
    //}
}
