# Visual Pinball Utilities
A suite of applications to help with managing, config, and backup of your vpin machine.
## Applications
### AutoFunctions
*These are functions that run on a timer.*

The functions contained are:

- **Rotate Screen**
- **USB Kill**
- **Move File**
- **App Kill**
  - this has not been tested

Use the App.config to modify settings.

#### configuration keys
- with all '_watchApp' or '_appName' keys if it ends with an '\*' it will see if the process name contains the string

`[Global]`

|Key|Value Type|Possible Values|Default Value(s)|Description|
|---|:---:|:---:|:---:|---|
|sleepTime|integer|0 - 86400|1|number of seconds to wait between checks|
|os-version|integer|32, 64|32|the bit of the OS ie: 32 or 64|
|config-file|string|[anything you want]|AutoFunctions.config|file name for your config|
|logging_enabled|boolean|true, false|true|enable or disable logging|
|log-file|string|[anything you want]|AutoFunctions.log|file name for your log|

`[Rotate Screen]`

|Key|Value Type|Possible Values|Default Value(s)|Description|
|---|:---:|:---:|:---:|---|
|rotate-screen_enable|boolean|true, false|false|enable or disable rotate screen|
|rotate-screen_monitor|integer|1 - 99|1|the index of the monitor (1=primary) for rotate screen|
|rotate-screen_watchApp|string|*application name*|pinballx\*|the case insensitive name of the application you want to monitor for rotate screen|

`[USB Kill]`

|Key|Value Type|Possible Values|Default Value(s)|Description|
|---|:---:|:---:|:---:|---|
|usb-kill_enable|boolean|true, false|false|enable or disable usb kill|
|usb-kill_deviceName|string|*device name*|LED-wiz|name of device you want to kill|
|usb-kill_watchApp|string|*application name*|pinballx\*|the case insensitive name of the application to (en/dis)able usb device|

`[Move File]`

- fromFolders and toFolders have a relationship in the order they are listed - the first 'from' will goto the first 'to', etc

|Key|Value Type|Possible Values|Default Value(s)|Description|
|---|:---:|:---:|:---:|---|
|move-file_enable|boolean|true, false|false|enables or disables the service|
|move-file_watchApp|string|*application name*|pinballx\*|the case insensitive name of the application to enable move file.  Leave blank to have ot run with service (always on)|
|move-file_overwrite|boolean|true, false|true|enables or disables overwriting existing files if they exist|
|move-file_extensions|string||\*|the file extensions you want moved seperated by a ';' semicolon.  An '\*' asterisk will check all files|
|move-file_fromFolders|string||C:\Emulators\PinballX\Media\Visual Pinball\Screen Grabs;C:\Emulators\PinballX\Media\Visual Pinball Physmod5\Screen Grabs;C:\Emulators\PinballX\Media\Visual Pinball X\Screen Grabs;C:\Emulators\PinballX\Media\P-ROC\Screen Grabs;|the folder you want to monitor for files seperated by a ';' semicolon|
|move-file_toFolders|string||C:\Emulators\PinballX\Media\Visual Pinball\Table Images;C:\Emulators\PinballX\Media\Visual Pinball Physmod5\Table Images;C:\Emulators\PinballX\Media\Visual Pinball X\Table Images;C:\Emulators\PinballX\Media\P-ROC\Table Images;|the folder you want the files moved into seperated by a ';' semicolon|

`[App Kill]`

|Key|Value Type|Possible Values|Default Value(s)|Description|
|---|:---:|:---:|:---:|---|
|app-kill_enable|boolean|true, false|false|enable or disable app kill|
|app-kill_appName|string|*application name*||the case insensitive name of the application you want to kill for application kill|
|app-kill_watchApp|string|*application name*|[-]pinballx\*|the case insensitive name of the application you want to watch for application kill [-] in front of the name denotes when this app is not open, [+] or nothing means when this app is open|

### USBLib
*This library is needed for AutoFunctions' USBKill as it gives access to USB devices on the machine.*
### VisualPinballBackupToolConsole
*The core functionality of the vpin backup tool.  This has not been completed.*
### VisualPinballUtilities
*The GUI for these utilities.*
