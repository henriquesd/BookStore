#waiting 90 seconds to wait the provisioning and the start of the database
sleep 90s
#run the command to create the database
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MyDB@123" -i create-database-docker.sql