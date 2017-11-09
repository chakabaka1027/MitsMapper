using UnityEngine;
using System;

namespace DISUnity.Utils
{
	public class Conversions
	{
		public static double RadToDeg( double Rad )
		{
			return Rad * ( 180.0 / System.Math.PI );
		}
		
		public static double DegToRad( double Deg )
		{
			return Deg * ( System.Math.PI  / 180.0 );
		}
		
		public static double FeetToMeters( double Feet )
		{
			return Feet / 3.2808;
		}

		
		public static double MetersToFeet( double Meters )
		{
			return Meters * 3.2808;
		}

		public static void RotateAboutAxis( double[] d , double[] s, double[] n, double  t )
		{
			double  st = Math.Sin( t );
			double  ct = Math.Cos( t );
			
			d[0] = (1.0-ct)*(n[0]*n[0]*s[0] + n[0]*n[1]*s[1] + n[0]*n[2]*s[2]) + ct*s[0] + st*(n[1]*s[2]-n[2]*s[1]);
			d[1] = (1.0-ct)*(n[0]*n[1]*s[0] + n[1]*n[1]*s[1] + n[1]*n[2]*s[2]) + ct*s[1] + st*(n[2]*s[0]-n[0]*s[2]);
			d[2] = (1.0-ct)*(n[0]*n[2]*s[0] + n[1]*n[2]*s[1] + n[2]*n[2]*s[2]) + ct*s[2] + st*(n[0]*s[1]-n[1]*s[0]);
		}
		
		public static void Cross( double[] d ,double[] a, double[] b )
		{
			d[0] = a[1] * b[2] - b[1] * a[2] ;
			d[1] = b[0] * a[2] - a[0] * b[2] ;
			d[2] = a[0] * b[1] - b[0] * a[1] ;
		}
		
		public static double Dot( double[] a, double[] b )
		{
			return  a[0]*b[0] + a[1]*b[1] + a[2]*b[2];
		}

		// variables below used by HeadingPitchRollToEuler & EulerToHeadingPitchRoll
		static readonly double[] E0 = { 0.0 , 1.0 , 0.0 };
		static readonly double[] N0 = { 0.0 , 0.0 , 1.0 };
		static readonly double[] x0 = { 1.0 , 0.0 , 0.0 };   // == D0
		static readonly double[] y0 = { 0.0 , 1.0 , 0.0 };   // == E0
		static readonly double[] z0 = { 0.0 , 0.0 , 1.0 };   // == Z0
		/// <summary>
		/// Converts Heading, Pitch and Roll to Euler for DIS.
		/// </summary>
		/// <param name="H">Heading in radians.</param>
		/// <param name="P">Pitch in radians.</param>
		/// <param name="R">Roll in radians.</param>
		/// <param name="Lat">Geodetic Latitude in radians.</param>
		/// <param name="Lon">Geodetic Longitude in radians.</param>
		/// <param name="Psi">Euler angle out.</param>
		/// <param name="Theta">Euler angle out.</param>
		/// <param name="Phi">Euler angle out.</param>
		public static void HeadingPitchRollToEuler( double H, double P, double R, double Lat, double Lon, 
		                                           out double Psi, out double Theta, out double Phi )
		{
			// local NED
			double[] me = new double[3];
			double[] N = new double[3];
			double[] E = new double[3];
			double[] D = new double[3];

			// 'E'
			RotateAboutAxis( E , E0 , N0 , Lon );
			me[0] = -E[0] ;
			me[1] = -E[1] ;
			me[2] = -E[2] ;
			// 'N'
			RotateAboutAxis( N , N0 , me , Lat );
			// 'D'
			Cross( D , N , E );
			/*
		    *  Orientation
		    */
			// rotate about D by heading
			double[] N1 = new double[3];
			double[] E1 = new double[3];
			double[] D1 = new double[3];
			RotateAboutAxis( N1 , N  , D , H );
			RotateAboutAxis( E1 , E  , D , H );
			Buffer.BlockCopy(D, 0, D1, 0, 3 * sizeof(Double)); 
			// rotate about E1 vector by pitch
			double[] N2 = new double[3];
			double[] E2 = new double[3];
			double[] D2 = new double[3];
			RotateAboutAxis( N2 , N1 , E1 , P );
			Buffer.BlockCopy(E1, 0, E2, 0, 3 * sizeof(Double)); 
			RotateAboutAxis( D2 , D1 , E1 , P );
			// rotate about N2 by roll
			double[] N3 = new double[3];
			double[] E3 = new double[3];
			double[] D3 = new double[3];
			Buffer.BlockCopy(N2, 0, N3, 0, 3 * sizeof(Double)); 
			RotateAboutAxis( E3 , E2 , N2 , R );
			RotateAboutAxis( D3 , D2 , N2 , R );
			
			// calculate angles from vectors
			double[] y2 = new double[3] ;
			double[] z2 = new double[3] ;
			Psi = Math.Atan2(  Dot( N3 , y0 ) , Dot( N3 , x0 ) );
			Theta = Math.Atan2( -Dot( N3 , z0 ) , Math.Sqrt( Math.Pow(Dot( N3 , x0 ), 2) + Math.Pow(Dot( N3 , y0 ), 2) ) );
			RotateAboutAxis( y2 , y0 , z0 , Psi );
			RotateAboutAxis( z2 , z0 , y2 , Theta );
			Phi = Math.Atan2(  Dot( E3 , z2 ) , Dot( E3 , y2 ) );
		}
		
		//////////////////////////////////////////////////////////////////////////

		static readonly double[] X = {1.0 ,0.0 ,0.0};
		static readonly double[] Y = {0.0 ,1.0 ,0.0};
		static readonly double[] Z = {0.0 ,0.0 ,1.0};
		/// <summary>
		/// Converts Euler to Heading, Pitch and Roll.
		/// </summary>
		/// <param name="Lat">Geodetic Latitude in radians.</param>
		/// <param name="Lon">Geodetic Longitude in radians.</param>
		/// <param name="Psi">Euler angle.</param>
		/// <param name="Theta">Euler angle.</param>
		/// <param name="Phi">Euler angle.</param>
		/// <param name="H">Heading in radians output.</param>
		/// <param name="P">Pitch in radians output.</param>
		/// <param name="R">Roll in radians output.</param>
		public static void EulerToHeadingPitchRoll( double Lat, double Lon, double Psi, double Theta, double Phi, 
													out double H, out double P, out double R )
		{
			// local NED vectors in ECEF coordinate frame
			double[] N = new double[3] ;
			double[] E = new double[3] ;
			double[] D = new double[3] ;
			
			// Calculate NED from lat and lon
			// local NED

			double[] me = new double[3] ;
			// 'E'
			RotateAboutAxis( E , E0 , N0 , Lon );
			me[0] = -E[0] ;
			me[1] = -E[1] ;
			me[2] = -E[2] ;
			// 'N'
			RotateAboutAxis( N , N0 , me , Lat );
			// 'D'
			Cross( D , N , E );
			/*
		     *  Orientation:
		     *    input : (x0,y0,z0)=(N,E,D) and (Psi,Theta,Phi Euler angles)
		     *    output: (x3,y3,z3)=body vectors in local frame
		     */
			// rotate about Z by Psi
			double[] X1 = new double[3];
			double[] Y1 = new double[3];
			double[] Z1 = new double[3];
			RotateAboutAxis( X1 , X  , Z , Psi );
			RotateAboutAxis( Y1 , Y  , Z , Psi );
			Buffer.BlockCopy(Z, 0, Z1, 0, 3 * sizeof(Double)); 
			// rotate about Y1 vector by Theta
			double[] X2 = new double[3];
			double[] Y2 = new double[3];
			double[] Z2 = new double[3];
			RotateAboutAxis( X2 , X1 , Y1 , Theta );
			Buffer.BlockCopy(Y1, 0, Y2, 0, 3 * sizeof(Double)); 
			RotateAboutAxis( Z2 , Z1 , Y1 , Theta );
			// rotate about X2 by Phi
			double[] X3 = new double[3];
			double[] Y3 = new double[3];
			double[] Z3 = new double[3];
			Buffer.BlockCopy(X2, 0, X3, 0, 3 * sizeof(Double)); 
			RotateAboutAxis( Y3 , Y2 , X2 , Phi );
			RotateAboutAxis( Z3 , Z2 , X2 , Phi );
			// calculate angles from vectors
			double[] x0 = new double[3];
			Buffer.BlockCopy(N, 0, x0, 0, 3 * sizeof(Double)); 
			double[] y0 = new double[3];
			Buffer.BlockCopy(E, 0, y0, 0, 3 * sizeof(Double)); 
			double[] z0 = new double[3];
			Buffer.BlockCopy(D, 0, z0, 0, 3 * sizeof(Double)); 
			double[] y2 = new double[3] ;
			double[] z2 = new double[3] ;
			H = Math.Atan2(  Dot( X3 , y0 ) , Dot( X3 , x0 ) );
			P = Math.Atan2( -Dot( X3 , z0 ) , Math.Sqrt( Math.Pow(Dot( X3 , x0 ), 2) + Math.Pow(Dot( X3 , y0 ), 2) ) );
			RotateAboutAxis( y2 , y0 , z0 , H );
			RotateAboutAxis( z2 , z0 , y2 , P );
			R = Math.Atan2(  Dot( Y3 , z2 ) , Dot( Y3 , y2 ) );
		}

		public class GeocentricGeodetic 
		{
			///  Refrence Ellipsoids, data taken from Wikipedia and DoD, WGS84, DMA TR 8350.2-B,1 Sept. 1991
			public enum RefEllipsoid
			{
				Airy,
				Airy_Modified,
				Australian_National,
				Bessel_1841,
				Bessel_1841_Namibia,
				Clarke_1866,
				Clarke_1880,
				Everest_Sabah_Sarawak,
				Everest_1830,
				Everest_1948,
				Everest_1956,
				Everest_1969,
				Fischer_1960,
				Fischer_1960_Modified,
				Fischer_1968,
				GRS_1980,
				Helmert_1906,
				Hough,
				International_1924,
				Karsovsky_1940,
				SGS_1985,
				South_American_1969,
				Sphere_6371km,
				WGS_1960,
				WGS_1966,
				WGS_1972,
				WGS_1984
			}

			public static void GetEllipsoidAxis( RefEllipsoid R, out double MajorAxis, out double MinorAxis )
			{
				switch( R )
				{
				case RefEllipsoid. Airy:
					MajorAxis = 6377563.396;
					MinorAxis = 6356256.909;
					// 1/F 299.324965
					break;
					
				case RefEllipsoid. Airy_Modified:
					MajorAxis = 6377340.189;
					MinorAxis = 6356034.448;
					// 1/F 299.324965
					break;
					
				case RefEllipsoid. Australian_National:
					MajorAxis = 6378160.000;
					MinorAxis = 6356774.719;
					// 1/F 298.250000
					break;
					
				case RefEllipsoid. Bessel_1841:
					MajorAxis = 6377397.155;
					MinorAxis = 6356078.963;
					// 1/F 299.152813
					break;
					
				case RefEllipsoid. Bessel_1841_Namibia:
					MajorAxis = 6377483.865;
					MinorAxis = 6356078.963;
					// 1/F 299.152813
					break;
					
				case RefEllipsoid. Clarke_1866:
					MajorAxis = 6378206.400;
					MinorAxis = 6356583.800;
					// 1/F 294.978698
					break;
					
				case RefEllipsoid. Clarke_1880:
					MajorAxis = 6378249.145;
					MinorAxis = 6356514.870;
					// 1/F 293.465000
					break;
					
				case RefEllipsoid. Everest_Sabah_Sarawak:
					MajorAxis = 6377298.556;
					MinorAxis = 6356097.550;
					// 1/F 300.801700
					break;
					
				case RefEllipsoid. Everest_1830:
					MajorAxis = 6377276.345;
					MinorAxis = 6356075.413;
					// 1/F 300.801700
					break;
					
				case RefEllipsoid. Everest_1948:
					MajorAxis = 6377304.063;
					MinorAxis = 6356103.039;
					// 1/F 300.801700
					break;
					
				case RefEllipsoid. Everest_1956:
					MajorAxis = 6377301.243;
					MinorAxis = 6356100.228;
					// 1/F 300.801700
					break;
					
				case RefEllipsoid. Everest_1969:
					MajorAxis = 6377295.664;
					MinorAxis = 6356094.668;
					// 1/F 300.801700
					break;
					
				case RefEllipsoid. Fischer_1960:
					MajorAxis = 6378166.000;
					MinorAxis = 6356784.284;
					// 1/F 298.300000
					break;
					
				case RefEllipsoid. Fischer_1960_Modified:
					MajorAxis = 6378155.000;
					MinorAxis = 6356773.320;
					// 1/F 298.300000
					break;
					
				case RefEllipsoid. Fischer_1968:
					MajorAxis = 6378150.000;
					MinorAxis = 6356768.337;
					// 1/F 298.300000
					break;
					
				case RefEllipsoid. GRS_1980:
					MajorAxis = 6378137.000;
					MinorAxis = 6356752.314;
					// 1/F 298.257222
					break;
					
				case RefEllipsoid. Helmert_1906:
					MajorAxis = 6378200.000;
					MinorAxis = 6356818.170;
					// 1/F 298.300000
					break;
					
				case RefEllipsoid. Hough:
					MajorAxis = 6378270.000;
					MinorAxis = 6356794.343;
					// 1/F 297.000000
					break;
					
				case RefEllipsoid. International_1924:
					MajorAxis = 6378388.000;
					MinorAxis = 6356911.946;
					// 1/F 297.000000
					break;
					
				case RefEllipsoid. Karsovsky_1940:
					MajorAxis = 6378245.000;
					MinorAxis = 6356863.019;
					// 1/F 298.300000
					break;
					
				case RefEllipsoid. SGS_1985:
					MajorAxis = 6378136.000;
					MinorAxis = 6356751.302;
					// 1/F 298.257000
					break;
					
				case RefEllipsoid. South_American_1969:
					MajorAxis = 6378160.000;
					MinorAxis = 6356774.719;
					// 1/F 298.250000
					break;
					
				case RefEllipsoid. Sphere_6371km:
					MajorAxis = 6371000;
					MinorAxis = 6371000;
					break;
					
				case RefEllipsoid. WGS_1960:
					MajorAxis = 6378165.000;
					MinorAxis = 6356783.287;
					// 1/F 298.300000
					break;
					
				case RefEllipsoid. WGS_1966:
					MajorAxis = 6378145.000;
					MinorAxis = 6356759.769;
					// 1/F 298.250000
					break;
					
				case RefEllipsoid. WGS_1972:
					MajorAxis = 6378135.000;
					MinorAxis = 6356750.520;
					// 1/F 298.260000
					break;
					
				case RefEllipsoid. WGS_1984:
					MajorAxis = 6378137.000;
					MinorAxis = 6356752.314245;
					// 1/F 298.257224
					break;

				default:
					// TODO: evaluate, what default value should be assigned?
					MajorAxis = 0;
					MinorAxis = 0;
					break;
				}
			}

			/// <summary>
			/// Converts Geodetic coords to Geocentric cartesian.
			/// </summary>
			/// <param name="GeodeticLat">Geodetic lat in degrees.</param>
			/// <param name="GeodeticLon">Geodetic lon in degrees.</param>
			/// <param name="GeodeticHeight">Geodetic height in meters.</param>
			/// <param name="GeocentricX">Geocentric x.</param>
			/// <param name="GeocentricY">Geocentric y.</param>
			/// <param name="GeocentricZ">Geocentric z.</param>
			/// <param name="R">R.</param>
			public static void GeodeticToGeocentric( double GeodeticLat, double GeodeticLon, double GeodeticHeight,
			                                         out double GeocentricX, out double GeocentricY, out double GeocentricZ,
			                                         RefEllipsoid R )
			{
				GeodeticLat = DegToRad( GeodeticLat );
				GeodeticLon = DegToRad( GeodeticLon );
				
				double MajorAxis, MinorAxis;
				GetEllipsoidAxis( R, out MajorAxis, out MinorAxis );
				
				double Esq = ( Math.Pow( MajorAxis, 2 ) - Math.Pow( MinorAxis, 2 ) ) / Math.Pow( MajorAxis, 2 );
				double V = MajorAxis / Math.Sqrt( 1 - ( Esq * Math.Pow( Math.Sin( GeodeticLat ), 2 ) ) );
				
				GeocentricX = ( V + GeodeticHeight ) * Math.Cos( GeodeticLat ) * Math.Cos( GeodeticLon );
				GeocentricY = ( V + GeodeticHeight ) * Math.Cos( GeodeticLat ) * Math.Sin( GeodeticLon );
				GeocentricZ = ( ( 1 - Esq ) * V + GeodeticHeight ) * Math.Sin( GeodeticLat );
			}

			public static void GeocentricToGeodetic( double x, double y, double z, 
			                                         out double lat, out double lon, out double alt, RefEllipsoid R )
			{
				double  a, b, t;
				
				GetEllipsoidAxis( R, out a, out b );
				
				double  e2  = 1.0 - ( b * b ) / ( a * a ) ;   // 1st eccentricity sqrd
				double  ed2 = ( a * a ) / ( b * b ) - 1.0 ;   // 2nd eccentricity sqrd
				double  a2  = a * a ;
				double  b2  = b * b ;
				double  z2  = z * z ;
				double  e4  = e2 * e2 ;
				double  r2  = x * x + y * y ;
				double  r   = Math.Sqrt( r2 );
				
				double  E2 = a2 - b2 ;
				
				double  F = 54.0 * b2 * z2 ;
				
				double  G = r2 + ( 1.0 - e2 ) * z2 - e2 * E2 ;
				
				double  C = e4 * F * r2 / ( G * G * G );
				
				double  S = Math.Pow( 1.0 + C + Math.Sqrt( C * C + 2.0 * C ) , 1.0 / 3.0 );
				
				t = S + 1.0 / S + 1.0 ;
				
				double  P = F / ( 3.0 * t * t * G * G );
				
				double  Q = Math.Sqrt( 1.0 + 2.0 * e4 * P );
				
				double r0 = -( P * e2 * r ) / ( 1.0 + Q ) + Math.Sqrt( 0.5 * a2 * ( 1.0 + 1.0 / Q ) - ( P * ( 1 - e2 ) * z2 ) / ( Q * ( 1.0 + Q ) ) - 0.5 * P * r2 );
				
				t = r - e2 * r0;
				double  U = Math.Sqrt( t * t + z2 );
				double  V = Math.Sqrt( t * t + ( 1.0 - e2 ) * z2 );
				
				t = b2 / ( a * V );
				
				alt = U * ( 1.0 - t );
				lat = Math.Atan2( z + ed2 * t * z , r );
				lon = Math.Atan2( y, x );
				
				// convert to degrees
				lat = RadToDeg( lat );
				lon = RadToDeg( lon );
			}
		}
	}
}