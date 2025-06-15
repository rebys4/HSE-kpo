package ru.hse.paymentsservice.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {
    @Bean
    public Exchange paymentsExchange() {
        return ExchangeBuilder.topicExchange("payments-exchange").durable(true).build();
    }

    @Bean
    public Queue inboxQueue() {
        return QueueBuilder.durable("payments-inbox-queue").build();
    }

    @Bean
    public Binding inboxBinding() {
        return BindingBuilder
                .bind(inboxQueue())
                .to(paymentsExchange())
                .with("payments.event")
                .noargs();
    }

    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        return new RabbitTemplate(connectionFactory);
    }
}