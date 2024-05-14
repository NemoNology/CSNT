using SharpPcap;

byte[] GetRandomLacpPacketBytes()
{
    Random rnd = new(DateTime.Now.Millisecond);
    byte[] rndBytes(int bytesAmount)
    {
        var bytes = new byte[bytesAmount];
        rnd.NextBytes(bytes);
        return bytes;
    }
    List<byte> bytesList = new(128);
    bytesList.AddRange([0x01, 0x80, 0xC2, 0x00, 0x00, 0x02]);
    bytesList.AddRange(rndBytes(6));
    bytesList.AddRange([0x88, 0x09, 0x01]);
    bytesList.AddRange(rndBytes(109));
    return [.. bytesList];
}

uint delayMilliseconds = 1000;
uint packetsCount = 7;
switch (args.Length)
{
	case 1:
		_ = uint.TryParse(args[0], out delayMilliseconds);
		break;
	case 2:
		_ = uint.TryParse(args[1], out packetsCount)
		break;
	default: break;
}

Console.WriteLine($"Started sending packets [ Delay: {delayMilliseconds} ms; Packets amount: {packetsCount} ]");

while (true)
{
	int sendedPacketsCount = 0;
    foreach (var device in CaptureDeviceList.Instance)
    {
		try
        {
			if (sendedPacketsCount == packetsCount)
			{
				Console.WriteLine($"Ended sending;\nSuccessfully sended {packetsCount} packets;");
				return;
			}
			
            device.Open();
            device.SendPacket(GetRandomLacpPacketBytes());
            Console.WriteLine(
                $"Sended packet on {device.Name} ({device.Description})");
            device.Close();
			sendedPacketsCount++;

            Thread.Sleep((int)delayMilliseconds);
        }
        catch (Exception e)
        {
            Console.WriteLine(
                $"Error when sending packet on {device.Name} ({device.Description}):\n\t{e.Message}");
            device.Close();
        }
    }
}