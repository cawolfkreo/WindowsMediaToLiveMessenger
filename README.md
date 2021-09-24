# MSN music sync for Windwos 10

# Build

```CMD
dotnet publish -r win-x86 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --output ".\Publish"
```

## Documentation used:

In order to create this project I used documentation and example from different people, this is the list of places where I received the information.
* MSN message queue, how to use and documentation:
    * **[Segin's psymp3 wiki](https://github.com/segin/psymp3/wiki/MsnMsgrUiManager)** Was essential in understanding how to detect the MSN window as well as how to both format and send the message.
    * **[Ledyba's Clock_For_WindowsLiveMessenger](https://github.com/ledyba/Clock_For_WindowsLiveMessenger/blob/082c0979dfb4165a396ceb5a3c023947ecfe18a4/Clock/wlm.cpp)** Helped me a bit on how to organize the code for detecting and sending the message to MSN
    * **[lowjoel's jrmc-oss-plugins](https://github.com/lowjoel/jrmc-oss-plugins/blob/212813071571b3c2097bbe4302be843b5d467e27/NowPlaying/NativeMethods.cs)** I was using an incorrect `sendMessage` function from the `user23.dll`, this project was really helpful to find and know how to use the correct one.
    * **[Microsoft's findWindow](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-findwindowexw), [Microsoft's COPYDATASTRUCT](https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-copydatastruct), [sendMessage](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage) and [Message queues](https://docs.microsoft.com/en-us/windows/win32/winmsg/about-messages-and-message-queues)** Also helped me understand a lot about the structure of the messages and the syntax used for this.
    * **[This stackoverflow quetion](https://stackoverflow.com/questions/6779731/c-sharp-using-sendmessage-problem-with-wm-copydata)** was where I found how to call external `.dll`s from C# (really important to find the msn window and to send the message using the windows APIs).
* For the Window's media session API I mostly used the[ Microsoft's documentation](https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance) but I also used other sources as guide. All of them were:
    * **[Pipe's coding clues](http://blog.pipe01.net/2021/01/gsmtc.html)** For examples on how to call the Window's media transport protocol from a new C# .Net 5 project.
    * **[Microsoft's session manager documenation](https://docs.microsoft.com/en-us/uwp/api/windows.media.control.globalsystemmediatransportcontrolssessionmanager?view=winrt-19041)** To see what kinds of events are in the controller and other information on how to use it.