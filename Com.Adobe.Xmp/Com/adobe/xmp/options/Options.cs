// =================================================================================================
// ADOBE SYSTEMS INCORPORATED
// Copyright 2006 Adobe Systems Incorporated
// All Rights Reserved
//
// NOTICE:  Adobe permits you to use, modify, and distribute this file in accordance with the terms
// of the Adobe license agreement accompanying it.
// =================================================================================================
using System.Collections;
using System.Text;
using Com.Adobe.Xmp;
using Sharpen;

namespace Com.Adobe.Xmp.Options
{
	/// <summary>The base class for a collection of 32 flag bits.</summary>
	/// <remarks>
	/// The base class for a collection of 32 flag bits. Individual flags are defined as enum value bit
	/// masks. Inheriting classes add convenience accessor methods.
	/// </remarks>
	/// <since>24.01.2006</since>
	public abstract class Options
	{
		/// <summary>the internal int containing all options</summary>
		private int options = 0;

		/// <summary>a map containing the bit names</summary>
		private IDictionary optionNames = null;

		/// <summary>The default constructor.</summary>
		public Options()
		{
		}

		/// <summary>Constructor with the options bit mask.</summary>
		/// <param name="options">the options bit mask</param>
		/// <exception cref="Com.Adobe.Xmp.XMPException">If the options are not correct</exception>
		public Options(int options)
		{
			// EMTPY
			AssertOptionsValid(options);
			SetOptions(options);
		}

		/// <summary>Resets the options.</summary>
		public virtual void Clear()
		{
			options = 0;
		}

		/// <param name="optionBits">an option bitmask</param>
		/// <returns>Returns true, if this object is equal to the given options.</returns>
		public virtual bool IsExactly(int optionBits)
		{
			return GetOptions() == optionBits;
		}

		/// <param name="optionBits">an option bitmask</param>
		/// <returns>Returns true, if this object contains all given options.</returns>
		public virtual bool ContainsAllOptions(int optionBits)
		{
			return (GetOptions() & optionBits) == optionBits;
		}

		/// <param name="optionBits">an option bitmask</param>
		/// <returns>Returns true, if this object contain at least one of the given options.</returns>
		public virtual bool ContainsOneOf(int optionBits)
		{
			return ((GetOptions()) & optionBits) != 0;
		}

		/// <param name="optionBit">the binary bit or bits that are requested</param>
		/// <returns>Returns if <emp>all</emp> of the requested bits are set or not.</returns>
		protected internal virtual bool GetOption(int optionBit)
		{
			return (options & optionBit) != 0;
		}

		/// <param name="optionBits">the binary bit or bits that shall be set to the given value</param>
		/// <param name="value">the boolean value to set</param>
		public virtual void SetOption(int optionBits, bool value)
		{
			options = value ? options | optionBits : options & ~optionBits;
		}

		/// <summary>Is friendly to access it during the tests.</summary>
		/// <returns>Returns the options.</returns>
		public virtual int GetOptions()
		{
			return options;
		}

		/// <param name="options">The options to set.</param>
		/// <exception cref="Com.Adobe.Xmp.XMPException"></exception>
		public virtual void SetOptions(int options)
		{
			AssertOptionsValid(options);
			this.options = options;
		}

		/// <seealso cref="object.Equals(object)"/>
		public override bool Equals(object obj)
		{
			return GetOptions() == ((Com.Adobe.Xmp.Options.Options)obj).GetOptions();
		}

		/// <seealso cref="object.GetHashCode()"/>
		public override int GetHashCode()
		{
			return GetOptions();
		}

		/// <summary>Creates a human readable string from the set options.</summary>
		/// <remarks>
		/// Creates a human readable string from the set options. <em>Note:</em> This method is quite
		/// expensive and should only be used within tests or as
		/// </remarks>
		/// <returns>
		/// Returns a String listing all options that are set to <code>true</code> by their name,
		/// like &quot;option1 | option4&quot;.
		/// </returns>
		public virtual string GetOptionsString()
		{
			if (options != 0)
			{
				StringBuilder sb = new StringBuilder();
				int theBits = options;
				while (theBits != 0)
				{
					int oneLessBit = theBits & (theBits - 1);
					// clear rightmost one bit
					int singleBit = theBits ^ oneLessBit;
					string bitName = GetOptionName(singleBit);
					sb.Append(bitName);
					if (oneLessBit != 0)
					{
						sb.Append(" | ");
					}
					theBits = oneLessBit;
				}
				return sb.ToString();
			}
			else
			{
				return "<none>";
			}
		}

		/// <returns>Returns the options as hex bitmask.</returns>
		public override string ToString()
		{
			return "0x" + Sharpen.Extensions.ToHexString(options);
		}

		/// <summary>To be implemeted by inheritants.</summary>
		/// <returns>Returns a bit mask where all valid option bits are set.</returns>
		protected internal abstract int GetValidOptions();

		/// <summary>To be implemeted by inheritants.</summary>
		/// <param name="option">a single, valid option bit.</param>
		/// <returns>Returns a human readable name for an option bit.</returns>
		protected internal abstract string DefineOptionName(int option);

		/// <summary>The inheriting option class can do additional checks on the options.</summary>
		/// <remarks>
		/// The inheriting option class can do additional checks on the options.
		/// <em>Note:</em> For performance reasons this method is only called
		/// when setting bitmasks directly.
		/// When get- and set-methods are used, this method must be called manually,
		/// normally only when the Options-object has been created from a client
		/// (it has to be made public therefore).
		/// </remarks>
		/// <param name="options">the bitmask to check.</param>
		/// <exception cref="Com.Adobe.Xmp.XMPException">Thrown if the options are not consistent.</exception>
		protected internal virtual void AssertConsistency(int options)
		{
		}

		// empty, no checks
		/// <summary>Checks options before they are set.</summary>
		/// <remarks>
		/// Checks options before they are set.
		/// First it is checked if only defined options are used,
		/// second the additional
		/// <see cref="AssertConsistency(int)"/>
		/// -method is called.
		/// </remarks>
		/// <param name="options">the options to check</param>
		/// <exception cref="Com.Adobe.Xmp.XMPException">Thrown if the options are invalid.</exception>
		private void AssertOptionsValid(int options)
		{
			int invalidOptions = options & ~GetValidOptions();
			if (invalidOptions == 0)
			{
				AssertConsistency(options);
			}
			else
			{
				throw new XMPException("The option bit(s) 0x" + Sharpen.Extensions.ToHexString(invalidOptions) + " are invalid!", XMPErrorConstants.Badoptions);
			}
		}

		/// <summary>Looks up or asks the inherited class for the name of an option bit.</summary>
		/// <remarks>
		/// Looks up or asks the inherited class for the name of an option bit.
		/// Its save that there is only one valid option handed into the method.
		/// </remarks>
		/// <param name="option">a single option bit</param>
		/// <returns>Returns the option name or undefined.</returns>
		private string GetOptionName(int option)
		{
			IDictionary optionsNames = ProcureOptionNames();
			int key = option;
			string result = (string)optionsNames.Get(key);
			if (result == null)
			{
				result = DefineOptionName(option);
				if (result != null)
				{
					optionsNames.Put(key, result);
				}
				else
				{
					result = "<option name not defined>";
				}
			}
			return result;
		}

		/// <returns>Returns the optionNames map and creates it if required.</returns>
		private IDictionary ProcureOptionNames()
		{
			if (optionNames == null)
			{
				optionNames = new Hashtable();
			}
			return optionNames;
		}
	}
}
