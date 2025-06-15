package ru.hse.ordersservice.config;

import org.springframework.amqp.core.*;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {
    public static final String ORDER_CREATED_QUEUE = "order.created";
    public static final String PAYMENT_RESULT_QUEUE = "payment.result";

    @Bean
    public Queue orderCreatedQueue() {
        return new Queue(ORDER_CREATED_QUEUE, true);
    }

    @Bean
    public Queue paymentResultQueue() {
        return new Queue(PAYMENT_RESULT_QUEUE, true);
    }
}