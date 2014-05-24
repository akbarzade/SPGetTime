﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SPGetTime.ClientCApp
{
  class Program
  {
	#region Define variables
	private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	#endregion
	static void Main(string[] args)
	{
	  Console.Title = "Client";
	  LoopConnect();
	  SendLoop();
	  Console.ReadLine();
	}
	#region Define Methode's
	private static void SendLoop()
	{
	  while (true)
	  {
		Console.Write("Enter a request: ");
		string req = Console.ReadLine();
		  if (req != null)
		  {
			  byte[] buffer = Encoding.ASCII.GetBytes(req);
			  _clientSocket.Send(buffer);
		  }

		  byte[] receivedBuf = new byte[1024];
		int rec = _clientSocket.Receive(receivedBuf);
		byte[] data = new byte[rec];
		Array.Copy(receivedBuf, data, rec);
		Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));
	  }
	}
	private static void LoopConnect()
	{
	  int attempts = 0;
	  while (!_clientSocket.Connected)
	  {
		try
		{
		  attempts++;
		  _clientSocket.Connect(IPAddress.Loopback, 100);
		}
		catch (SocketException)
		{
		  Console.Clear();
		  Console.WriteLine("Connection attempts:" + attempts.ToString());
		}
	  }
	  Console.Clear();
	  Console.WriteLine("Connected");
	}
	#endregion
  }
}