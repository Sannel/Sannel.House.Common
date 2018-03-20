/*
This library was started from Sparkfun's Si7021 library see below for more info

SparkFun Si7021 Temperature and HUmidity Breakout
By: Joel Bartlett
SparkFun Electronics
Date: December 10, 2015

This is an Arduino library for the Si7021 Temperature and Humidity Sensor Breakout

This library is based on the following libraries:
HTU21D Temperature / Humidity Sensor Library
By: Nathan Seidle
https://github.com/sparkfun/HTU21D_Breakout/tree/master/Libraries
Arduino Si7010 relative humidity + temperature sensor
By: Jakub Kaminski, 2014
https://github.com/teoqba/ADDRESS
This Library is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This Library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
For a copy of the GNU General Public License, see
<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Sensor.Temperature
{
	[Exportable(Includes = @"#include ""IWire.h""
#include ""IWireDevice.h""
#include ""ISensor.h""
#include ""ITemperatureSensor.h""
#include ""IHumiditySensor.h""
#include ""Si7021Resolutions.h""")]
	public class Si7021 : ISensor, ITemperatureSensor, IHumiditySensor
	{
		public enum Si7021Resolutions
		{
			_12Bit = 0,
			_8Bit = 1,
			_10Bit = 2,
			_11Bit = 3
		}

		public const byte SI7021_TEMP_MEASURE_HOLD = 0xE3;
		public const byte SI7021_HUMD_MEASURE_HOLD = 0xE5;
		public const byte SI7021_TEMP_MEASURE_NOHOLD = 0xF3;
		public const byte SI7021_HUMD_MEASURE_NOHOLD = 0xF5;
		public const byte SI7021_TEMP_PREV = 0xE0;
		public const byte SI7021_WRITE_USER_REG = 0xE6;
		public const byte SI7021_READ_USER_REG = 0xE7;
		public const byte SI7021_SOFT_RESET = 0xFE;
		public const byte SI7021_HTRE = 0x02;
		public const int SI7021_CRC_POLY = 0x988000; // Shifted Polynomial for CRC check

		// Error codes
		public const short SI7021_I2C_TIMEOUT = 998;
		public const short SI7021_BAD_CRC = 999;


		private IWireDevice device;
		public Si7021(IWireDevice device)
		{
			this.device = device;
		}

		private byte bv(byte bit)
		{
			return (byte)(1 << bit);
		}

		private byte checkID()
		{
			// Check device ID
			device.Write(0xFC, 0xC9);

			var id = device.ReadByte();

			return (id);
		}

		public void Begin()
		{
			var id_Temp_Hum = checkID();

			byte x = 0;

			if (id_Temp_Hum == 0x15)//Ping CheckID register
			{
				x = 1;
			}
			else if (id_Temp_Hum == 0x32)
			{
				x = 2;
			}
			else
			{
				x = 0;
			}

			if (x == 1)
			{
				Debug.WriteLine("Si7021 Found");
			}
			else if (x == 2)
			{
				Debug.WriteLine("HTU21D Found");
			}
			else
			{
				Debug.WriteLine("No Devices Detected");
			}

		}

		public void Dispose()
		{
			device?.Dispose();
		}

		private ushort makeMeasurment(byte command)
		{
			// Take one ADDRESS measurement given by command.
			// It can be either temperature or relative humidity
			// TODO: implement checksum checking

			var nBytes = 3;
			// if we are only reading old temperature, read only msb and lsb
			if (command == 0xE0)
			{
				nBytes = 2;
			}

			device.Write(command);

			// When not using clock stretching (*_NOHOLD commands) delay here
			// is needed to wait for the measurement.
			// According to datasheet the max. conversion time is ~22ms
			Task.Delay(100).Wait();

			var data = new byte[3];

			var read = device.Read(ref data, nBytes);
			if (read != nBytes)
			{
				return 100;
			}

			ushort msb = data[0];
			ushort lsb = data[1];
			// Clear the last to bits of LSB to 00.
			// According to datasheet LSB of RH is always xxxxxx10
			lsb &= 0xFC;
			var mesurment = (ushort)(msb << 8 | lsb);

			return mesurment;
		}

		public double GetRelativeHumidity()
		{
			// Measure the relative humidity
			var RH_Code = makeMeasurment(SI7021_HUMD_MEASURE_NOHOLD);
			var result = (125.0 * RH_Code / 65536) - 6;
			return result;
		}

		public double GetTemperatureCelsius()
		{
			// Measure temperature
			var temp_Code = makeMeasurment(SI7021_TEMP_MEASURE_NOHOLD);
			var result = (175.72*temp_Code / 65536) - 46.85;
			return result;
		}

		private void writeReg(byte value)
		{
			// Write to user register on ADDRESS
			device.Write(SI7021_WRITE_USER_REG, value);
		}

		private byte readReg()
		{
			// Read from user register on ADDRESS
			var regVal = device.WriteRead(SI7021_READ_USER_REG);
			return regVal;
		}

		public void Reset()
		{
			//Reset user resister
			writeReg(SI7021_SOFT_RESET);
		}

		public void ChangeResolution(Si7021Resolutions i)
		{
			// Changes to resolution of ADDRESS measurements.
			// Set i to:
			//      RH         Temp
			// 0: 12 bit       14 bit (default)
			// 1:  8 bit       12 bit
			// 2: 10 bit       13 bit
			// 3: 11 bit       11 bit

			var regVal = readReg();
			// zero resolution bits
			regVal &= 0b011111110;
			switch (i)
			{
				case Si7021Resolutions._8Bit:
					regVal |= 0b00000001;
					break;
				case Si7021Resolutions._10Bit:
					regVal |= 0b10000000;
					break;
				case Si7021Resolutions._11Bit:
					regVal |= 0b10000001;
					break;
				default:
					regVal |= 0b00000000;
					break;
			}
			// write new resolution settings to the register
			writeReg(regVal);
		}


		public void HeaterOn()
		{
			// Turns on the ADDRESS heater
			var regVal = readReg();
			regVal |= bv(SI7021_HTRE);
			//turn on the heater
			writeReg(regVal);
		}

		public void HeaterOff()
		{
			// Turns off the ADDRESS heater
			var regVal = readReg();
			regVal &= (byte)(~bv(SI7021_HTRE));
			writeReg(regVal);
		}

		public double ReadStoredTemp()
		{
			// Read temperature from previous RH measurement.
			var temp_Code = makeMeasurment(SI7021_TEMP_PREV);
			var result = (175.72 * temp_Code / 65536) - 46.85;
			return result;
		}
	}
}
