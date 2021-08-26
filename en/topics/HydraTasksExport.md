# Export (auto)

The task exports exchange data into various formats: Excel, xml, sql, bin, Json or txt.

![hydra tasks export](~/images/hydra_tasks_export.png)

**Database**

- **Connection**

   \- connection to the database. Used in case of export via SQL. 
- **Packet**

   \- the size of transmitted data packet. By default, the size is 50 elements. Used in case of export via SQL. 
- **Uniqueness**

   \- check the data uniqueness in database. Affects performance. Enabled by default. Used in case of export via SQL. 

> [!TIP]
> When using export via SQL, you need to set the connection string parameters

**New connection string**

![hydra tasks connstring](~/images/hydra_tasks_connstring.png)

- **Provider**

   \- provider settings. 
- **Server**

   \- server address or path to database. 
- **Database**

   \- database name. Not used for SQLite. 
- **Login**

   \- login to access the database. Not used for anonymous access. 
- **Password**

   \- password for accessing the database. Not used for anonymous access. 
- **Windows**

   \- use the current Windows account to connect to the database. 
- **Connection**

   \- ready\-made connection string. 

> [!TIP]
> You can check the connection to database using the **Check** button.

**General**

- **Header**

   \- Converter. 
- **Working hours**

   \- setting up the board work schedule. 

  ![hydra tasks backup desk](~/images/hydra_tasks_backup_desk.png)
- **Interval of operation**

   \- the interval of operation. 
- **Data directory**

   \- data directory, from where the data for conversion will be received. 
- **Format**

   \- the converted data format: BIN\/CSV. 
- **Max. errors**

   \- the maximum number of errors, upon which the task will be stopped. By default, 0 \- the number of errors is ignored. 
- **Dependency**

   \- a task that must be performed before running the current one. 

**CSV**

- **Templates**

   \- Templates for each type of exported data. 
- **Header**

   \- the header in the first line. If an empty string is passed, the header will not be added to the file.
- **Name format**

   \- the format for recording the exported file name. 

**Export (auto)**

- **Type**

   \- export type (format). 
- **Start date**

   \- from what date to start exporting data. 
- **Time offset**

   \- time offset in days. 
- **Export directory**

   \- directory where data will be exported. 
- **Format**

   \- data format. 
- **Split**

   \- split type. 

**Logging**

- **Identifier**

   \- the identifier. 
- **Logging level**

   \- the logging leve. 

Let's consider an example of automatic export:

1. Select security.
2. Set up the market data that needs to be exported..

   ![hydra tasks export 00](~/images/hydra_tasks_export_00.png)
3. Set the export period. If the download of market data in real time is configured, then you can omit the end date of period. In this case, the data will be exported in real time, according to the work interval (data update). 

   ![hydra tasks export 01](~/images/hydra_tasks_export_01.png)
4. Setting up directories. Operation interval. Data type. Data format.
5. We start exporting.

   ![hydra tasks export 02](~/images/hydra_tasks_export_02.png)

Let's view the exported data

![hydra tasks export 03](~/images/hydra_tasks_export_03.png)

**Watch [video tutorial](HydraExportAutoVideo.md)**
