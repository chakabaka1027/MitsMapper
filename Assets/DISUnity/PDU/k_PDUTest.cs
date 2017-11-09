using UnityEngine;
using System.Collections;
//using NUnit.Framework;
using DISUnity.PDU.EntityInfoInteraction;
using DISUnity.DataType;
using System.IO;

public class k_PDUTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		EncodeDecodeMatch_EntityState ();	
	}

	public void EncodeDecodeMatch_EntityState()
	{
		var pduOut = new EntityState();
		var stream = new MemoryStream(pduOut.Length);
		var bw = new BinaryWriter(stream);
		pduOut.Encode(bw);
		stream.Position = 0;

		var br = new BinaryReader(stream);
		var pduIn = new EntityState();
		pduIn.Decode(br);

		//Assert.IsTrue(pduIn.Equals(pduOut));

		Debug.Log (pduIn);
	}
}
