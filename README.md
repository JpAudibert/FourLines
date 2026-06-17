# FourLines
An app to schedule court/field for sports matches

## To start the environment
docker compose up -d

remember to start docker desktop / docker deamon

## Bruno collections 
FourLines.Api/Collections folder
Go in import workspace and do it

## To run migrations
dotnet ef migrations add "" -p FourLines.Api -s FourLines.Api
dotnet ef database update -p FourLines.Api -s FourLines.Api
