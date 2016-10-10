# Redux-Conquer-Online-Server
This is an open-source implementation of Conquer Online server based on pro4never's Redux source code.

# To Do
- Add every monster spawn to DB. http://co.99.com/guide/monsters/monsters1.shtml
- Complete every reborn skills. (Iron Shirt, Freezing Arrow, Dash?)
- 2nd Reborn. (Reborn path in DB, quest: drop_rules and spawn, etc.)
- AI. (Monsters escape, improve searching algorithm)
- Dis City

# From pro4never:
This is a fully functional, ready to play server emulating patch 5065. It will have some missing minor features and the odd minor bug but as of the 3.0 release this should be a fully working source for you to use in your own development (no guarantees that it will be pretty though)

I've been hosting this server live for a week now and in this update have added, fixed or tweaked all the things users have reported to the best of my ability. There may be new bugs introduced by my changes but as of right now I'm considering this project 'done' and will be working on future updates privately and may consider hosting a live server with it.


# How do I use this source?!
https://www.youtube.com/watch?v=Yb_zBSnvM2Y

Setup is incredibly simple but you will need to ensure you have a proper version of mysql installed. Follow the steps below and you'll be running within minutes!

- Download Mysql: http://dev.mysql.com/downloads/installer/5.6.html
- Download the Source
- Execute the SQL backup (using any management software such as navicat)
- Create an Account in the database (using any mangement software such as navicat
- Run the source
- Enter the Server Information requested on first run (ip/name/db info)

NOTE: Use network, hamachi or external ip. 127.0.0.1 will not work.

NOTE: If you bluescreen trying to run conquer, remove the tq anti virus folder and you will be fine.

If you do not have a client already you will need to follow the next steps

- Download a 5065 client. (remember to delete tq antivirus folder, It's the strange named folder)
- Download ConquerLoader: (Credits to nullable)
- Unrar the loader into the 5065 client. Edit IP inside LoaderSet.ini

TADA! You're ready to play


# Original Source (pro4never):
http://www.elitepvpers.com/forum/co2-pserver-guides-releases/2692305-redux-v2-official-5065-classic-source.html


# Special Thanks to:
- pro4never: Original Creator.
- Many, many, many contributors from elitepvpers. See Original Source.
