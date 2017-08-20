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

			Assert::AreEqual((unsigned char)1, store.GetSize());

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
			Assert::AreEqual((unsigned char)1, store.GetStoredPackets());

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
			Assert::AreEqual((unsigned char)2, store.GetStoredPackets());

			actual = store.GetPacket(1);

			Assert::AreEqual((int)packet2.SensorType, (int)actual.SensorType);
			Assert::AreEqual(packet2.Offset, actual.Offset);
			Assert::AreEqual(packet2.Values[0], actual.Values[0]);
		}

		TEST_METHOD(WritePackets)
		{
			// 😊 Test
			SensorStore store(2);

			unsigned char mac[6];
			mac[0] = 1;
			mac[1] = 2;
			mac[2] = 3;
			mac[3] = 4;
			mac[4] = 5;
			mac[5] = 6;
			store.SetMacAddress(mac);

			SensorPacket packet1;
			SensorPacket packet2;
			ResetSensorPacket(packet1);
			ResetSensorPacket(packet2);
			packet1.SensorType = SensorTypes::RainAmount;
			packet1.Offset = 2000;
			packet1.Values[0] = 20;
			packet2.SensorType = SensorTypes::Temperature;
			packet2.Offset = 4567;
			packet2.Values[1] = 10;
			packet2.Values[2] = 33;
			packet2.Values[3] = 43;
			packet2.Values[4] = 53;


			store.AddReading(packet1);

			Stream s;
			store.WritePackets(s);

			std::vector<unsigned char> data = s.GetData();
			
			Assert::AreEqual(size_t(95), data.size());
			Assert::AreEqual((unsigned char)1, data.at(0));
			Assert::AreEqual(mac[0], data.at(1));
			Assert::AreEqual(mac[1], data.at(2));
			Assert::AreEqual(mac[2], data.at(3));
			Assert::AreEqual(mac[3], data.at(4));
			Assert::AreEqual(mac[4], data.at(5));
			Assert::AreEqual(mac[5], data.at(6));

			CheckPacket(data, 7, packet1);

			s.Clear();

			store.AddReading(packet2);

			store.WritePackets(s);

			data = s.GetData();
			
			Assert::AreEqual(size_t(183), data.size());
			Assert::AreEqual((unsigned char)2, data.at(0));
			Assert::AreEqual(mac[0], data.at(1));
			Assert::AreEqual(mac[1], data.at(2));
			Assert::AreEqual(mac[2], data.at(3));
			Assert::AreEqual(mac[3], data.at(4));
			Assert::AreEqual(mac[4], data.at(5));
			Assert::AreEqual(mac[5], data.at(6));

			CheckPacket(data, 7, packet1);
			CheckPacket(data, 95, packet2);
		}

		TEST_METHOD(Reset)
		{
			SensorStore store(50);

			SensorPacket packet1;
			ResetSensorPacket(packet1);
			packet1.SensorType = SensorTypes::RainAmount;
			packet1.Offset = 2000;
			packet1.Values[0] = 20;

			store.AddReading(packet1);
			store.AddReading(packet1);
			store.AddReading(packet1);
			store.AddReading(packet1);

			Assert::AreEqual((unsigned char)4, store.GetStoredPackets());

			store.Reset();

			Assert::AreEqual((unsigned char)0, store.GetStoredPackets());
		}

	private:
		void CheckPacket(std::vector<unsigned char> &data, int startIndex, SensorPacket &packet)
		{
			unsigned char *q = (unsigned char *)&packet.SensorType;

			Assert::AreEqual(q[0], data.at(startIndex));
			Assert::AreEqual(q[1], data.at(startIndex + 1));
			Assert::AreEqual(q[2], data.at(startIndex + 2));
			Assert::AreEqual(q[3], data.at(startIndex + 3));

			q = (unsigned char *)&packet.Offset;

			Assert::AreEqual(q[0], data.at(startIndex + 4));
			Assert::AreEqual(q[1], data.at(startIndex + 5));
			Assert::AreEqual(q[2], data.at(startIndex + 6));
			Assert::AreEqual(q[3], data.at(startIndex + 7));

			for (int i = 0; i < 10; i++) 
			{
				q = (unsigned char *)&packet.Values[i];
				Assert::AreEqual(q[0], data.at((startIndex + 8 + (i * 8))));
				Assert::AreEqual(q[1], data.at((startIndex + 9 + (i * 8))));
				Assert::AreEqual(q[2], data.at((startIndex + 10 + (i * 8))));
				Assert::AreEqual(q[3], data.at((startIndex + 11 + (i * 8))));
				Assert::AreEqual(q[4], data.at((startIndex + 12 + (i * 8))));
				Assert::AreEqual(q[5], data.at((startIndex + 13 + (i * 8))));
				Assert::AreEqual(q[6], data.at((startIndex + 14 + (i * 8))));
				Assert::AreEqual(q[7], data.at((startIndex + 15 + (i * 8))));
			}
		}
	};
}