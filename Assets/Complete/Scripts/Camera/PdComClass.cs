// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace Complete
{
	public class PdComClass : MonoBehaviour
	{
		private int port = 7778;
		private string host = "127.0.0.1";
		
		public TcpClient client;
		public StreamWriter writer;

		public PdComClass (){}

		public void setupTCP() {
			client = new TcpClient(host, port);
			writer = new StreamWriter(client.GetStream());  
			Debug.Log("Connection made!");
		}

		public void send(string command) {
			writer.Write(command +" ;");
			writer.Flush();
			Debug.Log("Command send: " + command);
		}

		public void Awake(){
			setupTCP();
		}
		
		public void Update () {
		}
	}
}

