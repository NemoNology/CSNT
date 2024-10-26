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
    // Destination MAC
    bytesList.AddRange([0x01, 0x80, 0xC2, 0x00, 0x00, 0x02]);
    // Source MAC
    bytesList.AddRange(rndBytes(6));
    // Type\Length of slow protocol + subtype
    bytesList.AddRange([0x88, 0x09, 0x01]);
    // Random version
    bytesList.AddRange(rndBytes(1));

    // Random TLVs
    for (int i = 0; i < rnd.Next(0, 5); i++)
    {
        // Random tag value in TLV from 0 to 5
        bytesList.Add((byte)rnd.Next(0, 6));
        // Random length
        var length = (byte)rnd.Next(0, 255);
        bytesList.Add(length);
        // Random content
        if (DateTime.Now.Millisecond % 2 == 0)
            bytesList.AddRange(rndBytes(rnd.Next(0, 500)));
        else
            bytesList.AddRange(rndBytes(length));
    }

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
        _ = uint.TryParse(args[1], out packetsCount);
        break;
    default: break;
}

Console.WriteLine($"Started sending packets [ Delay: {delayMilliseconds} ms; Packets amount: {packetsCount} ]");

for (int i = 0; i < packetsCount; i++)
{
    var device = CaptureDeviceList
        .Instance
        .Where(d => d.Name == "\\Device\\NPF_{9D3F39FF-B9C3-4C72-815B-7C1A82202756}")
        .First();
    int sendedPacketsCount = 0;
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
            $"Sended packet on ({device.Description})");
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