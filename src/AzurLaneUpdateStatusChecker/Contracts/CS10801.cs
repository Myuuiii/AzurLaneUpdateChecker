using System.ComponentModel;
using Newtonsoft.Json;
using ProtoBuf;

namespace AzurLaneUpdateStatusChecker.Contracts
{
	[ProtoContract]
	class SC10801
	{
		public static int id = 10801;
		[ProtoMember(1, IsRequired = true, Name = "gateway_ip", DataFormat = DataFormat.Default)]
		[JsonProperty("gateway_ip")]
		public string GatewayIp { get; set; }

		[ProtoMember(2, IsRequired = true, Name = "gateway_port", DataFormat = DataFormat.TwosComplement)]
		[JsonProperty("gateway_port")]
		public uint GatewayPort { get; set; }

		[ProtoMember(3, IsRequired = true, Name = "url", DataFormat = DataFormat.Default)]
		[JsonProperty("url")]
		public string Url { get; set; }

		[ProtoMember(4, Name = "version", DataFormat = DataFormat.Default)]
		[JsonProperty("versions")]
		public List<string> Versions { get; set; }

		[ProtoMember(5, IsRequired = false, Name = "proxy_ip", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		[JsonProperty("proxy_ip")]
		public string ProxyIp { get; set; }

		[ProtoMember(6, IsRequired = false, Name = "proxy_port", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		[JsonProperty("proxy_port")]
		public uint ProxyPort { get; set; }

		[JsonProperty("cdn_url")]
		public string CdnUrl { get; set; }
	}
}