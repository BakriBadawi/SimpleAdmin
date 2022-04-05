# SimpleAdmin
Admin panel which is built using .net 6, entity core, web api, and blazor UI
to run this applicaiton using docker 
## docker compose 
using powershell go to the solution folder then run
```
docker-compose up -d
```
## docker
using powershell go to the solution folder then run
```
docker network create SimpleAdminNetwork
docker image build -t simpleadmindb -f ./SimpleAdmin.DataAccess/Dockerfile .
docker container run -d -p 12433:1433  --name SimpleAdmincdb --network SimpleAdminNetwork simpleadmindb
docker image build -t simpleadminapi -f ./SimpleAdmin.API/Dockerfile .
docker container run -d -p 8081:80  --name SimpleAdmincapi --network SimpleAdminNetwork simpleadminapi
docker image build -t simpleadminui -f ./SimpleAdmin.BlazorUI/Dockerfile .
docker container run -d -p 8082:80  --name simpleadmincui --network SimpleAdminNetwork simpleadminui
```
