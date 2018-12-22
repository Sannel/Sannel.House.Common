# Sannel House
Sannel House is a home automation framework. It uses docker for services that make since to run on docker. It uses MQTT for a local message bus specifically [eclipse-mosquitto](https://mosquitto.org/). Supports [Microsoft Sql Server](https://www.microsoft.com/en-us/sql-server), [MySql](https://www.mysql.com/), and [Sqlite](https://sqlite.org) for data storage. Some layers also support [MongoDB](https://www.mongodb.com/) for data storage.  

# Sannel.House.Common
Contains different projects that contain extensions and Classes are used in at least two projects/layers

#### Sannel.House.Client
Common classes used for client libraries for micro services.
#### Sannel.House.Data
Extensions used to help with Entity Framework Core projects
#### Sannel.House.Sensor
Interfaces and Base Classes to use for reading and writing to sensors.
#### Sannel.House.Web
Extensions and Classes used to provide standard signatures for api returns. As well as a helper class to install certificates in trusted root authority.

# Web Projects
## [Sannel.House.Gateway](https://github.com/Sannel/Sannel.House.Gateway)
The api gateway for the micro services
## [Sannel.House.Users](https://github.com/Sannel/Sannel.House.Users)
The authentication and user management micro service. Uses [IdentityServer4](https://github.com/IdentityServer/IdentityServer4)
## [Sannel.House.Devices](https://github.com/Sannel/Sannel.House.Devices)
The micro service to manage devices on the system
## [Sannel.House.SensorLogging](https://github.com/Sannel/Sannel.House.SensorLogging)
The micro service responsible for receiving sensor data.
## Sannel.House.Weather
Used to store and retrieve historical weather data.


# Library Projects
## [Sannel.House.ServerSDK](https://github.com/Sannel/Sannel.House.ServerSDK)
Combines all the client libraries from the web project into one project so you have one common library to include to access the system. 

# Service Projects
## Sannel.House.MessageListener
Listens to the MQTT bus and then logs the received data to the correct service. 
## Sannel.House.Weather.Underground
Pull in data from Weather Underground and store for easy access 
## Sannel.House.Weather.Local
Pull in data from SensorLogging thats relevant to the weather and send it to Sannel.House.Weather
## Sannel.House.AI
Pull in sensor data thats stored on the different services and create a heating / cooling plan for the house that can be sent to the thermostat.
## [Sannel.House.BackgroundTasks.Thermostat](https://github.com/Sannel/Sannel.House.BackgroundTasks.Thermostat)
A UWP background task responsible for thermostat duties.
## Sannel.House.Sprinkler
A UWP background task responsible for controlling the house sprinklers. Its expecting the [OpenSprinkler Pi](https://opensprinkler.com/product/opensprinkler-pi/) hardware. 
# UI Projects
## [Sannel.House.ThermostatUI](https://github.com/Sannel/Sannel.House.ThermostatUI)
A UWP App meant to be ran on a raspberry pi with a screen. Its meant to be the GUI on the thermostat of the home.
## Sannel.House.Mobile
A Xamarin app for controlling the thermostat and other aspects for sannel house.

# Device Projects
## Sannel.House.Weather.Station
Using Particle hardware. This project will monitor weather conditions and log them back to the MQTT bus.
## Sannel.House.Lawn.Puck
This project will monitor moisture conditions inside the lawn and log them back to the MQTT bus.
## Sannel.House.Room.Tempature
This project will monitor temperature conditions in different rooms and log them back to the MQTT bus.