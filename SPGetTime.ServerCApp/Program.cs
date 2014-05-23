﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGetTime.ServerCApp
{

  class Program
  {
	private static byte[] _buffer = new byte[1024];
	private static List<Socket> _clientSockets = new List<Socket>();
	private static Socket _serverSocket = new Socket
		(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	Socket server;

	private static Socket _serversocket = new Socket
		  (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	static void Main(string[] args)
	{
	  Console.Title = "Server";
	  SetupServer();
	  Console.ReadLine();
	}
	private static void SetupServer()
	{
	  Console.WriteLine("Setting up server…");
	  _serversocket.Bind(new IPEndPoint(IPAddress.Any, 100));
	  _serversocket.Listen(1);
	  _serversocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
	}
	private static void AcceptCallback(IAsyncResult AR)
	{
	  Socket socket = _serverSocket.EndAccept(AR);
	  _clientSockets.Add(socket);
	  Console.WriteLine("Client Connected");
	  socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
	  _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);


	}
	private static void ReceiveCallback(IAsyncResult AR)
	{
	  Socket socket = (Socket)AR.AsyncState;
	  int received = socket.EndReceive(AR);
	  byte[] dataBuf = new byte[received];
	  Array.Copy(_buffer, dataBuf, received);
	  string text = Encoding.ASCII.GetString(dataBuf);
	  Console.WriteLine("Text received: " + text);

	  string response = string.Empty;

	  if (text.ToLower() != "get time")
	  {
		response = "Invalid Request";
	  }
	  else
	  {
		response = DateTime.Now.ToLongTimeString();
	  }
	  byte[] data = Encoding.ASCII.GetBytes(response);
	  socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
	  socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
	}

	private static void SendCallback(IAsyncResult AR)
	{
	  Socket socket = (Socket)AR.AsyncState;
	  socket.EndSend(AR);
	}
  }
 
}
