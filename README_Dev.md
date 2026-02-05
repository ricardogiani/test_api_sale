# Configure PostGrees:
1. execute inside docker-compose.yml the service ambev.developerevaluation.database
2. change DefaultConnection inside  appsettings.json on webapi

# Configure migrations with settings another projeto
dotnet ef migrations add InitialCreate --project Ambev.DeveloperEvaluation.ORM --startup-project Ambev.DeveloperEvaluation.WebApi

# Apply migrations using settings another projeto
dotnet ef database update --project Ambev.DeveloperEvaluation.ORM --startup-project Ambev.DeveloperEvaluation.WebApi

# Start Kafka:
execute inside docker-compose.yml the service kafka

#  Create topic:
1. docker exec --workdir /opt/kafka/bin/ -it kafka sh
2. ./kafka-topics.sh --create --topic quickstart-events --bootstrap-server localhost:9092

# Configure settings Kafka:
change values BootstrapServers and Topic inside appsettings.json

#  Execute WebApi:
dotnet run


