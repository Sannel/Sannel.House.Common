#include "stdafx.h"
#include "CppUnitTest.h"
#include "../Sannel/src/SensorStore.h"
#include "../Sannel/src/MAddress.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace Sannel::House::Sensor;

namespace SannelTests
{		
	TEST_CLASS(SensorStoreTests)
	{
	public:
		
		TEST_METHOD(CreateTest)
		{
			SensorStore store(1);

			Assert::AreEqual(1, store.GetSize());

			MAddress macAddress = store.GetMacAddress();

			Assert::AreEqual((unsigned char)0, macAddress.Byte1);
			Assert::AreEqual((unsigned char)0, macAddress.Byte2);
			Assert::AreEqual((unsigned char)0, macAddress.Byte3);
			Assert::AreEqual((unsigned char)0, macAddress.Byte4);
			Assert::AreEqual((unsigned char)0, macAddress.Byte5);
			Assert::AreEqual((unsigned char)0, macAddress.Byte6);

		}

		TEST_METHOD(SetMacAddress) 
		{
			SensorStore store(1);

			unsigned char mac[6];

			mac[0] = 2;
			mac[1] = 3;
			mac[2] = 4;
			mac[3] = 5;
			mac[4] = 6;
			mac[5] = 7;

			store.SetMacAddress(mac);

			MAddress macAddress = store.GetMacAddress();

			Assert::AreEqual(mac[0], macAddress.Byte1);
			Assert::AreEqual(mac[1], macAddress.Byte2);
			Assert::AreEqual(mac[2], macAddress.Byte3);
			Assert::AreEqual(mac[3], macAddress.Byte4);
			Assert::AreEqual(mac[4], macAddress.Byte5);
			Assert::AreEqual(mac[5], macAddress.Byte6);
		}

		TEST_METHOD(AddReading)
		{
			SensorStore store(2);

			SensorPacket packet1;
			ResetSensorPacket(packet1);
			packet1.SensorType = SensorTypes::WindDirection;
			packet1.Values[0] = 1;

			bool rvalue = store.AddReading(packet1);

			Assert::IsFalse(rvalue);
			Assert::AreEqual(1, store.GetStoredPackets());

			SensorPacket actual = store.GetPacket(0);

			Assert::AreEqual((int)packet1.SensorType, (int)actual.SensorType);
			Assert::AreEqual(packet1.Offset, actual.Offset);
			Assert::AreEqual(packet1.Values[0], actual.Values[0]);

			SensorPacket packet2;
			ResetSensorPacket(packet2);
			packet2.SensorType = SensorTypes::RainAmount;
			packet2.Offset = 1;
			packet2.Values[0] = 2333.003;

			rvalue = store.AddReading(packet2);

			Assert::IsTrue(rvalue);
			Assert::AreEqual(2, store.GetStoredPackets());

			actual = store.GetPacket(1);

			Assert::AreEqual((int)packet2.SensorType, (int)actual.SensorType);
			Assert::AreEqual(packet2.Offset, actual.Offset);
			Assert::AreEqual(packet2.Values[0], actual.Values[0]);
		}

	};
}