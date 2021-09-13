# Azure IoT Hub - Raspberry Pi connection

This is a simple project that allows the connection between Azure IoT Hub and Raspberry Pi.
An Azure function is also set in a way that executes an action everytime the IoT Hub receives data from the Raspberry Pi.

In our case, the Azure function will be sending and email everytime the Raspberry Pi sends telemetry data to the Azure IoT Hub if a given conditions are met (a sensor detects something)

## Block diagram

![image](https://user-images.githubusercontent.com/10405193/133066918-ed804b08-0b1e-4b9c-a86c-7f437fc3d2e0.png)

# Guide

## What you need

- An Azure account
- A Raspberry Pi (in this case a Raspberry Pi 4 with 8 GB RAM)
- **OPTIONAL** Some sensors and actuators to use: in this case we used some simulated data (temperature and humidity) and also we used an LED as actuator and a BISS0001 PIRR motion detector. 

## Azure IoT Hub configuration

First of all, we need to set up the Azure IoT Hub. For this, we should sign in in Azure Portal with our Azure account and select **Create a resource**, then we search and select **IoT Hub** and click on **Create**:

![image](https://user-images.githubusercontent.com/10405193/133061885-c533495c-9901-4d32-83de-d74e6d5f7637.png)

The Azure IoT Hub must be configured according to our needs. We should, at least, select a Subscription, a Resource group (we can create a new one for this), an IoT hub name and a Region:

![image](https://user-images.githubusercontent.com/10405193/133062176-0ef8f6bf-9d71-4624-a227-915ca23ec006.png)

For this purpose we also set the Azure IoT Hub to use the **Free Tier**:

![image](https://user-images.githubusercontent.com/10405193/133062425-58c3c3b2-f125-459f-b645-fa088eb12671.png)

### Create a new IoT device

After creating the IoT Hub we should be able to add new **IoT Devices** inside:

![image](https://user-images.githubusercontent.com/10405193/133063329-701d344b-6de0-4f4f-8444-4276fd57a946.png)

For this purpose, we can use the default values given when creating a new IoT Device, we must only specify a new for the device:

![image](https://user-images.githubusercontent.com/10405193/133063618-11aa4520-2197-4624-9e74-3e35d2cd32b2.png)

### Connection with IoT Hub and Azure function

In order to be able to connect the Raspberry Pi with the Azure IoT Hub we should use the **Primary Connection String** specified in the created device:

![image](https://user-images.githubusercontent.com/10405193/133064151-9e0fb527-d481-4dd9-b168-aeddf19e2c20.png)

This must copied in the code wherever **PRIMARY_CONNECTION_STRING** appears.

Also, to have the Azure function connected with the IoT Hub we should use the **Event Hub-compatible endpoint**:

![image](https://user-images.githubusercontent.com/10405193/133064707-fb0ab869-2ee8-4681-91c7-2289a27a2a42.png)

This must copied in the **local.settings.json** in the value of the **"ConnectionString** property in the Azure function code.

## Twin device communication

Every Azure IoT device has a twin device that can be used to synchronize the IoT device with the Azure IoT Hub. In this case, we'll use it to give certains order to the IoT device from the IoT Hub. To make this, we should add a property in the twin device JSON:

![image](https://user-images.githubusercontent.com/10405193/133067687-496ed549-0604-4bd1-bfff-132899a3961a.png)

![image](https://user-images.githubusercontent.com/10405193/133067868-d0caca99-af9b-48db-9a3f-fb7b5ecf4f89.png)

The way the code is set will make that if we change the **led** property value to '1', the red LED will be on.
