using AzurLaneUpdateStatusChecker.Enums;

namespace AzurLaneUpdateStatusChecker.Records
{
	public record CdnGateway
	{
		public CdnGateway(Region region, string gatewayHost, int gatewayPort = 0, string cdnUrl = null)
		{
			Region = region;
			GatewayHost = gatewayHost;
			GatewayPort = gatewayPort;
			CdnUrl = cdnUrl;
		}
		public Region Region;
		public string GatewayHost;
		public int GatewayPort;
		public string CdnUrl;
	}
}