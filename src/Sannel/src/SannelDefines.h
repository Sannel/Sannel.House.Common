﻿#pragma once

#ifdef DEBUG
#define SERVER_PORT 5000
#define SENSOR_BROADCAST_PORT 8175
#else
#define SERVER_PORT 6000
#define SENSOR_BROADCAST_PORT 8257
#endif