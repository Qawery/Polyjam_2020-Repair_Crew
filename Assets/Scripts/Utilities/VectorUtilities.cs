using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polyjam2020
{
	public static class VectorUtilities
	{
		public static Vector3 Flat(this Vector3 vector)
		{
			return new Vector3(vector.x, 0, vector.z);
		}
	}


}
