package ru.hse.ordersservice.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hse.ordersservice.entity.Order;
import ru.hse.ordersservice.entity.OutboxEvent;
import ru.hse.ordersservice.repository.OrderRepository;
import ru.hse.ordersservice.repository.OutboxEventRepository;

import java.time.Instant;
import java.util.HashMap;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class OrderService {
    private final OrderRepository orderRepository;
    private final OutboxEventRepository outboxEventRepository;
    private final ObjectMapper objectMapper;

    @Transactional
    public Order createOrder(Long userId, Integer amount, String description) {
        Order order = Order.builder()
                .userId(userId)
                .amount(amount)
                .description(description)
                .status(Order.Status.NEW)
                .build();
        order = orderRepository.save(order);

        Map<String, Object> event = new HashMap<>();
        event.put("orderId", order.getId());
        event.put("userId", order.getUserId());
        event.put("amount", order.getAmount());
        event.put("description", order.getDescription());

        try {
            String payload = objectMapper.writeValueAsString(event);
            OutboxEvent outboxEvent = OutboxEvent.builder()
                    .type("OrderCreated")
                    .payload(payload)
                    .createdAt(Instant.now())
                    .published(false)
                    .build();
            outboxEventRepository.save(outboxEvent);
        } catch (Exception e) {
            throw new RuntimeException("Failed to serialize order event", e);
        }

        return order;
    }

    @Transactional
    public void updateOrderStatus(Long orderId, String paymentStatus) {
        orderRepository.findById(orderId).ifPresent(order -> {
            if ("success".equals(paymentStatus)) {
                order.setStatus(Order.Status.FINISHED);
            } else if ("failed".equals(paymentStatus)) {
                order.setStatus(Order.Status.CANCELLED);
            }
            orderRepository.save(order);
        });
    }
}