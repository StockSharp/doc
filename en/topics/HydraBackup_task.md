# S\#.Data settings

The following describes how to create and configure a backup task.

1. To create a task, click on the **Add tasks...** button, in the window that opens, select the **Backup** item and click the **OK** button..

   ![hydra tasks backup add](~/images/hydra_tasks_backup_add.png)
2. Next, you need to configure the task.

   ![hydra tasks backup](~/images/hydra_tasks_backup.png)

   **Backup**
   - **Service**

      \- the service address. 
   - **Address**

      \- the region address. The address of region specified in the bucket settings. See 

     [Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region)

     . 
   - **Storage**

      \- the bucket name. 
   - **Login**

      \- login. Access Key ID. 
   - **Password**

      \- password. 

     **Secret Access Key**

     . 
   - **Start date**

      \- from what date to start the backup. 
   - **Time offset**

      \- an offset in days from the current date. 

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

   **Logging**
   - **Identifier**

      \- the identifier. 
   - **Logging level**

      \- the logging level. 
3. After setting up the task, add the instruments that should be saved in the backup storage and click the **Start** button.

## Recommended content

[Creating and configuring an account](HydraBackup_account.md)
