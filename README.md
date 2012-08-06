Inbox2 desktop client
=====================

![inbox2 desktop](http://www.phab1.com/delhi06/wp-content/uploads/2010/07/inbox2_desktop_full.jpg "inbox2 desktop")

This is the source-code for the Inbox2 desktop client. One day I will write a better readme file, but for now fork it, fix it and send pull requests.

For an overview of the client (back when we did a public commercial launch, old stuff), check out these links:

* http://lifehacker.com/5479009/inbox2-desktop-combines-and-task+ifies-your-email-and-social-networks
* http://techcrunch.com/2010/02/23/inbox2-debuts-public-beta-of-message-management-desktop-client-for-windows/

More is available on our press page: 

* http://www.inbox2.com/press

Whatever, I just want the download
----------------------------------

Here it is: https://github.com/downloads/waseems/inbox2_desktop/Inbox2Installer.msi

Its still a bit buggy here and there but that is what this GitHub page is for. Submit bug reports and if you happen to know your c#; fork it, fix it and send pull requests :-)

Why Inbox2
----------

* Looks better then anything else out there
* Aggregates messages from email accounts + social networking accounts
* Shows updates from your friend on social networks in a side bar

Why open-source?
----------------

Check out the announcement on hackernews with my comments (waseem)

* http://news.ycombinator.com/item?id=3083935

Mad props
---------

* Waseem Sadiq (waseem@inbox2.com, www.twitter.com/waseemsadiq) - code jedi
* Dinesh Duggal (dinesh@inbox2.com) - interaction design clone trooper
* Moin Sayed (www.phab1.com) - design wookie
* Khuram Hussain (khuram@inbox2.com, www.twitter.com/khuramhussain) - senetor palpatine before he became darth sidious :-)

How to build this thing
-----------------------

* Open the Code\Inbox2 Client Only.sln solution
* Open the Configuration Manager (under the debug dropdown, choose Configuration Manager...)
* Under the "Active solution platform" dropdown choose x86
* Hit menu Build->Build Solution

This builds the client for x86. Since we use a couple of native libraries we need to do it like this. 
There is also a solution available for doing an x64 build but that one is a bit behind, will be fixed soon.

Data folder
-----------

Your data folder can be found under the windows roaming folder, for example under Windows 7 for me that is. 

	C:\Users\waseem\AppData\Roaming

Throwing away this folder resets everything for a fresh start

Preconfigured channels
----------------------

If you need to setup the same times often, create a file in the root of your C drive called PreChannels.xml. Its contents look like this:

	<channels>
		<channel name="GMail" username="waseem@inbox2.com" password="mysecretpassword" />
		<channel name="Exchange" hostname="https://exchange.somewhere.com/owa" username="waseem" password="mysecretpassword" />
	</channels>

Now everytime you open the setup these channels will be prefilled and you only need to hit the add button.

What about a mac version?
-------------------------

Maybe some day, if you are an objective-c/c/c++/cocoa wizard and you want to take this up, ping me.