cls
docker stop docker-webapi
docker rm docker-webapi
docker run -d -p 8000:8000 --name docker-webapi webapi
docker container ps

-- http://localhost:8000/swagger/index.html