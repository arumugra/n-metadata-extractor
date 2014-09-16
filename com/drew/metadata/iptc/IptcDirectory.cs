using System;
using System.Collections;
using com.drew.lang;
using com.drew.metadata;
using com.utils;

/// <summary>
/// This class was first written by Drew Noakes in Java.
///
/// This is public domain software - that is, you can do whatever you want
/// with it, and include it software that is licensed under the GNU or the
/// BSD license, or whatever other licence you choose, including proprietary
/// closed source licenses.  I do ask that you leave this header in tact.
///
/// If you make modifications to this code that you think would benefit the
/// wider community, please send me a copy and I'll post it on my site.
///
/// If you make use of this code, Drew Noakes will appreciate hearing 
/// about it: <a href="mailto:drew@drewnoakes.com">drew@drewnoakes.com</a>
///
/// Latest Java version of this software kept at 
/// <a href="http://drewnoakes.com">http://drewnoakes.com/</a>
///
/// The C# class was made by Ferret Renaud: 
/// <a href="mailto:renaud91@free.fr">renaud91@free.fr</a>
/// If you find a bug in the C# code, feel free to mail me.
/// </summary>
namespace com.drew.metadata.iptc
{
	/// <summary>
	/// The Iptc Directory class
	/// </summary>
	public class IptcDirectory : Directory 
	{
		public const int TAG_RECORD_VERSION = 0x0200;
		public const int TAG_CAPTION = 0x0278;
		public const int TAG_WRITER = 0x027a;
		public const int TAG_HEADLINE = 0x0269;
		public const int TAG_SPECIAL_INSTRUCTIONS = 0x0228;
		public const int TAG_BY_LINE = 0x0250;
		public const int TAG_BY_LINE_TITLE = 0x0255;
		public const int TAG_CREDIT = 0x026e;
		public const int TAG_SOURCE = 0x0273;
		public const int TAG_OBJECT_NAME = 0x0205;
		public const int TAG_DATE_CREATED = 0x0237;
		public const int TAG_CITY = 0x025a;
		public const int TAG_PROVINCE_OR_STATE = 0x025f;
		public const int TAG_COUNTRY_OR_PRIMARY_LOCATION = 0x0265;
		public const int TAG_ORIGINAL_TRANSMISSION_REFERENCE = 0x0267;
		public const int TAG_CATEGORY = 0x020f;
		public const int TAG_SUPPLEMENTAL_CATEGORIES = 0x0214;
		public const int TAG_URGENCY = 0x0200 | 10;
		public const int TAG_KEYWORDS = 0x0200 | 25;
		public const int TAG_COPYRIGHT_NOTICE = 0x0274;
		public const int TAG_RELEASE_DATE = 0x0200 | 30;
		public const int TAG_RELEASE_TIME = 0x0200 | 35;
		public const int TAG_TIME_CREATED = 0x0200 | 60;
		public const int TAG_ORIGINATING_PROGRAM = 0x0200 | 65;

		protected static readonly ResourceBundle BUNDLE = new ResourceBundle("IptcMarkernote");
		protected static readonly IDictionary tagNameMap = IptcDirectory.InitTagMap();

		/// <summary>
		/// Initialize the tag map.
		/// </summary>
		/// <returns>the tag map</returns>
		private static IDictionary InitTagMap() 
		{
			IDictionary resu = new Hashtable();
			resu.Add(TAG_RECORD_VERSION, BUNDLE["TAG_RECORD_VERSION"]);
			resu.Add(TAG_CAPTION, BUNDLE["TAG_CAPTION"]);
			resu.Add(TAG_WRITER, BUNDLE["TAG_WRITER"]);
			resu.Add(TAG_HEADLINE, BUNDLE["TAG_HEADLINE"]);
			resu.Add(TAG_SPECIAL_INSTRUCTIONS, BUNDLE["TAG_SPECIAL_INSTRUCTIONS"]);
			resu.Add(TAG_BY_LINE, BUNDLE["TAG_BY_LINE"]);
			resu.Add(TAG_BY_LINE_TITLE, BUNDLE["TAG_BY_LINE_TITLE"]);
			resu.Add(TAG_CREDIT, BUNDLE["TAG_CREDIT"]);
			resu.Add(TAG_SOURCE, BUNDLE["TAG_SOURCE"]);
			resu.Add(TAG_OBJECT_NAME, BUNDLE["TAG_OBJECT_NAME"]);
			resu.Add(TAG_DATE_CREATED, BUNDLE["TAG_DATE_CREATED"]);
			resu.Add(TAG_CITY, BUNDLE["TAG_CITY"]);
			resu.Add(TAG_PROVINCE_OR_STATE, BUNDLE["TAG_PROVINCE_OR_STATE"]);
			resu.Add(TAG_COUNTRY_OR_PRIMARY_LOCATION, BUNDLE["TAG_COUNTRY_OR_PRIMARY_LOCATION"]);
			resu.Add(TAG_ORIGINAL_TRANSMISSION_REFERENCE, BUNDLE["TAG_ORIGINAL_TRANSMISSION_REFERENCE"]);
			resu.Add(TAG_CATEGORY, BUNDLE["TAG_CATEGORY"]);
			resu.Add(TAG_SUPPLEMENTAL_CATEGORIES, BUNDLE["TAG_SUPPLEMENTAL_CATEGORIES"]);
			resu.Add(TAG_URGENCY, BUNDLE["TAG_URGENCY"]);
			resu.Add(TAG_KEYWORDS, BUNDLE["TAG_KEYWORDS"]);
			resu.Add(TAG_COPYRIGHT_NOTICE, BUNDLE["TAG_COPYRIGHT_NOTICE"]);
			resu.Add(TAG_RELEASE_DATE, BUNDLE["TAG_RELEASE_DATE"]);
			resu.Add(TAG_RELEASE_TIME, BUNDLE["TAG_RELEASE_TIME"]);
			resu.Add(TAG_TIME_CREATED, BUNDLE["TAG_TIME_CREATED"]);
			resu.Add(TAG_ORIGINATING_PROGRAM, BUNDLE["TAG_ORIGINATING_PROGRAM"]);
			return resu;
		}

		/// <summary>
		/// Constructor of the object.
		/// </summary>
		public IptcDirectory() : base()
		{
			this.SetDescriptor(new IptcDescriptor(this));
		}

		/// <summary>
		/// Provides the name of the directory, for display purposes.  E.g. Exif 
		/// </summary>
		/// <returns>the name of the directory</returns>
		public override string GetName() 
		{
			return BUNDLE["MARKER_NOTE_NAME"];
		}

		/// <summary>
		/// Provides the map of tag names, hashed by tag type identifier. 
		/// </summary>
		/// <returns>the map of tag names</returns>
		protected override IDictionary GetTagNameMap() 
		{
			return tagNameMap;
		}
	}
}