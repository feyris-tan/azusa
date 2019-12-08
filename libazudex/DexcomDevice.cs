using System;
using System.IO;
using System.Text;
using moe.yo3explorer.azusa.dex.IO;
using moe.yo3explorer.azusa.dex.Schema;
using moe.yo3explorer.azusa.dex.Schema.Enums;

namespace moe.yo3explorer.azusa.dex
{
    
    public class DexcomDevice
    {
        private Stream stream;

        public DexcomDevice(Stream stream)
        {
            this.stream = stream;
        }

        public ILogCallback LogCallback { get; set; }

        private PacketReader ReadPacket()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                System.Threading.Thread.Sleep(100);
            }
            byte[] buffer = new byte[PacketWriter.MAX_LEN];
            int ptr = 0;
            ptr += stream.Read(buffer, 0, 3);
            if (ptr == 1)
            {
                LogCallback.LogEvent("Packet reading delay!");
                System.Threading.Thread.Sleep(100);
                ptr += stream.Read(buffer, 1, 2);
            }
            ushort len = BitConverter.ToUInt16(buffer, 1);
            ushort left = len;
            left -= 3;
            while (left > 0)
            {
                int readAmount = stream.Read(buffer, ptr, left);
                ptr += readAmount;
                left -= (ushort) readAmount;
            }

            ushort expected = BitConverter.ToUInt16(buffer, len - 2);
            ushort actual = CRC16.CalculateCRC16(buffer, 0, len - 2);
            if (expected != actual)
            {
                throw new Exception("crc mismatch!");
            }

            return new PacketReader(buffer);
        }

        private void WriteCommand(DexcomCommands command, byte[] args = null)
        {
            PacketWriter pw = new PacketWriter();
            pw.ComposePacket(command, args);
            byte[] ps = pw.PacketString();
            stream.Write(ps, 0, ps.Length);
            stream.Flush();
        }

        public bool Ping()
        {
            if (LogCallback != null) LogCallback.LogEvent("Pinging...");
            WriteCommand(DexcomCommands.PING);
            PacketReader result = ReadPacket();
            return result.Command == DexcomCommands.ACK;
        }

        public LanguageCode ReadLanguage()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Language...");
            WriteCommand(DexcomCommands.READ_LANGUAGE);
            PacketReader result = ReadPacket();
            return (LanguageCode)BitConverter.ToInt16(result.Payload, 0);    //siehe: http://www.robvanderwoude.com/languagecodes.php
        }

        public int ReadBatteryLevel()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Battery Level...");
            WriteCommand(DexcomCommands.READ_BATTERY_LEVEL);
            PacketReader result = ReadPacket();
            return BitConverter.ToInt32(result.Payload, 0);
        }

        public FirmwareHeader ReadFirmwareHeader()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Firmware Header...");
            WriteCommand(DexcomCommands.READ_FIRMWARE_HEADER);
            PacketReader result = ReadPacket();
            return new FirmwareHeader(result.Payload);
        }

        public PartitionInfo ReadDatabaseParitionInfo()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Database Partition Info...");
            WriteCommand(DexcomCommands.READ_DATABASE_PARTITION_INFO);
            PacketReader result = ReadPacket();
            return PartitionInfo.Parse(result.Payload);
        }

        public DatabasePageRange ReadDatabasePageRange(Partition partition)
        {
            if (LogCallback != null) LogCallback.LogEvent(String.Format("Reading Database page range of {0}...", partition.Name));
            byte[] payload = new byte[] {Convert.ToByte(partition.Id)};
            WriteCommand(DexcomCommands.READ_DATABASE_PAGE_RANGE, payload);
            PacketReader result = ReadPacket();
            return new DatabasePageRange(result.Payload);
        }

        public DatabasePage ReadDatabasePage(Partition record_type, int page)
        {
            if (LogCallback != null) LogCallback.LogEvent(String.Format("Reading Database Page {1} of {0}...", record_type.Name, page));
            byte[] pageBuffer = BitConverter.GetBytes(page);
            byte[] payload = new byte[]
                {Convert.ToByte(record_type.Id), pageBuffer[0], pageBuffer[1], pageBuffer[2], pageBuffer[3], 1};
            WriteCommand(DexcomCommands.READ_DATABASE_PAGES, payload);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return new DatabasePage(result.Payload);
        }

        public string ReadTransmitterId()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Transmitter ID...");
            WriteCommand(DexcomCommands.READ_TRANSMITTER_ID);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return Encoding.UTF8.GetString(result.Payload);
        }

        public TimeSpan ReadDisplayTimeOffset()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Display Time Offset...");
            WriteCommand(DexcomCommands.READ_DISPLAY_TIME_OFFSET);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            int m = BitConverter.ToInt32(result.Payload, 0);
            return new TimeSpan(0, 0, m);
        }

        public DateTime ReadRTC()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading RTC...");
            WriteCommand(DexcomCommands.READ_RTC);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            int dtime = BitConverter.ToInt32(result.Payload, 0);
            DateTime dt = (new DateTime(2009, 1, 1));
            dt += new TimeSpan(TimeSpan.TicksPerSecond * dtime);
            return dt;
        }

        public DateTime ReadSystemTime()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading System Time...");
            WriteCommand(DexcomCommands.READ_SYSTEM_TIME);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            int dtime = BitConverter.ToInt32(result.Payload, 0);
            DateTime dt = (new DateTime(2009, 1, 1));
            dt += new TimeSpan(TimeSpan.TicksPerSecond * dtime);
            return dt;
        }

        public DateTime ReadSystemTimeOffset()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading System Time Offset...");
            WriteCommand(DexcomCommands.READ_SYSTEM_TIME_OFFSET);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            int dtime = BitConverter.ToInt32(result.Payload, 0);
            DateTime dt = (new DateTime(2009, 1, 1));
            dt += new TimeSpan(TimeSpan.TicksPerSecond * dtime);
            return dt;
        }

        public GlucoseUnit ReadGlucoseUnit()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Glucose Unit...");
            WriteCommand(DexcomCommands.READ_GLUCOSE_UNIT);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return (GlucoseUnit) result.Payload[0];
        }

        public bool ReadBlindedMode()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Blinded Mode...");
            WriteCommand(DexcomCommands.READ_BLINDED_MODE);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return result.Payload[0] != 0;
        }

        public ClockMode ReadClockMode()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Clock Mode...");
            WriteCommand(DexcomCommands.READ_CLOCK_MODE);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return (ClockMode) result.Payload[0];
        }

        public BatteryState ReadBatteryState()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Battery State...");
            WriteCommand(DexcomCommands.READ_BATTERY_STATE);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return (BatteryState) result.Payload[0];
        }

        public ushort ReadHardwareBoardID()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Hardware Board ID...");
            WriteCommand(DexcomCommands.READ_HARDWARE_BOARD_ID);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return BitConverter.ToUInt16(result.Payload, 0);
        }

        public bool ReadEnableSetupWizardFlag()
        {
            if (LogCallback != null) LogCallback.LogEvent("Reading Enable Setup Wizard Flag...");
            WriteCommand(DexcomCommands.READ_ENABLE_SETUP_WIZARD_FLAG);
            PacketReader result = ReadPacket();
            if (result.Command != DexcomCommands.ACK)
                throw new Exception();
            return result.Payload[0] != 0;
        }
    }
}