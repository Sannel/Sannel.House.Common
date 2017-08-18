#include "stdafx.h"
#include "CppUnitTest.h"
#include "../Sannel/src/SensorStore.h"

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

			const unsigned char* macAddress = store.GetMacAddress();

			Assert::AreEqual(unsigned int(6), sizeof(macAddress) / sizeof(macAddress[0]));
		}

	};
}