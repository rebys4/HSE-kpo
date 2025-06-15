package ru.hse.paymentsservice.messaging;

import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;
import ru.hse.paymentsservice.service.InboxService;

@Component
@RequiredArgsConstructor
public class PaymentEventListener {
    private final InboxService inboxService;

    @RabbitListener(queues = "payments-inbox-queue")
    public void handle(String message) {
        String eventId = extractEventId(message);
        if (!inboxService.registerEventIfNotProcessed(eventId, "Order", "ORDER_CREATED", message)) {
            return;
        }
        // Твоя логика на входящее событие...
    }

    private String extractEventId(String message) {
        // TODO: реализуй через Jackson
        return message.hashCode() + "";
    }
}