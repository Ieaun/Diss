FROM rabbitmq:3-management
ADD definitions.json /etc/rabbitmq/
ADD rabbitmq.config /etc/rabbitmq/
RUN chown rabbitmq:rabbitmq /etc/rabbitmq/rabbitmq.config /etc/rabbitmq/definitions.json

EXPOSE 5671 5672 15671 15672 

CMD ["rabbitmq-server"]