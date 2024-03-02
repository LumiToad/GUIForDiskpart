<div align="left">
  
  <h1>GUI for Diskpart - GUIFD</h1>

  <p>
    Building a GUI for Microsoft Diskpart and some Windows onboard features around disk space management.<br />
    A learning project.<br /><br />
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

  ![MainWindowUI](https://github.com/LumiToad/GUIForDiskpart/assets/129980440/03e801e7-2718-4f6a-a0d2-add00cd599e4)

  <hr />
  <h2>Engines / Languages</h2>
  Windows Presentation Foundation (.NET 6.0), WQL (Windows Query Language), C#, PowerShell, Batch, XAML
  
  <hr />
  <h2>Responsiblities</h2>
  - UI programming -<br /><br />
    After getting introduced into the code by my lead programmer,<br />
    we planned ahead, using a class structure diagramm. (UML)<br />
    I wrote one before, but this was my first "serious" use, which helped me <b>A LOT</b>, when coding.<br />
    Another responsibility: A lot of code had to be replaced, as a lot was still prototype code, not modular enough for further use.<br /><br />
  - Assisting the team as a programmer -<br /><br />
  Helping my team with implementation of assets and ideas was a daily given.<br />
  Since the team was amazing, there was not much work here.<br />
  I also took over some bug search / fixing, that my lead engineer simply had no time to attend to.<br />
  This turned out to work out really well, we made a great team!<br /><br />
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

  <h3>Itch</h3>
  <a href="#">
    <img src="https://github.com/LumiToad/LumiToad/blob/main/img/banner/github_gui_banner.png" alt="gui banner" />
  </a>
  
</div>
