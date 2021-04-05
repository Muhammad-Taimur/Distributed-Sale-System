# Distributed-Sale-System
This project is using NService Bus and RabbitMQ

In order to run this project we need to install Docker and run the RabbitMQ container
Below is the command to run after Docker is Installed.

docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
