/* Copyright 2017 Sannel Software, L.L.C.

Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/
/*
Based on SparkFun's TMP102 Library

SparkFunTMP102 Library
Alex Wende @ SparkFun Electronics
Original Creation Date: April 29, 2016
https://github.com/sparkfun/Digital_Temperature_Sensor_Breakout_-_TMP102
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor.Temperature
{
	public class TMP102 : ITemperatureSensor
	{
		public const byte TEMPERATURE_REGISTER = 0x00;
		public const byte CONFIG_REGISTER = 0x01;
		public const byte T_LOW_REGISTER = 0x02;
		public const byte T_HIGH_REGISTER = 0x03;

		private readonly IWireDevice device;
		/// <summary>
		/// Initializes a new instance of the <see cref="TMP102"/> class.
		/// </summary>
		/// <param name="wire">The wire.</param>
		/// <param name="address">The address. (0x48,0x49,0x4A,0x4B)</param>
		/// <exception cref="ArgumentNullException">wire</exception>
		TMP102(IWire wire, byte address)
		{
			this.device = (wire ?? throw new ArgumentNullException(nameof(wire))).GetDeviceById(address);
		}

		TMP102(IWireDevice device)
		{
			this.device = device ?? throw new ArgumentNullException(nameof(device));
		}

		public double GetTemperatureCelsius()
		{
			var registerByte = new int[2]; // Store the data from the register here
			int digitalTemp; // Temperature stored in TMP102 register

			// Read Temperature
			// Change pointer address to temperature register (0)
			device.Write(TEMPERATURE_REGISTER);
			// Read from temperature register
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);

			// Bit 0 of second byte will always be 0 in 12-bit readings and 1 in 13-bit
			if ((registerByte[1] & 0x01) > 0)	// 13 bit mode
			{
				// Combine bytes to create a signed int
				digitalTemp = ((registerByte[0]) << 5) | (registerByte[1] >> 3);
				// Temperature data can be + or -, if it should be negative,
				// convert 13 bit to 16 bit and use the 2s compliment.
				if (digitalTemp > 0xFFF)
				{
					digitalTemp |= 0xE000;
				}
			}
			else	// 12 bit mode
			{
				// Combine bytes to create a signed int 
				digitalTemp = ((registerByte[0]) << 4) | (registerByte[1] >> 4);
				// Temperature data can be + or -, if it should be negative,
				// convert 12 bit to 16 bit and use the 2s compliment.
				if (digitalTemp > 0x7FF)
				{
					digitalTemp |= 0xF000;
				}
			}
			// Convert digital reading to analog temperature (1-bit is equal to 0.0625 C)
			return digitalTemp*0.0625;
		}

		/// <summary>
		/// Sleeps the device to low power mode
		/// </summary>
		public void Sleep()
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (0x01)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = device.WriteRead(0);

			registerByte |= 0x01;   // Set SD (bit 0 of first byte)

			// Set configuration register
			device.Write(CONFIG_REGISTER, // Point to configuration register
				registerByte); // write first byte
		}

		/// <summary>
		/// Wakeups the device and start running in normal power mode
		/// </summary>
		public void Wakeup()
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = device.WriteRead(0);

			registerByte &= 0xFE;	// Clear SD (bit 0 of first byte)

			// Set configuration registers
			device.Write(CONFIG_REGISTER, // Point to configuration register
						registerByte); // Write first byte
		}

		/// <summary>
		/// Returns the state of the Alert register
		/// </summary>
		/// <returns></returns>
		public bool Alert()
		{

			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = device.WriteRead(1);

			registerByte &= 0x20; // Clear everything but the alert bit (bit 5)
			return (registerByte >> 5) > 0;
		}

		public void SetConversionRate(byte rate)
		{
			var registerByte = new int[2]; // Store the data from the register here
			rate = (byte)(rate & 0x03); // Make sure rate is not set higher than 3.

			// Change pointer address to configuration register (0x01)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);

			// Load new conversion rate
			registerByte[1] &= 0x3F;  // Clear CR0/1 (bit 6 and 7 of second byte)
			registerByte[1] |= rate << 6;   // Shift in new conversion rate

			// Set configuration registers
			device.Write(CONFIG_REGISTER,
						(byte)registerByte[0],
						(byte)registerByte[1]);
		}
		public void SetExtendedMode(bool mode)
		{
			var registerByte = new int[2]; // Store the data from the register here

			// Change pointer address to configuration register (0x01)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);

			int m = mode ? 1 : 0;
			// Load new value for extention mode
			registerByte[1] &= 0xEF;		// Clear EM (bit 4 of second byte)
			registerByte[1] |= m << 4;	// Shift in new exentended mode bit

			// Set configuration registers
			device.Write(CONFIG_REGISTER, // Point to configuration register
						(byte)registerByte[0],	// Write first byte
						(byte)registerByte[1]); 	// Write second byte
		}

		public void SetAlertPolarity(bool polarity)
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = device.WriteRead(0);

			// Load new value for polarity
			registerByte &= 0xFB; // Clear POL (bit 2 of registerByte)
			registerByte |= (byte)((polarity?1:0) << 2);  // Shift in new POL bit

			// Set configuration register
			device.Write(CONFIG_REGISTER,	// Point to configuration register
						registerByte);	    // Write first byte
		}
		public void SetLowTempC(double temperature)
		{
			var registerByte = new int[2];	// Store the data from the register here
			bool extendedMode;	// Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C

								// Prevent temperature from exceeding 150C or -55C
			if (temperature > 150.0f)
			{
				temperature = 150.0f;
			}
			if (temperature < -55.0)
			{
				temperature = -55.0f;
			}

			//Check if temperature should be 12 or 13 bits
			device.Write(CONFIG_REGISTER);	// Read configuration register settings

													// Read current configuration register value
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;	// 0 - temp data will be 12 bits
															// 1 - temp data will be 13 bits

															// Convert analog temperature to digital value
			temperature = temperature / 0.0625;

			// Split temperature into separate bytes
			if (extendedMode)	// 13-bit mode
			{
				registerByte[0] = (int)temperature >> 5;
				registerByte[1] = ((int)temperature << 3);
			}
			else	// 12-bit mode
			{
				registerByte[0] = (int)temperature >> 4;
				registerByte[1] = (int)temperature << 4;
			}

			// Write to T_LOW Register
			device.Write(T_LOW_REGISTER, 	// Point to T_LOW
						(byte)registerByte[0],  // Write first byte
						(byte)registerByte[1]);  // Write second byte
		}


		public void SetHighTempC(double temperature)
		{
			var registerByte = new int[2];	// Store the data from the register here
			bool extendedMode;	// Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C

								// Prevent temperature from exceeding 150C
			if (temperature > 150.0f)
			{
				temperature = 150.0f;
			}
			if (temperature < -55.0)
			{
				temperature = -55.0f;
			}

			// Check if temperature should be 12 or 13 bits
			device.Write(CONFIG_REGISTER);	// Read configuration register settings

													// Read current configuration register value
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0; // 0 - temp data will be 12 bits
															// 1 - temp data will be 13 bits

			// Convert analog temperature to digital value
			temperature = temperature / 0.0625;

			// Split temperature into separate bytes
			if (extendedMode)	// 13-bit mode
			{
				registerByte[0] = (int)temperature >> 5;
				registerByte[1] = ((int)temperature << 3);
			}
			else	// 12-bit mode
			{
				registerByte[0] = (int)temperature >> 4;
				registerByte[1] = (int)temperature << 4;
			}

			// Write to T_HIGH Register
			device.Write(T_HIGH_REGISTER, 	// Point to T_HIGH register
						(byte)registerByte[0],  // Write first byte
						(byte)registerByte[1]);  // Write second byte
		}

		public double ReadLowTempC()
		{
			var registerByte = new int[2];	// Store the data from the register here
			bool extendedMode;	// Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C
			int digitalTemp;		// Store the digital temperature value here

			// Check if temperature should be 12 or 13 bits
			device.Write(CONFIG_REGISTER);	// Read configuration register settings
											// Read current configuration register value

			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;	// 0 - temp data will be 12 bits
																// 1 - temp data will be 13 bits
			device.Write(T_LOW_REGISTER);
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);

			if (extendedMode)	// 13 bit mode
			{
				// Combine bytes to create a signed int
				digitalTemp = ((registerByte[0]) << 5) | (registerByte[1] >> 3);
				// Temperature data can be + or -, if it should be negative,
				// convert 13 bit to 16 bit and use the 2s compliment.
				if (digitalTemp > 0xFFF)
				{
					digitalTemp |= 0xE000;
				}
			}
			else	// 12 bit mode
			{
				// Combine bytes to create a signed int 
				digitalTemp = ((registerByte[0]) << 4) | (registerByte[1] >> 4);
				// Temperature data can be + or -, if it should be negative,
				// convert 12 bit to 16 bit and use the 2s compliment.
				if (digitalTemp > 0x7FF)
				{
					digitalTemp |= 0xF000;
				}
			}
			// Convert digital reading to analog temperature (1-bit is equal to 0.0625 C)
			return digitalTemp*0.0625;
		}


		public double ReadHighTempC()
		{
			var registerByte = new int[2];	// Store the data from the register here
			bool extendedMode;	// Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C
			int digitalTemp;		// Store the digital temperature value here

			// Check if temperature should be 12 or 13 bits
			device.Write(CONFIG_REGISTER);	// read configuration register settings
											// Read current configuration register value
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;	// 0 - temp data will be 12 bits
																// 1 - temp data will be 13 bits
			device.Write(T_HIGH_REGISTER);
			registerByte[0] = device.WriteRead(0);
			registerByte[1] = device.WriteRead(1);

			if (extendedMode)	// 13 bit mode
			{
				// Combine bytes to create a signed int
				digitalTemp = ((registerByte[0]) << 5) | (registerByte[1] >> 3);
				// Temperature data can be + or -, if it should be negative,
				// convert 13 bit to 16 bit and use the 2s compliment.
				if (digitalTemp > 0xFFF)
				{
					digitalTemp |= 0xE000;
				}
			}
			else	// 12 bit mode
			{
				// Combine bytes to create a signed int 
				digitalTemp = ((registerByte[0]) << 4) | (registerByte[1] >> 4);
				// Temperature data can be + or -, if it should be negative,
				// convert 12 bit to 16 bit and use the 2s compliment.
				if (digitalTemp > 0x7FF)
				{
					digitalTemp |= 0xF000;
				}
			}
			// Convert digital reading to analog temperature (1-bit is equal to 0.0625 C)
			return digitalTemp*0.0625;
		}

		public void SetFault(byte faultSetting)
		{
			byte registerByte; // Store the data from the register here

			faultSetting = (byte)(faultSetting & 3); // Make sure rate is not set higher than 3.

			// Change pointer address to configuration register (0x01)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = device.WriteRead(0);

			// Load new conversion rate
			registerByte &= 0xE7;  // Clear F0/1 (bit 3 and 4 of first byte)
			registerByte |= (byte)(faultSetting << 3);// Shift new fault setting

			// Set configuration registers
			device.Write(CONFIG_REGISTER, 	// Point to configuration register
						registerByte);     // Write byte to register
		}

		public void SetAlertMode(bool mode)
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			device.Write(CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = device.WriteRead(0);

			// Load new conversion rate
			registerByte &= 0xFD;	// Clear old TM bit (bit 1 of first byte)
			registerByte |= (byte)((mode?1:0) << 1);	// Shift in new TM bit

			// Set configuration registers
			device.Write(CONFIG_REGISTER, 	// Point to configuration register
						registerByte);     // Write byte to register
		}

		public void Begin()
		{
		}

		public void Dispose()
		{
			device?.Dispose();
		}

	}
}
