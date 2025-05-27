<div align="left">

  <h1>GUI for Diskpart - GUIFD</h1>

  <p>
  <h3>Currently refactoring to MVP View-Model (as of the end of May 2025)</h3>
  Yes, the project is still alive! I intend to add more features, adding stuff to the wiki and some UI adjustments, before this is considered "done".<br />
  But MVP refactor comes first!
  </p>
  <br />
  <p>
    <b>DOWNLOAD BELOW!</b><br/><br/>
    Building a GUI for Microsoft Diskpart and some Windows onboard features around disk space management.<br />
    A learning project.<br /><br />
    <div align="center">
      <img src="https://github.com/LumiToad/GUIForDiskpart/assets/129980440/03e801e7-2718-4f6a-a0d2-add00cd599e4" alt="diskpart image" width="400" height="400"/><br /><br />
    </div>
    Developement started during the 3 weeks of summer break 2023 in the <a href="https://www.school4games.net">School For Games</a>.<br />
    It was just one of these days, were you help a friends friend for 10h, halfway across the globe, fixing their virus infected Laptop,<br />
    when I was so annoyed with Windows on-board disk management tools, that I decided to try something.<br /><br />
    The initial idea was to build a tool, which can format any flash drive, no matter what filesystem is on it.<br />
    In this case, it was a Linux boot stick. Windows can delete it, using Diskpart.<br />
    But explaining the commands via Discord was not that easy and Diskpart syntax is also... lacking.<br />
    So I tried coding a small prototype and tested it on an old PlayStation 4 HDD, which had 15 propietary ext4 partitions on it.<br />
    I was able to delete them with one click, which gave me the idea of making a whole software out of it!<br /><br />
    Also, while developing games sure is a dream, one reason why I picked programming was,<br />
    because I always wanted to be able to develop apps as well!<br /><br />
    This project taught me a lot! I worked on it a few hours a week during school.<br />
    Initialy a summer break project, I am still working on it, when I have free time.<br />
    A lot of the code from earlier stages of developement will probably be replaced at some point.<br /><br />
    A developement diary is online on <a href="https://www.linkedin.com/in/lukas-schmidt-93b532256/">my LinkedIn</a>:<br />
    <ul>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-01-first-steps-lukas-schmidt/?trackingId=WVK4aIkqQZOT0Qj%2F7jFHBw%3D%3D">#01 - First Steps</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-02-first-issues-lukas-schmidt/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">#02 - First Issues</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-03-some-ui-lukas-schmidt/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">#03 - Some UI</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-04-actually-useful-lukas-schmidt/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">#04 - Actually useful</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-05-more-issues-lukas-schmidt-w56pe/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">#05 - More Issues</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-06-overscoped-lukas-schmidt-cfzrf/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">#06 - Overscoped</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-07-finished-program-beta-lukas-schmidt-j2gdf/?trackingId=dfzLrGWKOs2nXPPXoTqAhA%3D%3D">#07 - A finished program (beta)</a></li>
      <li><a href="#">Coming soon...</a></li>
    </ul>
  </p>

  <hr />
  <h2>Engines / Languages</h2>
  Windows Presentation Foundation (.NET 6.0), WQL (Windows Query Language), C#, PowerShell, Batch, XAML
  
  <hr />
  <h2>Features</h2>
  Features are basically split in 5 Parts:
  <ul>
    <li><img src="https://github.com/LumiToad/GUIForDiskpart/blob/main/resources/diskpart.png" width="20" height="20" /> Diskpart commands</li>
    <li><img src="https://github.com/LumiToad/GUIForDiskpart/blob/main/resources/cmd.png" width="20" height="20" /> MS CMD commands</li>
    <li><img src="https://github.com/LumiToad/GUIForDiskpart/blob/main/resources/commandline.png" width="20" height="20" /> Powershell commands</li>
    <li><img src="https://github.com/LumiToad/GUIForDiskpart/blob/main/resources/guifd.png" width="20" height="20" /> GUIFD commands</li>
    <li>Easy access to some hidden information of your drives<br /> (e.g. Windows Storage Management infos)</li>
  </ul>
  
  A more detailed walkthrough of the features can be found in the <a href="https://github.com/LumiToad/GUIForDiskpart/wiki">WIKI</a>!

  <hr />
  <h2>Example scripts</h2>
  Here are some scripts, I'd like to show:<br /><br />
  <ul>
    <li><a href="https://github.com/LumiToad/GUIForDiskpart/blob/main/diskpart/DPFunctions.cs">Diskpart commands service class.</a></li>
    <li><a href="https://github.com/LumiToad/GUIForDiskpart/blob/main/main/CommandExecuter.cs">Service class to execute commands.</a></li>
    <li><a href="https://github.com/LumiToad/GUIForDiskpart/blob/main/main/WSM_GPT_PartitionTypes.cs">GUID types service class. prevents "magic strings" anywhere in code.<br /> There are a lot of classes like these.</a></li>
    <li><a href="https://github.com/LumiToad/GUIForDiskpart/blob/main/main/DiskRetriever.cs">Service class for retrieving drive data.</a></li>
  </ul>
  Feel free to explore the code. Keep in mind, that the whole thing is still in developement.

  <hr />
  <h2>Downloads and Website</h2>

  <h3>Website</h3>
  <a href="#">
    <img src="https://github.com/LumiToad/LumiToad/blob/main/img/banner/github_gui_banner.png" alt="gui banner" />
  </a>

  <a href="https://drive.google.com/file/d/1ztzVAGwbFWlIPmalTah8BCYWZPJBA2YK/view?usp=sharing">Download x64</a>
  <a href="https://drive.google.com/file/d/1RAhkcu7WOxz9CXCyY2SWVb07iWAeS6Cd/view?usp=sharing">Download x86</a>
  

  Main logo artwork by Lars Rocksch:
  - https://www.linkedin.com/in/lars-rocksch-10828a234/
  
</div>
