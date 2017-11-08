using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor.Temperature
{
	public class TMP102 : ITemperatureSensor
	{
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sleeps the device to low power mode
		/// </summary>
		public void Sleep()
		{

		}

		/// <summary>
		/// Wakeups the device and start running in normal power mode
		/// </summary>
		public void Wakeup()
		{

		}

		/// <summary>
		/// Returns the state of the Alert register
		/// </summary>
		/// <returns></returns>
		public bool Alert()
		{
			throw new NotImplementedException();
		}

		/*GetPressure
				class TMP102 : public ITemperatureSensor
				{
				public:
					void setLowTempC(float temperature);  // Sets T_LOW (degrees C) alert threshold
					void setHighTempC(float temperature); // Sets T_HIGH (degrees C) alert threshold
					void setLowTempF(float temperature);  // Sets T_LOW (degrees F) alert threshold
					void setHighTempF(float temperature); // Sets T_HIGH (degrees F) alert threshold
					float readLowTempC(void);	// Reads T_LOW register in C
					float readHighTempC(void);	// Reads T_HIGH register in C
					float readLowTempF(void);	// Reads T_LOW register in F
					float readHighTempF(void);	// Reads T_HIGH register in F		

												// Set the conversion rate (0-3)
												// 0 - 0.25 Hz
												// 1 - 1 Hz
												// 2 - 4 Hz (default)
												// 3 - 8 Hz
					void setConversionRate(byte rate);

					// Enable or disable extended mode
					// 0 - disabled (-55C to +128C)
					// 1 - enabled  (-55C to +150C)
					void setExtendedMode(bool mode);

					// Set the polarity of Alert
					// 0 - Active LOW
					// 1 - Active HIGH
					void setAlertPolarity(bool polarity);

					// Set the number of consecutive faults
					// 0 - 1 fault
					// 1 - 2 faults
					// 2 - 4 faults
					// 3 - 6 faults
					void setFault(byte faultSetting);

					// Set Alert type
					// 0 - Comparator Mode: Active from temp > T_HIGH until temp < T_LOW
					// 1 - Thermostat Mode: Active when temp > T_HIGH until any read operation occurs
					void setAlertMode(bool mode);

					void Begin() override;
					double GetTemperatureCelsius() override;

				private:
					int _address; // Address of Temperature sensor (0x48,0x49,0x4A,0x4B)
					void openPointerRegister(byte pointerReg); // Changes the pointer register
					byte readRegister(bool registerNumber);	// reads 1 byte of from register
					*/
		public void Begin()
		{
		}

		public void Dispose()
		{
		}

	}
}
