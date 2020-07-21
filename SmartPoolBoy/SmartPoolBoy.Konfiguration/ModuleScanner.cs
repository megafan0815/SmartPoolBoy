using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SmartPoolBoy.Konfiguration
{
	public class DeviceInfo
    {
		public byte DeviceSlaveAddress { get; }
		public string DeviceDescription { get; }

        public DeviceInfo(byte deviceSlaveAddress, string deviceDescription)
        {
            DeviceSlaveAddress = deviceSlaveAddress;
            DeviceDescription = deviceDescription;
        }
    }

    public class ModuleScanner
    {
        public ModuleScanner()
        {

        }

        public List<DeviceInfo> ScanForI2CDevices()
        {
			var result = new List<DeviceInfo>();
            foreach (var devInfo in FindDevicesAsync().Result)
            {
				result.Add(new DeviceInfo(devInfo.Key, devInfo.Value));
            }
			return result;
        }

		public async Task<IEnumerable<KeyValuePair<byte, string>>> FindDevicesAsync()
		{
			IList<KeyValuePair<byte, string>> returnValue = new List<KeyValuePair<byte, string>>();
			// *** 
			// *** Get a selector string that will return all I2C controllers on the system 
			// *** 
			string aqs = I2cDevice.GetDeviceSelector();
            // *** 
            // *** Find the I2C bus controller device with our selector string 
            // *** 
            DeviceInformationCollection dis = await DeviceInformation.FindAllAsync(aqs).AsTask();
			if (dis.Count > 0)
			{
				const int minimumAddress = 0x08;
				const int maximumAddress = 0x77;
				for (byte address = minimumAddress; address <= maximumAddress; address++)
				{
					var settings = new I2cConnectionSettings(address);
					settings.BusSpeed = I2cBusSpeed.FastMode;
					settings.SharingMode = I2cSharingMode.Shared;
					// *** 
					// *** Create an I2cDevice with our selected bus controller and I2C settings 
					// *** 
					using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
					{
						if (device != null)
						{
							try
							{
								byte[] writeBuffer = new byte[1] { 0 };
								device.Write(writeBuffer);
								// *** 
								// *** If no exception is thrown, there is 
								// *** a device at this address. 
								// *** 
								// TODO hier könnte man jetzt eine Art Device-Info vom Device abfragen
								//byte[] readBuffer = new byte[1];
								//device.Read(readBuffer);
								//if (readBuffer[0] == 0x2D)
								//{
								//	returnValue.Add(address);
								//}
								returnValue.Add(new KeyValuePair<byte, string>(address, "TODO"));
							}
							catch
							{
								// *** 
								// *** If the address is invalid, an exception will be thrown. 
								// *** 
							}
						}
					}
				}
			}
			return returnValue;
		}
	}
}
