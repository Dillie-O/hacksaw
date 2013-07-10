This package contains all the source code, as well as a Visual Studio 2008 solution/project file to use when working with HackSaw. Simply uncompress into the location desired and you are ready to go. 

You will need to setup your webserver or use the Visual Studio 2008 web server if you with to view/run HackSaw.

HackSaw requires the following resources to function properly:
* Microsoft .NET Framework version 2.0. (http://www.asp.net/downloads/essential.aspx?tabid=62)
* Microsoft AJAX Extensions (http://ajax.asp.net)
* A web server capable of running .NET framework applications, such as:
  - Internet Information Services (IIS)
  - Cassini (http://www.asp.net/Projects/Cassini/Download/)
  
Installation:
1. Unzip the core package to the directory of your choosing.
2. Open up the IIS manager, drill down to your web server, select Action->New->Virtual Directory.
3. Follow the wizard's directions, specify whatever alias you would like, though Hacksaw should be sufficient.
4. Browse to and select the directory you unzipped the files to.
5. Select "Next" on the final screen to allow Read and Run Scripts access.
6. Once the wizard has completed, right click on the virtual directory name and select "Properties."
7. Select the "Documents" tab, remove all existing entries for the default document, and add "Main.aspx" to the list.
8. Congratulations! Installation is complete.

Note: If you are using Hacksaw in an environment where you will need to access the logs from multiple servers, you may run into security issues that will prevent the log files from loading. To do this, there are a couple of options.

1. Grant the ASPNET user account for the web server machine read privileges to the folder/log file specified.
2. Grant everybody read priviliges to the folder/log file specified.
3. Set identity impersonate="true" in the web.config file. This will cause the web application to request these files as the user running the application. You may need to change the directory security of the virtual directory Hacksaw is located in to do some basic authentication to  store the uers's credentials in the browser.

Enjoy!!!