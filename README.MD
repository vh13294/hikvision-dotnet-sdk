# HikVision DVR & NVR Live-Viewer .Net 4.8 / C#

## Extension of HikVision Windows SDK
https://www.hikvision.com/en/support/download/sdk/

### Require Config in json format
- /PreviewDemo/bin/DeviceCfg.json 

```javascript
// DeviceCfg.json 
{
  "displayScreen": 0,
  "numberOfScreen": 3,
  "Devices": [
    {
      "ip": "192.168.1.1",
      "username": "admin",
      "password": "admin",
      "port": 8000,
      "channels": [1, 2, 7, 33]
    },
    {
      "ip": "192.168.1.2",
      "username": "admin",
      "password": "admin",
      "port": 8050,
      "channels": [1, 2, 4]
    },
    {
      "ip": "192.168.1.3",
      "username": "admin",
      "password": "admin",
      "port": 8000,
      "channels": [1, 3]
    }
  ]
}

// numberOfScreen
// 1 => 1
// 2 => 2x2
// 3 => 3x3
// 4 => 4x4
```

## Full screen windows application
![screenshot](./screenshot.png)


### startup.bat
- Bat file for windows startup application
- ./startup.bat

```bat
cd "C:\hikvision\PreviewDemo\bin"
start "" PreviewDemo.exe
```