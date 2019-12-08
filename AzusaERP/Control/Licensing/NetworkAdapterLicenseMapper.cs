using System;
using System.Net.NetworkInformation;

namespace moe.yo3explorer.azusa.Control.Licensing
{
    static class NetworkAdapterLicenseMapper
    {
        public static byte[] GetLicenseMapping()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.Description.StartsWith("TAP-"))
                    continue;
                if (networkInterface.Description.Contains("Virtual"))
                    continue;
                if (networkInterface.Description.Contains("Bluetooth"))
                    continue;
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                Console.WriteLine("{0} -> {1}",networkInterface.Description,networkInterface.Id);

                PlatformID platform = Environment.OSVersion.Platform;
                Guid idGuid;
                switch (platform)
                {
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        idGuid = Guid.Parse(networkInterface.Id);
                        break;
                    case PlatformID.Unix:
                        idGuid = LinuxWorkaround(networkInterface.Id);
                        break;
                    default:
                        throw new NotImplementedException(platform.ToString());
                }
                
                return idGuid.ToByteArray();
            }

            throw new LicenseValidationFailedException(LicenseState.NoUsableNetworkAdapter);
        }

        private static Guid LinuxWorkaround(string id)
        {
            Guid azusaLinuxNetNamespace = Guid.Parse("6508dae8-ebc2-4635-9d3e-2f5493b1cec5");
            return GuidUtils.MakeVersion3(azusaLinuxNetNamespace, id);
        }
    }
}
