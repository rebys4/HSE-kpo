package ru.hse.ordersservice.service;

import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;
import ru.hse.ordersservice.config.RabbitMQConfig;
import ru.hse.ordersservice.entity.OutboxEvent;
import ru.hse.ordersservice.repository.OutboxEventRepository;

import java.util.List;

@Component
@RequiredArgsConstructor
public class OutboxPublisher {
    private final OutboxEventRepository outboxEventRepository;
    private final RabbitTemplate rabbitTemplate;

    @Scheduled(fixedDelay = 2000)
    public void publishOutboxEvents() {
        List<OutboxEvent> events = outboxEventRepository.findAllByPublishedFalse();
        for (OutboxEvent event : events) {
           rabbitTemplate.convertAndSend(RabbitMQConfig.ORDER_CREATED_QUEUE, event.getPayload());
           event.setPublished(true);
              outboxEventRepository.save(event);
        }
    }
}