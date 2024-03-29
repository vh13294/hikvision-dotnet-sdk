# HikVision DVR & NVR Live-Viewer .Net 4.8 / C#

## This project only support release build x64

## Extension of HikVision Windows SDK
https://www.hikvision.com/en/support/download/sdk/


### Application
- bin/PreviewDemo.exe

### Require Config in json format
- bin/DeviceConfig.json 

```javascript
// DeviceConfig.json 
{
  "displayScreen": 0,
  "numberOfScreen": 3,
  "Devices": [
    {
      "ip": "192.168.1.1",
      "username": "admin",
      "password": "admin",
      "port": 8000,
      "streamType": 0,
      "channels": [1, 2, 7, 33]
    },
    {
      "ip": "192.168.1.2",
      "username": "admin",
      "password": "admin",
      "port": 8050,
      "streamType": 0,
      "channels": [1, 2, 4]
    },
    {
      "ip": "192.168.1.3",
      "username": "admin",
      "password": "admin",
      "port": 8000,
      "streamType": 1,
      "channels": [1, 3]
    }
  ]
}

// displayScreen: support for multiple monitors, select the N monitor to display

// numberOfScreen: the number of video frames per monitor
// 1 => 1
// 2 => 2x2 total 4   videos live-view
// 3 => 3x3 total 9   videos live-view
// 4 => 4x4 total 16  videos live-view


// streamType
// 0 => main stream
// 1 => sub stream

```
## Note
- Please do not rename the bin directory


## Full screen windows application (PreviewDemo.exe)
![screenshot](./screenshot.png)


### startup.bat
- Batch file for windows startup application

```bat
cd "C:\bin\"
start "" PreviewDemo.exe
```