using UnityEngine;
using System.Collections;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.DataType;
using DISUnity.Simulation.Managers;

namespace DISUnity.Simulation
{
    /// <summary>
    /// A simulated entity in the DIS exercise NOT controlled by us.
    /// </summary>
    public class RemoteEntity : Entity
    {     
        //*
		//Enter these values from the terrain
		//SW_CornerX should be the X value of the 3D cartesian coordinates of your terrain's SW corner
		private float SW_CornerX = 1105453.7970f;

		//SE_CornerX should be the X value of the 3D cartesian coordinates of your terrain's SE corner
		private float SE_CornerX = 1105699.3987f;

		//SW_CornerZ should be the Z value of the 3D cartesian coordinates of your terrain's SW corner
		private float SW_CornerZ = 3975686.7542f;

		//NW_CornerZ should be the Z value of the 3D cartesian coordinates of your terrain's NW corner
		private float NW_CornerZ = 3975903.0170f;

		//Min_HeightY should be the Y value of the 3D cartesian coordinates of your terrain's lowest elevation point
		private float Min_HeightY = -5240820.4549f;

		//Max_HeightY should be the Y value of the 3D cartesian coordinates of your terrain's highest elevation point
		private float Max_HeightY = -5239733.3769f;

		//Terrain_Width should be the width of your terrain in Meters (from West to East)
		private float Terrain_Width = 1930.0f;

		//Terrain_Height should be the height of your terrain in Meters (from South to North)
		private float Terrain_Height = 1940.0f;

		//Terrain_Elev should be the z-range (elevation range) of your terrain in Meters (how many meters from the lowest point to the highest point)
		private float Terrain_Elev = 50.0f;
		//*

		/// <summary>
        /// Called when the entity is created.
        /// </summary>
        /// <param name="es">The first EntityState PDU for this entity.</param>
        public virtual void Init( EntityState es )
        {
            State = es;

            // Set GameObject name
            name = es.Marking.ASCII;
        }

        /// <summary>
        /// Update the entity with the new state.
        /// </summary>
        /// <param name="es"></param>
        public virtual void UpdateEntity( EntityState es )
        {
            State = es;

			//*
			//Re-map X to X
			float reMapX = MapInterval ((float) es.Location.X, SW_CornerX, SE_CornerX, 0.0f, Terrain_Width);
			//Re-map Y to Z (these are flipped going from DIS to Unity)
			float reMapZ = MapInterval ((float) es.Location.Y, Min_HeightY, Max_HeightY, 0.0f, Terrain_Height);
			//Re-map Z to Y  (these are flipped going from DIS to Unity)
			float reMapY = MapInterval ((float) es.Location.Z, SW_CornerZ, NW_CornerZ, 0.0f, Terrain_Elev);
			//*

            // TODO: Smoothing
            // Update our position            
			transform.position = new Vector3(reMapX, reMapY, reMapZ);
        }

		//*
		float MapInterval (float val, float srcMin, float srcMax, float dstMin, float dstMax) {
			if (val>=srcMax) return dstMax;
			if (val<=srcMin) return dstMin;
			return dstMin + (val-srcMin) / (srcMax-srcMin) * (dstMax-dstMin);
		}
		//*
    }
}