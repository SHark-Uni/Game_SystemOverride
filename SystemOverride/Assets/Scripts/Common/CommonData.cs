using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Common
{ 
	public enum eLayerMask
	{
		Player = 1 << 6,
		Monster = 1 << 7,
		Ground = 1 << 8,
	}

	public enum eLayerNumber
	{
		Player = 6,
		Monster = 7,
		Ground = 8,
	}
}
