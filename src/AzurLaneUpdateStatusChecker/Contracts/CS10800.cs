using ProtoBuf;

namespace AzurLaneUpdateStatusChecker.Contracts
{
	[ProtoContract]
	public class CS10800
	{
		public static int id = 10800;

		[ProtoMember(1, IsRequired = true, Name = "state", DataFormat = DataFormat.TwosComplement)]
		public uint State = 21;

		[ProtoMember(2, IsRequired = true, Name = "platform", DataFormat = DataFormat.Default)]
		public string Platform = "0";
	}
}