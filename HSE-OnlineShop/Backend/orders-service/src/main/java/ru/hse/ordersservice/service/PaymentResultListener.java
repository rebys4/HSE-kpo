package ru.hse.ordersservice.service;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

@Component
public class PaymentResultListener {
    private final OrderService orderService;
    private final ObjectMapper objectMapper;

    @Autowired
    public PaymentResultListener(OrderService orderService, ObjectMapper objectMapper) {
        this.orderService = orderService;
        this.objectMapper = objectMapper;
    }

    @RabbitListener(queues = "payment.result")
    public void onPaymentResult(String message) {
        try {
            JsonNode node = objectMapper.readTree(message);
            Long orderId = node.get("orderId").asLong();
            String status = node.get("status").asText();
            orderService.updateOrderStatus(orderId, status);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
