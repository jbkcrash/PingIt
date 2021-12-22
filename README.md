# PingIt

This is a small application it is intended to give a robust desktop experience for ICMP echo. The project is created in Visual Studio 2019.

The ultimate goal is to make an application that isn't intrusive, but always visible, and still allow a direct and robust control over sending IPV4 and IPV6 pings. Recording the results of responses to visualize a "quality" thereof.

I created this application after our local Internet service provider started to have intermittent upstream issues. It was helpful to know the second the percieved quality went below a threshold, or the latency above one, allowing us to quickly report it happenning to the ISP. 

Obviously this problem isn't new, and there are many existing solutions, but I couldn't find that discrete, easy to invoke and clean up per se solution. I didn't like the idea of setting up a whole monitoring application, even if it could be done in a container, i.e. just to get an occasional alert for a temporary event. To be honest those solutions weren't as "instant" as one would expect in the world of network monitoring either. Therefore I developed PingIt to solve this problem. It is like running Ping in a console window, but desktop friendly.

If anything this code is a possilbe bad example of how to do ICMP and/or WPF applications in Windows, bad code that exercises some of the finer nuances of desktop apps.

```
As a disclaimer, though I do consider myself to be a great programmer, this application is just for fun. 

I have spent nowhere as much time or effort on this, as would be done in my professional capacity. 

I don't sit down at a computer after work very often just to code. Computer programming/engineering, when I am employed for it, is just a job! 

I do many other things as hobbies in my true spare time. FPV flying, Electronics - Digital and Analog, Musclecars, this list can go on for a while, but never on that list is "Application Programming".
```

Disclaimer now aside, take this application for what it is worth. it does work and I will improve it, but it will never see a majority of my spare time, as I am rarely in the mood to just code for fun anymore.

Thank you for looking at it though, hope you enjoy the work so far, and any help you want to lend on making this better for the community is apppreciated. I do think this application is a good idea for a network tool that as an Open Source project could grow into something very useful for a lot of people.



## Simple Crappy Directions

### Open the Application

When you open the application it will sit in the lower right corner of your desktop. It is transparent, all but the richTextBox it currently uses for a console output. It is without the Window controls. The idea of the application is to stay in focus where you can see it at a glance, and this state not to be interrupted by what else you may need to do on the desktop. However I have started to work on system tray interaction, starting with a minimize and restore option using the system tray.

### ICMP Controls

You can select the version of ICMP you want to use. Note - The IPV6 use case isn't as well exercised and my original intent was IPV4, so it may need work, but in the earliest testing it did work. It is definately not using the work intended to make the results more obvious visually yet, e.g utilizing Runs and Paragraphs within richTextBox.

You can enter an IP address to ping, and the actual payload if you desire. The default is ok for standard Ping, as in what is done with the CLI tool as default. There are also experimental controls for continous, e.g. flood like pings, and for random data in the payload. You can also adjust the MTU with using random data, up to approximately the MTU of most ethernet networks.

In the future, Fragment and other options will be exposed to adjust as well.

### Console

The console is currently a richTextBox, this will need replaced or at least massaged. Right now the text window just grows and grows, in the continous mode this can add up to large document. It is valuable to be able to save this file for practically free, but I do believe the lack of grooming the text is a problem. I can see the application size grow as it is being used. I am surely not freeing some needed items and there is a limit to who big a session's backlog can get, so this needs addressed soon.

I also started on drawing a simple meter over the console, one that will display a quality assesment and thresholds, etc... but that will likely only come after the console is settled.

### Buttons

Buttons should be pretty intuitive, the minimize will put the application in the system tray with a generic icon. In the future I want to make this icon reflect some of the status of the meter for example, or it could be a pause icon when not pinging, turn red when threshold is hit. Many possibilities.

