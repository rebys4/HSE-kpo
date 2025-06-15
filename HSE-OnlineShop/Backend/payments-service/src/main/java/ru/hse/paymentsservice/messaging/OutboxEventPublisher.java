package ru.hse.paymentsservice.messaging;

import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;
import ru.hse.paymentsservice.entity.OutboxEvent;
import ru.hse.paymentsservice.service.OutboxService;

import java.util.List;

@Component
@RequiredArgsConstructor
public class OutboxEventPublisher {
    private final OutboxService outboxService;
    private final RabbitTemplate rabbitTemplate;

    @Scheduled(fixedDelay = 2000)
    public void publishUnsentEvents() {
        List<OutboxEvent> unsent = outboxService.findUnsent();
        for (OutboxEvent event : unsent) {
            rabbitTemplate.convertAndSend("payments-exchange", "payments.event", event.getPayload());
            outboxService.markAsSent(event.getId());
        }
    }
}