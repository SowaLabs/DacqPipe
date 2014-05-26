DacqPipe
========

DacqPipe (Data Acqisition Pipeline) is a massive RSS data acquisition tool. It consists of a series of components that interoperate to acquire and prepare Web documents for further analysis.

Clone and Build
---------------

1. Clone [DacqPipe](http://source.ijs.si/kt/dacqpipe.git) from the GIT repository into, for example, C:\Work\DacqPipe:

 ```
 git clone http://source.ijs.si/kt/dacqpipe.git C:\Work\DacqPipe
 ```

2. Clone the dependencies:
    * Clone [LATINO](http://source.ijs.si/mgrcar/latino.git) into C:\Work\LATINO (see [the LATINO readme file](http://source.ijs.si/mgrcar/latino/blob/master/README.md) for more details):

     ```
     git clone http://source.ijs.si/mgrcar/latino.git C:\Work\LATINO
     ```
    * Clone [LATINO Workflows](http://source.ijs.si/kt/latino-workflows.git) into C:\Work\LatinoWorkflows:

     ```
     git clone http://source.ijs.si/kt/latino-workflows.git C:\Work\LatinoWorkflows
     ```
    * Clone [SemWeb](http://source.ijs.si/kt/semweb.git) into C:\Work\SemWeb:

     ```
     git clone http://source.ijs.si/kt/semweb.git C:\Work\SemWeb
     ```
    * Clone [SharpNLP](http://source.ijs.si/kt/sharpnlp.git) into C:\Work\SharpNLP:

     ```
     git clone http://source.ijs.si/kt/sharpnlp.git C:\Work\SharpNLP
     ```

3. Open the solution file (C:\Work\DacqPipe\DacqPipe.sln) in Visual Studio. This solution file was created in Visual Studio 2008. If you are using a later version, VS will automatically upgrade it.

4. Build the solution.

Configure and Run
-----------------

1. Copy the contents of C:\Work\DacqPipe\DacqPipe\bin\Release to a deployment folder (for example, C:\DacqPipe).

2. To configure DacqPipe, edit the file DacqPipe.exe.config (located in the deployment folder) with a text editor. The configuration file contains a set of key-value pairs in the form &lt;add key="..." value="..."/>. The following table lists and explains the supported configuration keys:

 |Key                |Description                                                                                                                 |Default value  |
 |-------------------|----------------------------------------------------------------------------------------------------------------------------|---------------|
 |logFileName        |The name of the log file to which DacqPipe writes events (important mainly for debugging).                                  |Not set        |
 |xmlDataRoot        |The location to which the acquired (accepted) documents are stored in the XML format.                                       |Data           |
 |xmlDataDumpRoot    |The location to which the rejected documents (mostly duplicates) are stored in the XML format.                              |Not set        |
 |htmlDataRoot       |The location to which the acquired (accepted) documents are stored in their original HTML form.                             |DataHtml       |
 |htmlDataDumpRoot   |The location to which the rejected documents are stored in their original HTML form.                                        |Not set        |
 |htmlViewRoot       |The location to which the previews of the acquired (accepted) documents are stored. A preview is an HTML page displaying content, annotations, and metadata of the corresponding Web document.|Not set|
 |dataSourcesFileName|The name of the file containing RSS sources to be polled for content.                                                       |RssSources.txt |
 |dbConnectionString |The string containing information required to connect to the DacqPipe database.                                             |Server=(local);Database=DacqPipe;Trusted_Connection=Yes|
 |language           |The language in which the acquired (accepted) documents are written. Note that setting this to other than English turns off the NLP part of the pipeline.|English|
 |numPipes           |The number of parallel pipelines between which load balancing is performed. You should increase this if you see the RAM consumption constantly increasing (the queues are filling up). If this does not work, your system most likely does not have enough processing resources.|2|
 |sleepBetweenPolls  |The amount of time a RSS reader waits before polling its RSS feeds again from the start.                                    |00:15:00       |

3. Create the file with RSS sources. The name of this file is specified with the dataSourcesFileName configuration parameter. The file format is relatively simple and contains several lists of RSS sources, one for each Web site. Each RSS list starts with a site identifier (e.g., "Site: cnn"). The URLs of RSS sources are listed after the site identifier, each in its own line. This list ends with the next site identifier (or with the end of file). If a line starts with "#", which indicates a comment, it is ignored by DacqPipe. The following is an example of such a file:

     ```
     Site: cnn
     # Site: http://edition.cnn.com/
     # RSS list: http://edition.cnn.com/services/rss/
     http://rss.cnn.com/rss/edition.rss
     http://rss.cnn.com/rss/edition_asia.rss
     http://rss.cnn.com/rss/edition_europe.rss
     http://rss.cnn.com/rss/edition_us.rss
     http://rss.cnn.com/rss/edition_world.rss
     http://rss.cnn.com/rss/edition_africa.rss
     http://rss.cnn.com/rss/edition_americas.rss
     http://rss.cnn.com/rss/edition_meast.rss
     http://rss.cnn.com/rss/edition_business.rss
     http://rss.cnn.com/rss/edition_technology.rss
     http://rss.cnn.com/rss/edition_space.rss
     http://rss.cnn.com/rss/edition_entertainment.rss
     http://rss.cnn.com/rss/edition_sport.rss
     http://rss.cnn.com/rss/edition_football.rss
     http://rss.cnn.com/rss/edition_travel.rss
     http://rss.cnn.com/rss/cnn_freevideo.rss
     http://rss.cnn.com/rss/cnn_latest.rss
     http://rss.cnn.com/rss/edition_business360.rss
     http://rss.cnn.com/rss/edition_connecttheworld.rss
     http://rss.cnn.com/rss/edition_questmeansbusiness.rss
     http://rss.cnn.com/rss/edition_worldsportblog.rss
     http://rss.cnn.com/rss/edition_golf.rss
     http://rss.cnn.com/rss/edition_motorsport.rss
     http://rss.cnn.com/rss/edition_tennis.rss
     http://afghanistan.blogs.cnn.com/feed/
     http://news.blogs.cnn.com/feed/
     
     Site: mirror
     # Site: http://www.mirror.co.uk/
     http://www.mirror.co.uk/rss.xml
     
     Site: spiegel
     # Site: http://www.spiegel.de/international/
     # RSS list: http://www.spiegel.de/international/0,1518,643192,00.html
     http://www.spiegel.de/schlagzeilen/index.rss
     http://www.spiegel.de/international/index.rss
     http://www.spiegel.de/international/germany/index.rss
     http://www.spiegel.de/international/europe/index.rss
     http://www.spiegel.de/international/world/index.rss
     http://www.spiegel.de/international/business/index.rss
     http://www.spiegel.de/international/zeitgeist/index.rss
     http://www.spiegel.de/schlagzeilen/tops/index.rss
     ```

4. Create the database:

    1. Start SQL Server Management Studio.

    2. Create a new database.

    3. Run the script [CreateTables.sql](http://source.ijs.si/kt/dacqpipe/blob/master/DacqPipe/DB/CreateTables.sql) (contained in C:\Work\DacqPipe\DacqPipe\DB) on the newly created database.

    4. Make sure that the database connection string is set correctly in DacqPipe.exe.config.

5. Execute DacqPipe.exe. DacqPipe starts as a console-mode application. The console displays activity and error messages. The same messages are written into a log file if logging is enabled. 

DacqPipe is shut down by pressing Ctrl-C. The message "Ctrl-C command received." appears in the console. Note that DacqPipe needs some time to shut down gracefully as it needs to finalize the processing of document queues.

Acquired Data
-------------

Documents acquired with DacqPipe are internally stored as annotated document objects. An annotated document is described with features and contains annotations. An annotation gives a special meaning to a text segment (e.g., boilerplate, token, sentence) and can further be described with features.

DacqPipe stores acquired documents into files. The corresponding metadata is stored into the database. The database structure is very simple, containing practically only one table called Documents. Each record corresponds to one acquired (accepted) document. Apart from the metadata, a record contains the reference to the corresponding data files.

Each acquired document can be stored as a compressed XML (.xml.gz), compressed HTML (.html.gz), and/or preview HTML. While the HTMLs are the original documents acquired from the Web, the XMLs contain extracted and annotated content with additional metadata (features). In addition, a preview is an HTML page displaying content, annotations, and metadata of the corresponding Web document.

Each of these three datasets is stored into a separate root folder in which DacqPipe creates a separate folder for each day (e.g., &lt;xmlDataRoot>\2011\09\08\ would be created on September 8, 2011) and assigns unique names to data files. The name of a file consists of a time stamp and the document identifier (e.g., &lt;xmlDataRoot>\2011\09\08\14_29_33_c9bef21a1d4f4e4db0c82624d5b741bb.xml.gz). Note that the time stamp (the first 8 characters in the file name, i.e., hh_mm_ss) represents the acquisition time and not the publication time.

Advanced Config
---------------

You can configure the following advanced settings in DacqPipe.exe.config:

|Key                |Description                                                                                                                 |Default value  |
|-------------------|----------------------------------------------------------------------------------------------------------------------------|---------------|
|maxDocsPerCorpus   |Specifies how many acquired documents are bundled in a document corpus that is passed between the pipeline components. Smaller document corpora are more suitable for effective load balancing. On the other hand, larger document corpora are better for solving the cold start problem in the boilerplate removal process.                                               |50             |
|randomDelayAtStart |Specifies whether each RSS reader component should sleep for some amount of time before making the first request. Set this to "yes" if you experience problems with network traffic or simultanious requests at startup.|no|
|rssReaderDefaultRssXmlEncoding|Specifies the default encoding of retrieved RSS XML documents (used when encoding is not specified in the header or HTTP response).|ISO-8859-1|
|rssReaderDefaultHtmlEncoding|Specifies the default encoding of retrieved HTML documents (used when encoding is not specified in the header or HTTP response).|ISO-8859-1|
|urlRulesFileName   |Points to the file containing URL normalization rules required for boilerplate removal (see below*).                        |Not set        |
|urlBlacklistFileName|Points to the file specifying URLs from which the content should be rejected (see below**).                                |Not set        |

\* TBD

** TBD

Additional Notes
----------------

TBD

License
-------

Most of DacqPipe is under [the MIT license](http://opensource.org/licenses/MIT). However, certain parts and/or dependencies fall under other licenses. See [LICENSE.txt](http://source.ijs.si/kt/dacqpipe/blob/master/LICENSE.txt) for more details.