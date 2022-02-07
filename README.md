# OracleScriptRunner

You have to run multiple sql-scripts on multiple databases, you need an easy-to-use tool? Well, here it is.


Features:
  - Build batch- and summarized-sql-script to execute multiple scripts on multiple databases
    - select and order multiple sql scripts
    - select one or more database connections (optional)
    - "Build" will write all files into an empty folder and open the folder
      - all scripts to execute (including pre and post scripts) will be copied
      - execute-sql-script will execute: pre script -> all scripts in order -> post script
      - execute-batch-file will call sqlplus for all selected database connections and write all outputs into _log.txt
  - Settings
    - database connections (invalid connections are used as connection groups)
    - pre and post sqls
    - all settings are persisted in files located in ./settings folder
    - all changes to those settings are historized (when edited in GUI)


Features to come: 
- direct execution of 
- Terminal frontend
  
