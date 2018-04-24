# Sannel.House.Common
Used as a sub module for classes or other types that should be shared between any layers of Sannel.House

# Port Configurations

### Development
|Application|Port|
|--|--|
|Server Port|5000|
|Sensor Broadcast|8175|

### Production
|Application|Port|
|--|--|
|Server Port|6000|
|Sensor Broadcast|8257|

# Package Description
* First Byte == number of entries max of 255
* Next 6 bytes == mac address
* for each entry
	* first 4 bytes are the type of sensor the first <= 255 are reserved for types define by SensorTypes
	* Next byte == number of properties
	* for each property
		* next 4 bytes == the abbreviation of the property (i.e. temp)
		* next 4 bytes == the float value


