using UnityEngine;
using System.Collections;
using DISUnity.Utils;

namespace DISUnity.Simulation
{
	public abstract class GeodeticCoordinateConverter : WorldCoordinateConverter 
	{
		/// <summary>
		/// Converts DIS geocentric coordinate to Geodetic coordinate using WGS84 reference ellipsoid.
		/// </summary>
		/// <param name="x">The geocentric x coordinate.</param>
		/// <param name="y">The geocentric y coordinate.</param>
		/// <param name="z">The geocentric z coordinate.</param>
		/// <param name="lat">Latitude.</param>
		/// <param name="lon">Lonngitude.</param>
		/// <param name="alt">Altitude.</param>
		public virtual void RemoteToGeodetic( double x, double y, double z, out double lat, out double lon, out double alt )
		{
			// We use WGS84 as default, users should override if needed
			Conversions.GeocentricGeodetic.GeocentricToGeodetic( x, y, z, out lat, out lon, out alt, 
			                                                     Conversions.GeocentricGeodetic.RefEllipsoid.WGS_1984 );
		}

		/// <summary>
		/// Converts Geodetic to DIS Geocentric coordiantes using WGS84 reference ellipsoid.
		/// </summary>
		/// <param name="lat">Latitude.</param>
		/// <param name="lon">Lonngitude.</param>
		/// <param name="alt">Altitude.</param>
		/// <param name="x">The geocentric x coordinate.</param>
		/// <param name="y">The geocentric y coordinate.</param>
		/// <param name="z">The geocentric z coordinate.</param>
		public virtual void GeodeticToRemote( double lat, double lon, double alt, out double x, out double y, out double z )
		{
			// We use WGS84 as default, users should override if needed
			Conversions.GeocentricGeodetic.GeodeticToGeocentric( lat, lon, alt, out x, out y, out z, 
			                                                     Conversions.GeocentricGeodetic.RefEllipsoid.WGS_1984 );
		}

		/// <summary>
		/// Converts Geodetic to local Unity coordinates.
		/// </summary>
		/// <param name="lat">Latitude.</param>
		/// <param name="lon">Lonngitude.</param>
		/// <param name="alt">Altitude.</param>
		/// <param name="local">Local Unity coordinates.</param>
		public abstract void GeodeticToLocal( double lat, double lon, double alt, out Vector3 local );

		/// <summary>
		/// Converts local Unity to Geodetic coordinates.
		/// </summary>
		/// <param name="local">Local Unity coordinates.</param>
		/// <param name="lat">Latitude.</param>
		/// <param name="lon">Lonngitude.</param>
		/// <param name="alt">Altitude.</param>
		public abstract void LocalToGeodetic( Vector3 local, out double lat, out double lon, out double alt );

		/// <summary>
		/// Converts DIS Geocentric to local Unity coordinates.
		/// </summary>
		/// <remarks>WorldScale is ignored.</remarks>
		/// <param name="x">The geocentric x coordinate.</param>
		/// <param name="y">The geocentric y coordinate.</param>
		/// <param name="z">The geocentric z coordinate.</param>
		/// <param name="local"></param>
		public override void RemoteToLocal( double x, double y, double z, out Vector3 local )
		{
			double lat, lon, alt;
			RemoteToGeodetic( x, y, z, out lat, out lon, out alt );
			GeodeticToLocal( lat, lon, alt, out local );
		}
		
		/// <summary>
		/// Converts local Unity to DIS Geocentric coordinates.
		/// </summary>
		/// <remarks>WorldScale is ignored.</remarks>
		/// <param name="local"></param>
		/// <param name="remote"></param>
		public override void LocalToRemote( Vector3 local, out double[] remote ) // TODO: this will actually create memory leak if used frequently
		{
			double lat, lon, alt;
			LocalToGeodetic( local, out lat, out lon, out alt );
			double x, y, z;
			GeodeticToRemote( lat, lon, alt, out x, out y, out z );
			remote = new double[] { x, y, z };
		}
	}
}