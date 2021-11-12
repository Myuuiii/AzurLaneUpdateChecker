using System.Net.Sockets;
using AzurLaneUpdateStatusChecker.Contracts;
using AzurLaneUpdateStatusChecker.Records;
using Newtonsoft.Json;
using ProtoBuf;

namespace AzurLaneUpdateStatusChecker
{
	public class Program
	{
		public static List<CdnGateway> cdnGateways = new List<CdnGateway>()
		{
			new CdnGateway(Enums.Region.EN, "blhxusgate.yo-star.com", 80, "https://blhxusstatic.akamaized.net"),
			new CdnGateway(Enums.Region.CN, "line1-login-bili-blhx.bilibiligame.net", 80, "https://line3-patch-blhx.bilibiligame.net"),
			new CdnGateway(Enums.Region.JP, "blhxjploginapi.azurlane.jp", 80, "https://blhxstatic.akamaized.net"),
			new CdnGateway(Enums.Region.KR, "bl-kr-gate.xdg.com", 80, "http://blcdn.imtxwy.com")
		};

		private static byte[] CombineByteArray(byte[][] arrays)
		{
			byte[] result = new byte[arrays.Sum(a => a.Length)];
			int offset = 0;
			foreach (byte[] array in arrays)
			{
				Buffer.BlockCopy(array, 0, result, offset, array.Length);
				offset += array.Length;
			}
			return result;
		}

		public static void Main(string[] args)
		{
			if (!Directory.Exists("./data"))
			{
				Directory.CreateDirectory("./data");
			}

			var memoryStream = new MemoryStream();
			Serializer.Serialize(memoryStream, new CS10800());
			var binary = memoryStream.ToArray();
			memoryStream.Close();

			byte[][] basePacket = {
				BitConverter.GetBytes((ushort)(5 + binary.Length)),
				BitConverter.GetBytes(false),
				BitConverter.GetBytes((ushort)10800u),
				BitConverter.GetBytes((ushort)0u),
				binary
			};

			// workaround endianness
			if (BitConverter.IsLittleEndian)
				for (var i = 0; i <= 3; i++)
					basePacket[i] = basePacket[i].Reverse().ToArray();

			foreach (CdnGateway cdnGateway in cdnGateways)
			{
				var packet = CombineByteArray(basePacket);
				var gateway = cdnGateway.GatewayHost;
				var port = cdnGateway.GatewayPort;

				var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 5000 };

				if (
					socket.BeginConnect(
						gateway,
						port,
						null, null
					).AsyncWaitHandle.WaitOne(5000, true)
				)
				{
					socket.Send(packet);
					var output = new byte[1024 * 64];
					socket.Receive(output);

					// decode the buffer
					var stream = new MemoryStream(output, 7, ((output[0] << 8) | output[1]) - 5);
					var response = Serializer.Deserialize<SC10801>(stream);
					stream.Close();

					response.CdnUrl = cdnGateway.CdnUrl;

					string storedContentFilename = $"./data/{cdnGateway.Region}.json";
					string fetchedContent = JsonConvert.SerializeObject(response, Formatting.Indented);
					if (File.Exists(storedContentFilename))
					{
						string storedContent = File.ReadAllText(storedContentFilename);

						if (fetchedContent == storedContent)
						{
							Console.WriteLine($"Region {cdnGateway.Region}: Not Updated");
						}
						else
						{
							Console.WriteLine($"Region {cdnGateway.Region}: Was Updated");
							File.WriteAllText(storedContentFilename, fetchedContent);
						}
					}
					else
					{
						Console.WriteLine($"Region {cdnGateway.Region}: No stored content, stored fetched data");
						File.WriteAllText(storedContentFilename, fetchedContent);
					}
				}
			}
		}
	}
}