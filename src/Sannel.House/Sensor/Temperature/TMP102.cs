/* Copyright 2018 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/

/******************************************************************************
SparkFunTMP102.cpp
SparkFunTMP102 Library Source File
Alex Wende @ SparkFun Electronics
Original Creation Date: April 29, 2016
https://github.com/sparkfun/Digital_Temperature_Sensor_Breakout_-_TMP102
This file implements all functions of the TMP102 class. Functions here range
from reading the temperature from the sensor, to reading and writing various
settings in the sensor.
Development environment specifics:
IDE: Arduino 1.6
Hardware Platform: Arduino Uno
LSM9DS1 Breakout Version: 1.0
This code is beerware; if you see me (or any other SparkFun employee) at the
local, and you've found our code helpful, please buy us a round!
Distributed as-is; no warranty is given.
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor.Temperature
{
	[Exportable(Includes = @"#include ""IWire.h""
#include ""IWireDevice.h""
#include ""ISensor.h""
#include ""ITemperatureSensor.h""
")]
	public class TMP102 : ITemperatureSensor
	{
		public const byte TMP102_TEMPERATURE_REGISTER = 0x00;
		public const byte TMP102_CONFIG_REGISTER = 0x01;
		public const byte TMP102_T_LOW_REGISTER = 0x02;
		public const byte TMP102_T_HIGH_REGISTER = 0x03;
		private IWireDevice device;

		public TMP102(IWireDevice device)
		{
			this.device = device;
		}

		/// <summary>
		/// Gets the temperature Celsius.
		/// </summary>
		/// <returns></returns>
		public double GetTemperatureCelsius()
		{
			var registerByte = new byte[2];    // Store the data from the register here
			int digitalTemp;  // Temperature stored in TMP102 register

			// Read Temperature
			// Change pointer address to temperature register (0)
			openPointerRegister(TMP102_TEMPERATURE_REGISTER);
			// Read from temperature register
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);

			// Bit 0 of second byte will always be 0 in 12-bit readings and 1 in 13-bit
			if ((registerByte[1] & 0x01) > 0) // 13 bit mode
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
			else    // 12 bit mode
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
			return digitalTemp * 0.0625;
		}

		/// <summary>
		/// Switch the sensor to low power mode
		/// </summary>
		public void Sleep()
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (0x01)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = readRegister(0);

			registerByte |= 0x01;   // Set SD (bit 0 of first byte)

			// Set configuration register
			device.Write(TMP102_CONFIG_REGISTER,
				registerByte);
		}

		/// <summary>
		/// Wakeup the sensor and enter normal running mode
		/// </summary>
		public void Wakeup()
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = readRegister(0);

			registerByte &= 0xFE;   // Clear SD (bit 0 of first byte)

			// Set configuration registers
			device.Write(TMP102_CONFIG_REGISTER,
				registerByte);
		}

		/// <summary>
		/// Sets the low temperature Celsius.
		/// </summary>
		/// <param name="temp">The temporary any decimal will be ignored</param>
		public void SetLowTemperatureCelsius(double temperature)
		{
			var registerByte = new int[2];    // Store the data from the register here
			bool extendedMode;  // Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C

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
			openPointerRegister(TMP102_CONFIG_REGISTER);   // Read configuration register settings

			// Read current configuration register value
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;   // 0 - temp data will be 12 bits
																  // 1 - temp data will be 13 bits

			// Convert analog temperature to digital value
			var l = (long)(temperature / 0.0625);

			// Split temperature into separate bytes
			if (extendedMode)   // 13-bit mode
			{
				registerByte[0] = (int)(l >> 5);
				registerByte[1] = (int)(l << 3);
			}
			else    // 12-bit mode
			{
				registerByte[0] = (int)(l >> 4);
				registerByte[1] = (int)(l << 4);
			}

			// Write to T_HIGH Register
			device.Write(
				TMP102_T_LOW_REGISTER,
				(byte)registerByte[0],
				(byte)registerByte[1]
			);
		}

		/// <summary>
		/// Sets the high temperature Celsius.
		/// </summary>
		/// <param name="temp">The temporary.</param>
		public void SetHighTemperatureCelsius(double temperature)
		{
			var registerByte = new int[2];    // Store the data from the register here
			bool extendedMode;  // Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C

			// Prevent temperature from exceeding 150C or -55C
			if (temperature > 150f)
			{
				temperature = 150f;
			}
			if (temperature < -55f)
			{
				temperature = -55f;
			}

			//Check if temperature should be 12 or 13 bits
			openPointerRegister(TMP102_CONFIG_REGISTER);   // Read configuration register settings

			// Read current configuration register value
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;   // 0 - temp data will be 12 bits
																  // 1 - temp data will be 13 bits

			// Convert analog temperature to digital value
			var l = (long)(temperature / 0.0625);

			// Split temperature into separate bytes
			if (extendedMode)   // 13-bit mode
			{
				registerByte[0] = (int)(l >> 5);
				registerByte[1] = (int)(l << 3);
			}
			else    // 12-bit mode
			{
				registerByte[0] = (int)(l >> 4);
				registerByte[1] = (int)(l << 4);
			}

			// Write to T_LOW Register
			device.Write(
				TMP102_T_LOW_REGISTER,
				(byte)registerByte[0],
				(byte)registerByte[1]
			);
		}

		/// <summary>
		/// Reads the low temperature Celsius.
		/// </summary>
		/// <returns></returns>
		public double ReadLowTemperatureCelsius()
		{
			var registerByte = new int[2];    // Store the data from the register here
			bool extendedMode;  // Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C
			int digitalTemp;        // Store the digital temperature value here

			// Check if temperature should be 12 or 13 bits
			openPointerRegister(TMP102_CONFIG_REGISTER);   // Read configuration register settings
														   // Read current configuration register value
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;   // 0 - temp data will be 12 bits
																  // 1 - temp data will be 13 bits
			openPointerRegister(TMP102_T_LOW_REGISTER);
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);

			if (extendedMode)   // 13 bit mode
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
			else    // 12 bit mode
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
			return digitalTemp * 0.0625;
		}

		public double ReadHighTemperatureCelsius()
		{
			var registerByte = new int[2];    // Store the data from the register here
			bool extendedMode;  // Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C
			int digitalTemp;        // Store the digital temperature value here

			// Check if temperature should be 12 or 13 bits
			openPointerRegister(TMP102_CONFIG_REGISTER);   // read configuration register settings
														   // Read current configuration register value
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);
			extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;   // 0 - temp data will be 12 bits
																  // 1 - temp data will be 13 bits
			openPointerRegister(TMP102_T_HIGH_REGISTER);
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);

			if (extendedMode)   // 13 bit mode
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
			else    // 12 bit mode
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
			return digitalTemp * 0.0625;
		}

		/// <summary>
		/// Sets the conversion rate.
		/// Set the conversion rate (0-3)
		/// 0 - 0.25 Hz
		/// 1 - 1 Hz
		/// 2 - 4 Hz (default)
		/// 3 - 8 Hz
		/// </summary>
		/// <param name="rate">The rate.</param>
		public void SetConversionRate(byte rate)
		{

			var registerByte = new int[2]; // Store the data from the register here
			var i = rate & 0x03; // Make sure rate is not set higher than 3.

			// Change pointer address to configuration register (0x01)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);

			// Load new conversion rate
			registerByte[1] &= 0x3F;  // Clear CR0/1 (bit 6 and 7 of second byte)
			registerByte[1] |= i << 6;   // Shift in new conversion rate

			// Set configuration registers
			device.Write(TMP102_CONFIG_REGISTER,
				(byte)registerByte[0],
				(byte)registerByte[1]
			);
		}

		/// <summary>
		/// Sets the extended mode.
		/// Enable or disable extended mode
		/// 0 - disabled (-55C to +128C)
		/// 1 - enabled  (-55C to +150C)
		/// </summary>
		/// <param name="mode">if set to <c>true</c> [mode].</param>
		public void SetExtendedMode(bool mode)
		{
			var registerByte = new int[2]; // Store the data from the register here

			// Change pointer address to configuration register (0x01)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte[0] = readRegister(0);
			registerByte[1] = readRegister(1);

			// Load new value for extention mode
			registerByte[1] &= 0xEF;        // Clear EM (bit 4 of second byte)
			registerByte[1] |= (mode) ? 1 : 0 << 4;   // Shift in new exentended mode bit

			// Set configuration registers
			device.Write(
				TMP102_CONFIG_REGISTER,
				(byte)registerByte[0],
				(byte)registerByte[1]
			);
		}

		/// <summary>
		/// Sets the alert polarity.
		/// Set the polarity of Alert
		/// 0 - Active LOW
		/// 1 - Active HIGH
		/// </summary>
		/// <param name="polarity">if set to <c>true</c> [polarity].</param>
		public void SetAlertPolarity(bool polarity)
		{
			int registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = readRegister(0);

			// Load new value for polarity
			registerByte &= 0xFB; // Clear POL (bit 2 of registerByte)
			registerByte |= (polarity) ? 1 : 0 << 2;  // Shift in new POL bit

			// Set configuration register
			device.Write(TMP102_CONFIG_REGISTER,
				(byte)registerByte);
		}

		/// <summary>
		/// Sets the fault.
		/// Set the number of consecutive faults
		/// 0 - 1 fault
		/// 1 - 2 faults
		/// 2 - 4 faults
		/// 3 - 6 faults
		/// </summary>
		/// <param name="faultSetting">The fault setting.</param>
		public void SetFault(byte faultSetting)
		{
			int registerByte; // Store the data from the register here

			var f = faultSetting & 3; // Make sure rate is not set higher than 3.

			// Change pointer address to configuration register (0x01)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = readRegister(0);

			// Load new conversion rate
			registerByte &= 0xE7;  // Clear F0/1 (bit 3 and 4 of first byte)
			registerByte |= f << 3;  // Shift new fault setting

			// Set configuration registers
			device.Write(
				TMP102_CONFIG_REGISTER,
				(byte)registerByte
			);
		}

		/// <summary>
		/// Sets the alert mode.
		/// Set Alert type
		/// 0 - Comparator Mode: Active from temp > T_HIGH until temp < T_LOW
		/// 1 - Thermostat Mode: Active when temp > T_HIGH until any read operation occurs
		/// </summary>
		/// <param name="mode">if set to <c>true</c> [mode].</param>
		public void SetAlertMode(bool mode)
		{
			int registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = readRegister(0);

			// Load new conversion rate
			registerByte &= 0xFD;   // Clear old TM bit (bit 1 of first byte)
			registerByte |= (mode) ? 1 : 0 << 1;  // Shift in new TM bit

			// Set configuration registers
			device.Write(
				TMP102_CONFIG_REGISTER,
				(byte)registerByte
			);
		}

		byte Alert()
		{
			byte registerByte; // Store the data from the register here

			// Change pointer address to configuration register (1)
			openPointerRegister(TMP102_CONFIG_REGISTER);

			// Read current configuration register value
			registerByte = readRegister(1);

			registerByte &= 0x20;   // Clear everything but the alert bit (bit 5)
			return (byte)(registerByte >> 5);
		}

		/// <summary>
		/// Opens the pointer register.
		/// Changes the pointer register
		/// </summary>
		/// <param name="pointerReg">The pointer reg.</param>
		private void openPointerRegister(byte pointerReg)
		{
			device.Write(pointerReg);
		}

		/// <summary>
		/// Reads the register.
		/// reads 1 byte of from register
		/// </summary>
		/// <param name="registerNumber">if set to <c>true</c> [register number].</param>
		/// <returns></returns>
		private byte readRegister(byte registerNumber)
		{
			var registerByte = new byte[2];    // We'll store the data from the registers here
			device.Read(ref registerByte, 2);

			return registerByte[registerNumber];
		}

		public void Begin()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			device?.Dispose();
		}
	}
}
