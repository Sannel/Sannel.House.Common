#include "stdafx.h"
#include "CppUnitTest.h"
#include "../Sannel/src/SensorPacket.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace Sannel::House::Sensor;

namespace SannelTests
{		
	TEST_CLASS(SensorPacketTests)
	{
	public:
		
		TEST_METHOD(ResetSensorPacketTest)
		{
			SensorPacket p;
			p.SensorType = SensorTypes::RainAmount;
			for (int i = 0; i < 10; i++) 
			{
				p.Values[i] = 2;
			}

			ResetSensorPacket(p);

			Assert::AreEqual(int(SensorTypes::Test), int(p.SensorType));

			unsigned char* data = (unsigned char*)p.Values;

			for (int i = 0; i < sizeof(data); i++) 
			{
				Assert::AreEqual(unsigned char(255), data[i]);
			}
		}

	};
}