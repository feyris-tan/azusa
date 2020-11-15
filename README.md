# What's this?
You've stumbled upon something I call "Azusa", an application I use to manage my lifestyle.
I'm a collector of all sorts of media, and I use this application to manage the media I collected, and also make sure of its integrity.

To most people, this probably won't be of much use as this is specifially tailored to my needs. But maybe there is someone on the internet (or someone who finds this is the arctic code vault) who finds this interesting.

Do not expect any fancy web design tricks or colorful graphics from this, as this is a Windows Forms Application. I'm pretty much a desktop guy, and do not like CSS/HTML/JS at all.

# What can it do?
- Keep a list of your media collection, including date of purchase, cost, article number, cover scans, and screenshots.
	- For optical media, cue sheets, IBG Graph Data, checksums, CD-Text, Audio playlists, logfiles and a list of files may be attached.
- Help increasing a media collection, by keeping a local copy of relevant data from:
	- VGMDB.info
	- psxdatacenter.net
	- vndb.org
	- myfigurecollection.net
	- vocadb.net
	Tools to scrape data from these sources are included within this repository.
- Have all this on a second, offline machine.
	- I implemented this feature because I often go to flea markets, conventions and swap meets and lost track of all the stuff I already own. The whole database can be "sync"-ed and be read without access to your home database or internet access.
	- Connecting to your home database is still possible through an SSH tunnel.

# How to set this up?
1. Clone this repository
2. Setup a PostgreSQL Database and execute the setup.sql script into it.
3. Insert some lines into azusa_mediatypes and azusa_shelves. The column names should be self-explanatory.
4. Restore the NuGet Packages.
5. Build the AzusaERP project.
6. Run Azusa.ERP.exe
7. The program will complain that it wasn't able to find it's configuration. Hit yes when asked to run the setup utility.
8. Configure PostgreSQL Credentials in the set-up utility.
9. Close and re-start the program. It will complain about an invalid license. Hit yes when asked to run the setup utility.
10. Use the activation feature of the utility. Don't worry, this is not real licensing and totally free, but more like an anti-theft feature.
11. Close and re-start the program. Now you're ready to go.
