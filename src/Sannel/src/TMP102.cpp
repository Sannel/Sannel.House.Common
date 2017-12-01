
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
#include "TMP102.h"
#include "IWire.h"
#include "IWireDevice.h"

using namespace Sannel::House::Sensor::Temperature;



TMP102::TMP102(IWire& wire, uint8_t& address)
{
	device = &wire.GetDeviceById(address);
}

TMP102::TMP102(IWireDevice& d) 
{
	device = &d;
}

void TMP102::Begin()
{

}

/*void TMP102::Begin(void)
{
	Wire.begin();  // Join I2C bus
}*/


/*void TMP102::openPointerRegister(byte pointerReg)
{
	Wire.beginTransmission(_address); // Connect to TMP102
	Wire.write(pointerReg); // Open specified register
	Wire.endTransmission(); // Close communication with TMP102
}


byte TMP102::readRegister(bool registerNumber) {
	int registerByte[2];	// We'll store the data from the registers here

							// Read current configuration register value
	Wire.requestFrom(_address, 2); 	// Read two bytes from TMP102
	Wire.endTransmission();
	registerByte[0] = (Wire.read());	// Read first byte
	registerByte[1] = (Wire.read());	// Read second byte

	return registerByte[registerNumber];
}*/

double TMP102::GetTemperatureCelsius()
{
	int registerByte[2]; // Store the data from the register here
	int digitalTemp; // Temperature stored in TMP102 register

	// Read Temperature
	// Change pointer address to temperature register (0)
	this->device->Write(TMP102_TEMPERATURE_REGISTER);
	// Read from temperature register
	registerByte[0] = this->device->WriteRead(0);
	registerByte[1] = this->device->WriteRead(1);

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

void TMP102::SetConversionRate(uint8_t& rate) 
{
	int registerByte[2]; // Store the data from the register here
	uint8_t r = rate & 0x03; // Make sure rate is not set higher than 3.

	// Change pointer address to configuration register (0x01)
	this->device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte[0] = this->device->WriteRead(0);
	registerByte[1] = this->device->WriteRead(1);

	// Load new conversion rate
	registerByte[1] &= 0x3F;  // Clear CR0/1 (bit 6 and 7 of second byte)
	registerByte[1] |= r << 6;   // Shift in new conversion rate

	// Set configuration registers
	this->device->Write(TMP102_CONFIG_REGISTER,
				registerByte[0],
				registerByte[1]);
}

void TMP102::SetExtendedMode(bool& mode)
{
	int registerByte[2]; // Store the data from the register here

	// Change pointer address to configuration register (0x01)
	this->device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte[0] = this->device->WriteRead(0);
	registerByte[1] = this->device->WriteRead(1);

	int m = mode ? 1 : 0;
	// Load new value for extention mode
	registerByte[1] &= 0xEF;		// Clear EM (bit 4 of second byte)
	registerByte[1] |= m << 4;	// Shift in new exentended mode bit

	// Set configuration registers
	this->device->Write(TMP102_CONFIG_REGISTER, // Point to configuration register
				registerByte[0],	// Write first byte
				registerByte[1]); 	// Write second byte
}

void TMP102::Sleep()
{
	uint8_t registerByte; // Store the data from the register here

	// Change pointer address to configuration register (0x01)
	this->device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte = device->WriteRead(0);

	registerByte |= 0x01;   // Set SD (bit 0 of first byte)

	// Set configuration register
	device->Write(TMP102_CONFIG_REGISTER, // Point to configuration register
		registerByte); // write first byte
}

void TMP102::Wakeup()
{
	uint8_t registerByte; // Store the data from the register here

	// Change pointer address to configuration register (1)
	device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte = device->WriteRead(0);

	registerByte &= 0xFE;	// Clear SD (bit 0 of first byte)

	// Set configuration registers
	device->Write(TMP102_CONFIG_REGISTER, // Point to configuration register
				registerByte); // Write first byte
}

void TMP102::SetAlertPolarity(bool& polarity)
{
	uint8_t registerByte; // Store the data from the register here

	// Change pointer address to configuration register (1)
	device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte = device->WriteRead(0);

	// Load new value for polarity
	registerByte &= 0xFB; // Clear POL (bit 2 of registerByte)
	registerByte |= (polarity?1:0) << 2;  // Shift in new POL bit

	// Set configuration register
	device->Write(TMP102_CONFIG_REGISTER,	// Point to configuration register
				registerByte);	    // Write first byte
}

bool TMP102::Alert()
{
	uint8_t registerByte; // Store the data from the register here

	// Change pointer address to configuration register (1)
	device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte = device->WriteRead(1);

	registerByte &= 0x20; // Clear everything but the alert bit (bit 5)
	return (registerByte >> 5) > 0;
}

void TMP102::SetLowTemperatureCelsius(double& temperature)
{
	int registerByte[2];	// Store the data from the register here
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
	device->Write(TMP102_CONFIG_REGISTER);	// Read configuration register settings

											// Read current configuration register value
	registerByte[0] = device->WriteRead(0);
	registerByte[1] = device->WriteRead(1);
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
	device->Write(TMP102_T_LOW_REGISTER, 	// Point to T_LOW
				registerByte[0],  // Write first byte
				registerByte[1]);  // Write second byte
}

void TMP102::SetHighTemperatureCelsius(double& temperature)
{
	int registerByte[2];	// Store the data from the register here
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
	device->Write(TMP102_CONFIG_REGISTER);	// Read configuration register settings

											// Read current configuration register value
	registerByte[0] = device->WriteRead(0);
	registerByte[1] = device->WriteRead(1);
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
	device->Write(TMP102_T_HIGH_REGISTER, 	// Point to T_HIGH register
				registerByte[0],  // Write first byte
				registerByte[1]);  // Write second byte
}

double TMP102::ReadLowTemperatureCelsius()
{
	int registerByte[2];	// Store the data from the register here
	bool extendedMode;	// Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C
	int digitalTemp;		// Store the digital temperature value here

	// Check if temperature should be 12 or 13 bits
	device->Write(TMP102_CONFIG_REGISTER);	// Read configuration register settings
									// Read current configuration register value

	registerByte[0] = device->WriteRead(0);
	registerByte[1] = device->WriteRead(1);
	extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;	// 0 - temp data will be 12 bits
														// 1 - temp data will be 13 bits
	device->Write(TMP102_T_LOW_REGISTER);
	registerByte[0] = device->WriteRead(0);
	registerByte[1] = device->WriteRead(1);

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

double TMP102::ReadHighTemperatureCelsius()
{
	int registerByte[2];	// Store the data from the register here
	bool extendedMode;	// Store extended mode bit here 0:-55C to +128C, 1:-55C to +150C
	int digitalTemp;		// Store the digital temperature value here

	// Check if temperature should be 12 or 13 bits
	device->Write(TMP102_CONFIG_REGISTER);	// read configuration register settings
									// Read current configuration register value
	registerByte[0] = device->WriteRead(0);
	registerByte[1] = device->WriteRead(1);
	extendedMode = ((registerByte[1] & 0x10) >> 4) > 0;	// 0 - temp data will be 12 bits
														// 1 - temp data will be 13 bits
	device->Write(TMP102_T_HIGH_REGISTER);
	registerByte[0] = device->WriteRead(0);
	registerByte[1] = device->WriteRead(1);

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

void TMP102::SetFault(uint8_t& faultSetting)
{
	uint8_t registerByte; // Store the data from the register here

	uint8_t f = faultSetting & 3; // Make sure rate is not set higher than 3.

	// Change pointer address to configuration register (0x01)
	device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte = device->WriteRead(0);

	// Load new conversion rate
	registerByte &= 0xE7;  // Clear F0/1 (bit 3 and 4 of first byte)
	registerByte |= f << 3;// Shift new fault setting

	// Set configuration registers
	device->Write(TMP102_CONFIG_REGISTER, 	// Point to configuration register
				registerByte);     // Write byte to register
}

void TMP102::SetAlertMode(bool& mode)
{
	uint8_t registerByte; // Store the data from the register here

	// Change pointer address to configuration register (1)
	device->Write(TMP102_CONFIG_REGISTER);

	// Read current configuration register value
	registerByte = device->WriteRead(0);

	// Load new conversion rate
	registerByte &= 0xFD;	// Clear old TM bit (bit 1 of first byte)
	registerByte |= mode << 1;	// Shift in new TM bit

	// Set configuration registers
	device->Write(TMP102_CONFIG_REGISTER, 	// Point to configuration register
				registerByte);     // Write byte to register
}
