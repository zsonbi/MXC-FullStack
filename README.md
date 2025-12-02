# MXC Full-Stack projekt

### This project was made for the hiring process of MXC Software company

### The project specified to have to have premade users instead of register window which are:
| Username| Password |
| :--- | :--- |
| tester | String123 |
| janos | String123 |
| vonat | String123 |
| arlie.donnelly | String123 |


### Requirements
- Docker enviroment (Docker Desktop on windows, on linux please check out https://docs.docker.com/engine/install for the proper install commands)
- In case you want to run it on bare metal you will need dotnet for backend and node.js 22 for the frontend
- System requirements: negilible

### How to deploy
#### To deploy this project the easiest is to use the `docker-compose.yaml` file in the project's root. To run it you just need to run `docker compose --profile prod up` if you are developing and want to run the backend on bare metal use `docker compose --profile only-db up` which will only start the Database
