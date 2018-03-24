/* Copyright 2018 Sannel Software, L.L.C.

Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/
/****************************************************************************
 * based on the library from Spark Fun https://github.com/sparkfun/APDS-9301_Breakout/tree/master/Libraries/Arduino/src
 * <filename>
 * <Title>
 * Mike Hord @ SparkFun Electronics
 * 13 Apr 2017
 * <github repository address>
 *
 * <multiline verbose description of file functionality>
 *
 * Resources:
 * <additional library requirements>
 *
 * Development environment specifics:
 * <arduino/development environment version>
 * <hardware version>
 * <etc>
 *
 * This code is beerware; if you see me (or any other SparkFun employee) at the
 * local, and you've found our code helpful, please buy us a round!
 * ****************************************************************************/

#include "APDS9301.h"
#include "IWireDevice.h"

using namespace Sannel::House::Sensor;
using namespace Sannel::House::Sensor::Light;

APDS9301::APDS9301(IWireDevice& device)
{
	this->device = &device;
}

void APDS9301::Begin()
{
	this->PowerEnable(true);
}

// powerEnable() enables/disables the power of the sensor. Normally
//  this will be handled by the .begin() function, but it's possible
//  a user may want to power the sensor off or on to save power
//  outside of the normal begin()/end() methods.
bool APDS9301::PowerEnable(bool powerOn)
{
	if (powerOn)
	{
		return setRegister(APDS9301_CONTROL_REG, 3);
	}
	else
	{
		return setRegister(APDS9301_CONTROL_REG, 0);
	}
}

/// <summary>
/// set gain and integration time functions. By default the gain is
/// set to low and integration time to its lowest setting; this is
/// most suitable to high brightness environments. For lower
/// brightness areas these probably ought to be turned up.
/// </summary>
/// <param name="high">if set to <c>true</c> [high]. if set to <c>false</c> [low]</param>
/// <returns></returns>
bool APDS9301::SetGain(bool high)
{
	uint8_t regVal = getRegister(APDS9301_TIMING_REG);
	if (high)
	{
		regVal |= 0x10;
	}
	else
	{
		regVal = regVal & ~0x10;
	}
	return setRegister(APDS9301_TIMING_REG, regVal);
}

/// <summary>
/// Sets the integration time.
/// </summary>
/// <param name="integrationTime">The integration time.</param>
/// <returns></returns>
bool APDS9301::SetIntegrationTime(APDS9301_IntegrationTime integrationTime)
{
	uint8_t regVal = getRegister(APDS9301_TIMING_REG);
	regVal = regVal & ~0x03;
	if (integrationTime == APDS9301_IntegrationTime::_13_7_MS)
	{
		regVal |= 0x00;
	}
	else if (integrationTime == APDS9301_IntegrationTime::_101_MS)
	{
		regVal |= 0x01;
	}
	else if (integrationTime == APDS9301_IntegrationTime::_402_MS)
	{
		regVal |= 0x02;
	}

	return setRegister(APDS9301_TIMING_REG, regVal);
}

// Enable or disable interrupt.
/// <summary>
/// Enables the interrupt.
/// </summary>
/// <param name="interruptMode">if set to <c>true</c> enable interrupt. if set to <c>false</c> disable interrupt</param>
/// <returns></returns>
bool APDS9301::EnableInterrupt(bool interruptMode)
{
	uint8_t regVal = getRegister(APDS9301_INTERRUPT_REG);
	// This is not a typo- OFF requires bits 4&5 to be cleared, but
	//  ON only requires bit 4 to be set. I dunno why.
	if (interruptMode)
	{
		regVal |= 0x10;
	}
	else
	{
		regVal = regVal & ~0x30;
	}

	return setRegister(APDS9301_INTERRUPT_REG, regVal);
}

// Clear the interrupt flag.
/// <summary>
/// Clears the interrupt flag
/// </summary>
/// <returns></returns>
bool APDS9301::ClearInterruptFlag()
{
	device->Write(0xC0);
	return true;
}

/// <summary>
/// Sets the cycles for interrupt.
/// Number of cycles outside of threshold range before interrupt is
///  asserted. 0 generates an interrupt on every ADC cycle, 1 will
///  generate an interrupt if the value of channel 0 (Vis + IR level)
///  is outside of the threshold region at all, 2-15 require that many
///  cycles out of the threshold region to trigger.
/// </summary>
/// <param name="cycles">The cycles.</param>
/// <returns></returns>
bool APDS9301::SetCyclesForInterrupt(uint8_t cycles)
{
	uint8_t regVal = getRegister(APDS9301_INTERRUPT_REG);
	regVal = regVal & ~0x0F; // clear lower four bits of register
	cycles = cycles & ~0xF0; // ensure top four bits of data are clear
	regVal |= cycles; // Sets any necessary bits in regVal.
	return setRegister(APDS9301_INTERRUPT_REG, regVal);
}

/// <summary>
/// Sets the low threshold.
/// Thresholds for interrupt. Below low or above high will cause an
/// interrupt, depending on the interrupt cycles set by the prior
/// function.
/// </summary>
/// <param name="threshold">The threshold.</param>
/// <returns></returns>
bool APDS9301::SetLowThreshold(uint16_t threshold)
{
	return setTwoRegisters(APDS9301_THRESHLOWLOW_REG, threshold);
}

bool APDS9301::SetHighThreshold(uint16_t threshold)
{
	return setTwoRegisters(APDS9301_THRESHHILOW_REG, threshold);
}

/// <summary>
/// Gets the identifier reg.
/// getIDReg() returns the ID register value. Register will read
/// b0101xxxx where the lower four bits change with the silicon
/// revision of the chip.
/// </summary>
/// <returns></returns>
uint8_t APDS9301::GetIDReg()
{
	return getRegister(APDS9301_ID_REG);
}

// see above for more info about what these get functions do.
/// <summary>
/// Gets the gain.
/// Returns true if its High returns false if its low
/// </summary>
/// <returns></returns>
bool APDS9301::GetGain()
{
	uint8_t regVal = getRegister(APDS9301_TIMING_REG);
	regVal &= 0x10;
	return regVal != 0;
}
/// <summary>
/// Gets the integration time.
/// </summary>
/// <returns></returns>
APDS9301_IntegrationTime APDS9301::GetIntegrationTime()
{
	uint8_t regVal = getRegister(APDS9301_TIMING_REG);
	regVal &= 0x03;
	if (regVal == 0x00)
	{
		return APDS9301_IntegrationTime::_13_7_MS;
	}
	else if (regVal == 0x01)
	{
		return APDS9301_IntegrationTime::_101_MS;
	}
	else
	{
		return APDS9301_IntegrationTime::_402_MS;
	}
}

/// <summary>
/// Gets the cycles for interrupt.
/// </summary>
/// <returns></returns>
uint8_t APDS9301::GetCyclesForInterrupt()
{
	uint8_t regVal = getRegister(APDS9301_INTERRUPT_REG);
	regVal = regVal & ~0x0F;
	return regVal;
}
/// <summary>
/// Gets the low threshold.
/// </summary>
/// <returns></returns>
uint16_t APDS9301::GetLowThreshold()
{
	uint16_t retVal = getRegister(APDS9301_THRESHLOWHI_REG) << 8;
	retVal |= getRegister(APDS9301_THRESHLOWLOW_REG);
	return retVal;
}


/// <summary>
/// Gets the high threshold.
/// </summary>
/// <returns></returns>
uint16_t APDS9301::GetHighThreshold()
{
	uint16_t retVal = getRegister(APDS9301_THRESHHIHI_REG) << 8;
	retVal |= getRegister(APDS9301_THRESHHILOW_REG);
	return retVal;
}

/// <summary>
/// Sensor read functions. These are the actual values the sensor
/// read at last conversion. Most often, this is going to be
/// ignored in favor of the calculated Lux level.
/// </summary>
/// <returns></returns>
uint16_t APDS9301::ReadCH0Level()
{
	return getTwoRegisters(APDS9301_DATA0LOW_REG);
}

/// <summary>
/// Reads the c h1 level.
/// </summary>
/// <returns></returns>
uint16_t APDS9301::ReadCH1Level()
{
	return getTwoRegisters(APDS9301_DATA1LOW_REG);
}

/// <summary>
/// Calculated Lux level. Accurate to within +/- 40%.
/// </summary>
/// <returns></returns>
double APDS9301::GetLuxLevel()
{
	uint16_t ch1Int = ReadCH1Level();
	uint16_t ch0Int = ReadCH0Level();
	double ch0 = ReadCH0Level();
	double ch1 = ReadCH1Level();
	switch (GetIntegrationTime())
	{
	case APDS9301_IntegrationTime::_13_7_MS:
		if ((ch1Int >= 5047) || (ch0Int >= 5047))
		{
			return 1.0 / 0.0;
		}
		break;

	case APDS9301_IntegrationTime::_101_MS:
		if ((ch1Int >= 37177) || (ch0Int >= 37177))
		{
			return 1.0 / 0.0;
		}
		break;

	case APDS9301_IntegrationTime::_402_MS:
		if ((ch1Int >= 65535) || (ch0Int >= 65535))
		{
			return 1.0 / 0.0;
		}
		break;
	}
	double ratio = ch1 / ch0;
	switch (GetIntegrationTime())
	{
	case APDS9301_IntegrationTime::_13_7_MS:
		ch0 *= 1 / 0.034;
		ch1 *= 1 / 0.034;
		break;

	case APDS9301_IntegrationTime::_101_MS:
		ch0 *= 1 / 0.252;
		ch1 *= 1 / 0.252;
		break;

	case APDS9301_IntegrationTime::_402_MS:
		ch0 *= 1;
		ch1 *= 1;
		break;
	}

	if (GetGain() == false)// low
	{
		ch0 *= 16;
		ch1 *= 16;
	}

	double luxVal = 0.0;

	if (ratio <= 0.5)
	{
		luxVal = (0.0304 * ch0) - ((0.062 * ch0) * (pow((ch1 / ch0), 1.4)));
	}
	else if (ratio <= 0.61)
	{
		luxVal = (0.0224 * ch0) - (0.031 * ch1);
	}
	else if (ratio <= 0.8)
	{
		luxVal = (0.0128 * ch0) - (0.0153 * ch1);
	}
	else if (ratio <= 1.3)
	{
		luxVal = (0.00146 * ch0) - (0.00112 * ch1);
	}

	return luxVal;
}

uint8_t APDS9301::getRegister(uint8_t regAddress)
{
	return device->WriteRead(regAddress);
}

/// <summary>
/// Sets the register.
/// </summary>
/// <param name="regAddress">The reg address.</param>
/// <param name="newVal">The new value.</param>
/// <returns></returns>
bool APDS9301::setRegister(uint8_t regAddress, uint8_t newVal)
{
	device->Write(regAddress, newVal);
	return true;
}

/// <summary>
/// Gets the two registers.
/// </summary>
/// <param name="regAddress">The reg address.</param>
/// <returns></returns>
uint16_t APDS9301::getTwoRegisters(uint8_t regAddress)
{
	uint8_t data[2];
	device->WriteRead(0x20 | regAddress, data, 2);
	return data[0] | (data[1] << 8);
}

/// <summary>
/// Sets the two registers.
/// </summary>
/// <param name="regAddress">The reg address.</param>
/// <param name="newVal">The new value.</param>
/// <returns></returns>
bool APDS9301::setTwoRegisters(uint8_t regAddress, uint16_t newVal)
{
	device->Write(0x20 | regAddress, newVal, (newVal >> 8));
	return true;
}
