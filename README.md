# MSN music sync for Windwos 10

## Table of Contents
<details><summary>Expand to see contents</summary>
  <p>

* **[Description](#Description)**<br />
* **[Motivation](#motivation)**<br />
* **[Getting Started](#getting-started)**<br />
* **[Deployment](#deployment)**<br />
* **[Documentation and acknowledgements](#documentation-and-acknowledgements)**<br />
* **[Author](#author)**<br />
* **[Contributing](#contributing)**<br />
* **[License](#license)**<br />

</p>
</details>

## Description
This is a small project to "bridge" the Windows 10 Media status with the MSN client, specifically, the [escargot project version of the client](https://escargot.chat/get-started). With this program you can modify the current song message that is displayer on messenger and you even have control on what information is beign shown. This means that anything that displays a status on facebook (i.e. Spotify, Youtube, etc.) will be displayed on Windows Messenger for others to see, just like the old times!

## Motivation
I found the [escargot project](https://escargot.chat/get-started) and decided to use it in order to play a bit with MSN again and I remembered the option to display the current song you are listening. After some digging I saw the [project supports that feature](https://www.escargot.chat/forums/threads/guide-messenger-status-integrations.20/) but it mostly needs you to use any old media player that connects to MSN in order to display the song that you are listening to. I didn't want to use any old media player since I don't save music files on my computers like I used to. I look into this a bit and found other [options for just displaying the spotify status](https://wink.messengergeek.com/t/release-wlm-now-playing-on-spotify/7990/1) and that was alright but some of those projects were "forgotten", had unresolved bugs or simply weren't 100% what I wanted. So, I decided to do what anyone on my position would do: take things into my own hands and research how to make this aplication for Windows 10.

## Getting Started

### Get the development tools:
If you want to use this project, modify it or just compile it yourself, you need four things: Visual Studio, the .NET cross-platform tools, the .NET desktop tools and the Universal Windows Platform tools. In order to download these you need to:
1. Download **Visual Studio community** ~~not VS Code~~ from the **[official website](https://visualstudio.microsoft.com/es/)** and install it. 
1. When the installer runs, the Visual Studio installer window will appear asking you for the complements that you want to add. Here you want to select the **.NET Core cross-platform development** option and before you click next. 
2. Select the **.NET desktop development** as well as the **Universal Windows Platform development**. These complements are important since the project used them for creating it's interface as well as working with the Windows 10 media API.
3. Click on "install" and the Visual Studio IDE will be installed with the components selected.

### Clone or Download this repository:
Now that you have the necessary tools to build, debug and modify this project, you now need to have it on your local machine. Simply clone or download this repository in any folder you want on your computer.

### Set up the project:
Open the Visual Studio IDE now, you can do so from the Visual studio installer window or simply searching for it on your computer's programs. If it's the first time you open it it might ask you for your Microsoft's account (you can skip this step), what colors you want to use, etc. After that is done It will give you a menu to create projects or open an existing one. Simply **click on the open an existing one** option and navigate to the folder where you previously cloned this repository. After this, wait a bit and once the project opens you are done.

## Deployment
The way I deploy this project is by using a command from the command line (the build configurations often make too many files instead of a simple `.exe`). But before you run it for the first time, follow these steps:
1. Open the folder where you have the repository.
1. Open the `TaskTrayUI` folder.
2. Create a folder named `Publish`.
3. Copy the `Resources` folder and paste it in the one you just created.
5. On the Visual Studio IDE open a [development terminal](https://devblogs.microsoft.com/visualstudio/say-hello-to-the-new-visual-studio-terminal/?WT.mc_id=-blog-scottha) or just open CMD, powershell or any terminal you have on Windows on the project's folder ~~(psst, it's the folder where the `.sln` file is)~~ and paste the following command:

```CMD
dotnet publish -r win-x86 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --output ".\TasktrayUI\Publish"
```
Once it runs, and assuming it didn't had any errors, you will have an `AudioSyncMSNUI.exe` file as well as an `AudioSyncMSNUI.pdb` file. The `AudioSyncMSNUI.pdb` can be removed but keep the `.exe` and the copy of the `Resources` folder togheter wherever you want to have the app. Once you have these element where you want to have them, you can simply run the program by double clicking on the `.exe` file.

## Documentation and acknowledgements
In order to create this project I used documentation projects and examples from different people and places, If anyone wants to create a smiliar thing for a MSN client I'd like to have an easy to find section for anyone to read on the subject and this is it. If you want to know more about Windows 10 media API or how to interact with messenger, these are the resources I used:

<details><summary>Expand to see contents</summary>
  <p>

* MSN message queue, how to use and documentation:
    * **[Segin's psymp3 wiki](https://github.com/segin/psymp3/wiki/MsnMsgrUiManager)** Was essential in understanding how to detect the MSN window as well as how to both format and send the message.
    * **[Ledyba's Clock_For_WindowsLiveMessenger](https://github.com/ledyba/Clock_For_WindowsLiveMessenger/blob/082c0979dfb4165a396ceb5a3c023947ecfe18a4/Clock/wlm.cpp)** Helped me a bit on how to organize the code for detecting and sending the message to MSN
    * **[lowjoel's jrmc-oss-plugins](https://github.com/lowjoel/jrmc-oss-plugins/blob/212813071571b3c2097bbe4302be843b5d467e27/NowPlaying/NativeMethods.cs)** I was using an incorrect `sendMessage` function from the `user23.dll`, this project was really helpful to find and know how to use the correct one.
    * **[Microsoft's findWindow](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-findwindowexw), [Microsoft's COPYDATASTRUCT](https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-copydatastruct), [sendMessage](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage) and [Message queues](https://docs.microsoft.com/en-us/windows/win32/winmsg/about-messages-and-message-queues)** Also helped me understand a lot about the structure of the messages and the syntax used for this.
    * **[This stackoverflow quetion](https://stackoverflow.com/questions/6779731/c-sharp-using-sendmessage-problem-with-wm-copydata)** was where I found how to call external `.dll`s from C# (really important to find the msn window and to send the message using the windows APIs).
* For the Window's media session API I mostly used the[ Microsoft's documentation](https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance) but I also used other sources as guide. All of them were:
    * **[Pipe's coding clues](http://blog.pipe01.net/2021/01/gsmtc.html)** For examples on how to call the Window's media transport protocol from a new C# .NET 5 project.
    * **[Microsoft's session manager documenation](https://docs.microsoft.com/en-us/uwp/api/windows.media.control.globalsystemmediatransportcontrolssessionmanager?view=winrt-19041)** To see what kinds of events are in the controller and other information on how to use it.
</p>
</details>

## Author
* [__Camilo Zambrano Votto__](https://github.com/cawolfkreo)

## Contributing
If anyone wants to give me any help or ideas, you cando so by making new [Issues](https://github.com/cawolfkreo/WindowsMediaToLiveMessenger/issues) or [Pull requests](https://github.com/cawolfkreo/WindowsMediaToLiveMessenger/pulls).

## License
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This repository has the standard MIT license. You can find it [here.](https://github.com/cawolfkreo/WindowsMediaToLiveMessenger/blob/master/LICENSE)