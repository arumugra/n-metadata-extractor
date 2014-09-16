using System;
using System.Collections;
using System.Text;
using System.IO;
using com.drew.metadata;
using com.drew.lang;

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
namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for GPS
	/// </summary>
	public class GpsDescriptor : TagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="directory">a directory</param>
		public GpsDescriptor(Directory directory) : base(directory)
		{
		}

		/// <summary>
		/// Returns a descriptive value of the the specified tag for this image. 
		/// Where possible, known values will be substituted here in place of the raw tokens actually 
		/// kept in the Exif segment. 
		/// If no substitution is available, the value provided by GetString(int) will be returned.
		/// This and GetString(int) are the only 'get' methods that won't throw an exception.
		/// </summary>
		/// <param name="tagType">the tag to find a description for</param>
		/// <returns>a description of the image's value for the specified tag, or null if the tag hasn't been defined.</returns>
		public override string GetDescription(int tagType)  
		{
			switch(tagType) 
			{
				case GpsDirectory.TAG_GPS_ALTITUDE :
					return GetGpsAltitudeDescription();
				case GpsDirectory.TAG_GPS_ALTITUDE_REF :
					return GetGpsAltitudeRefDescription();
				case GpsDirectory.TAG_GPS_STATUS :
					return GetGpsStatusDescription();
				case GpsDirectory.TAG_GPS_MEASURE_MODE :
					return GetGpsMeasureModeDescription();
				case GpsDirectory.TAG_GPS_SPEED_REF :
					return GetGpsSpeedRefDescription();
				case GpsDirectory.TAG_GPS_TRACK_REF :
				case GpsDirectory.TAG_GPS_IMG_DIRECTION_REF :					
				case GpsDirectory.TAG_GPS_DEST_BEARING_REF :
					return GetGpsDirectionReferenceDescription(tagType);
				case GpsDirectory.TAG_GPS_TRACK :
				case GpsDirectory.TAG_GPS_IMG_DIRECTION :
				case GpsDirectory.TAG_GPS_DEST_BEARING :
					return GetGpsDirectionDescription(tagType);
				case GpsDirectory.TAG_GPS_DEST_DISTANCE_REF :
					return GetGpsDestinationReferenceDescription();
				case GpsDirectory.TAG_GPS_TIME_STAMP :
					return GetGpsTimeStampDescription();
					// three rational numbers -- displayed in HH"MM"SS.ss
				case GpsDirectory.TAG_GPS_LONGITUDE :
					return GetGpsLongitudeDescription();
				case GpsDirectory.TAG_GPS_LATITUDE :
					return GetGpsLatitudeDescription();
				default :
					return _directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Returns the Gps Latitude Description. 
		/// </summary>
		/// <returns>the Gps Latitude Description.</returns>
		private string GetGpsLatitudeDescription()  
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_LATITUDE))
				return null;
			return GetHoursMinutesSecondsDescription(GpsDirectory.TAG_GPS_LATITUDE);
		}

		/// <summary>
		/// Returns the Gps Longitude Description. 
		/// </summary>
		/// <returns>the Gps Longitude Description.</returns>
		private string GetGpsLongitudeDescription()  
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_LONGITUDE))
				return null;
			return GetHoursMinutesSecondsDescription(
				GpsDirectory.TAG_GPS_LONGITUDE);
		}

		/// <summary>
		/// Returns the Hours Minutes Seconds Description. 
		/// </summary>
		/// <returns>the Hours Minutes Seconds Description.</returns>
		private string GetHoursMinutesSecondsDescription(int tagType)
		{
			Rational[] components = _directory.GetRationalArray(tagType);
			// TODO create an HoursMinutesSecods class ??
			int deg = components[0].IntValue();
			float min = components[1].FloatValue();
			float sec = components[2].FloatValue();
			// carry fractions of minutes into seconds -- thanks Colin Briton
			sec += (min % 1) * 60;
			string[] tab = new string[] {deg.ToString(), ((int) min).ToString(), sec.ToString()};
			return BUNDLE["HOURS_MINUTES_SECONDS", tab];
		}

		/// <summary>
		/// Returns the Gps Time Stamp Description. 
		/// </summary>
		/// <returns>the Gps Time Stamp Description.</returns>
		private string GetGpsTimeStampDescription()  
		{
			// time in hour, min, sec
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_TIME_STAMP))
				return null;
			int[] timeComponents =
				_directory.GetIntArray(GpsDirectory.TAG_GPS_TIME_STAMP);
			string[] tab = new string[] {timeComponents[0].ToString(), timeComponents[1].ToString(), timeComponents[2].ToString()};
			return BUNDLE["GPS_TIME_STAMP", tab];
		}

		/// <summary>
		/// Returns the Gps Destination Reference Description. 
		/// </summary>
		/// <returns>the Gps Destination Reference Description.</returns>
		private string GetGpsDestinationReferenceDescription() 
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_DEST_DISTANCE_REF))
				return null;
			string destRef =
				_directory.GetString(GpsDirectory.TAG_GPS_DEST_DISTANCE_REF).Trim().ToUpper();
			if ("K".Equals(destRef)) 
			{
				return BUNDLE["KILOMETERS"];
			} 
			else if ("M".Equals(destRef)) 
			{
				return BUNDLE["MILES"];
			} 
			else if ("N".Equals(destRef)) 
			{
				return BUNDLE["KNOTS"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", destRef];
			}
		}

		/// <summary>
		/// Returns the Gps Direction Description. 
		/// </summary>
		/// <returns>the Gps Direction Description.</returns>
		private string GetGpsDirectionDescription(int tagType) 
		{
			if (!_directory.ContainsTag(tagType))
				return null;
			string gpsDirection = _directory.GetString(tagType).Trim();
			return BUNDLE["DEGREES", gpsDirection];
		}

		/// <summary>
		/// Returns the Gps Direction Reference Description. 
		/// </summary>
		/// <returns>the Gps Direction Reference Description.</returns>
		private string GetGpsDirectionReferenceDescription(int tagType) 
		{
			if (!_directory.ContainsTag(tagType))
				return null;
			string gpsDistRef = _directory.GetString(tagType).Trim().ToUpper();
			if ("T".Equals(gpsDistRef)) 
			{
				return BUNDLE["TRUE_DIRECTION"];
			} 
			else if ("M".Equals(gpsDistRef)) 
			{
				return BUNDLE["MAGNETIC_DIRECTION"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", gpsDistRef];
			}
		}

		/// <summary>
		/// Returns the Gps Speed Ref Description. 
		/// </summary>
		/// <returns>the Gps Speed Ref Description.</returns>
		private string GetGpsSpeedRefDescription() 
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_SPEED_REF))
				return null;
			string gpsSpeedRef =
				_directory.GetString(GpsDirectory.TAG_GPS_SPEED_REF).Trim().ToUpper();
			if ("K".Equals(gpsSpeedRef)) 
			{
				return BUNDLE["KPH"];
			} 
			else if ("M".Equals(gpsSpeedRef)) 
			{
				return BUNDLE["MPH"];
			} 
			else if ("N".Equals(gpsSpeedRef)) 
			{
				return BUNDLE["KNOTS"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", gpsSpeedRef];
			}
		}

		/// <summary>
		/// Returns the Gps Measure Mode Description. 
		/// </summary>
		/// <returns>the Gps Measure Mode Description.</returns>
		private string GetGpsMeasureModeDescription() 
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_MEASURE_MODE))
				return null;
			string gpsSpeedMeasureMode =
				_directory.GetString(GpsDirectory.TAG_GPS_MEASURE_MODE).Trim().ToUpper();
			if ("2".Equals(gpsSpeedMeasureMode) || "3".Equals(gpsSpeedMeasureMode)) 
			{
				return BUNDLE["DIMENSIONAL_MEASUREMENT", gpsSpeedMeasureMode];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", gpsSpeedMeasureMode];
			}
		}

		/// <summary>
		/// Returns the Gps Status Description. 
		/// </summary>
		/// <returns>the Gps Status Description.</returns>
		private string GetGpsStatusDescription() 
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_STATUS))
				return null;
			string gpsStatus =
				_directory.GetString(GpsDirectory.TAG_GPS_STATUS).Trim().ToUpper();
			if ("A".Equals(gpsStatus)) 
			{
				return BUNDLE["MEASUREMENT_IN_PROGESS"];
			} 
			else if ("V".Equals(gpsStatus)) 
			{
				return BUNDLE["MEASUREMENT_INTEROPERABILITY"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", gpsStatus];
			}
		}

		/// <summary>
		/// Returns the Gps Altitude Ref Description. 
		/// </summary>
		/// <returns>the Gps Altitude Ref Description.</returns>
		private string GetGpsAltitudeRefDescription()  
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_ALTITUDE_REF))
				return null;
			int alititudeRef = _directory.GetInt(GpsDirectory.TAG_GPS_ALTITUDE_REF);
			if (alititudeRef == 0) 
			{
				return BUNDLE["SEA_LEVEL"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", alititudeRef.ToString()];
			}
		}

		/// <summary>
		/// Returns the Gps Altitude Description. 
		/// </summary>
		/// <returns>the Gps Altitude Description.</returns>
		private string GetGpsAltitudeDescription()  
		{
			if (!_directory.ContainsTag(GpsDirectory.TAG_GPS_ALTITUDE))
				return null;
			string alititude =
				_directory.GetRational(
				GpsDirectory.TAG_GPS_ALTITUDE).ToSimpleString(
				true);
			return BUNDLE["METRES", alititude];
		}
	}
}