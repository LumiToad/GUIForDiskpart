<div align="left">
  
  <h1>GUI for Diskpart - GUIFD</h1>

  <p>
    Building a GUI for Microsoft Diskpart and some Windows onboard features around disk space management.<br />
    A learning project.<br /><br />
    <img src="https://github.com/LumiToad/GUIForDiskpart/assets/129980440/03e801e7-2718-4f6a-a0d2-add00cd599e4" alt="diskpart image" width="400" height="400"/><br /><br />
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
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-01-first-steps-lukas-schmidt/?trackingId=WVK4aIkqQZOT0Qj%2F7jFHBw%3D%3D">First Steps - #01</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-02-first-issues-lukas-schmidt/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">First Issues - #02</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-03-some-ui-lukas-schmidt/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">Some UI - #03</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-04-actually-useful-lukas-schmidt/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">Actually useful - #04</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-05-more-issues-lukas-schmidt-w56pe/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">More Issues - #05</a></li>
      <li><a href="https://www.linkedin.com/pulse/guifordiskpart-06-overscoped-lukas-schmidt-cfzrf/?trackingId=MQHtZADDSEm7DUZnMAyZxg%3D%3D">Overscoped - #06</a></li>
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

  <hr />
  <h2>Example scripts</h2>
  Here are some scripts, I'd like to show:<br /><br />
  <ul>
    <li><a href="https://github.com/LumiToad/ArcanumFortuna/tree/main/systems/ui/ui_base">UI base structure</a></li>
    This one needs explanation. As I joined the team two weeks before release,<br />
    there was a lot of prototype UI code still in the game.<br />
    We tried to rip out most of it, but it wasn't possible.<br />
    In order to work around that,<br />
    I came up with this system to add new canvas layers above the game,<br />
    so that mouse interactions won't cancel each other out.<br />
    I wrote my own class structure diagramm for that and once my lead kind of approved it, (he had a lot todo!)<br />
    we used it to build a lot of the UI screens, you can see in the final game. It was a huge time saver!
    <li><a href="https://github.com/LumiToad/ArcanumFortuna/tree/main/systems/tutorial">The entire tutorial</a></li>
    This was a lot of work. Besides the old Tutorial, everything here is my work.<br />
    It builds upon the card battle phase system, which was already part of the game.<br />
    <li><a href="https://github.com/LumiToad/ArcanumFortuna/blob/main/systems/ui/screen_fade_overlay.gd">Multi purpose screen fade it / out</a></li>
    <li><a href="https://github.com/LumiToad/ArcanumFortuna/tree/main/systems/ui/new_deck_preview">Deck preview screen</a></li>
    There is more, as some UI has been adjusted to the new structure...
  </ul>

  <hr />
  <h2>Downloads and Website</h2>

  <h3>Website</h3>
  <a href="#">
    <img src="https://github.com/LumiToad/LumiToad/blob/main/img/banner/github_gui_banner.png" alt="gui banner" />
  </a>
  
</div>
