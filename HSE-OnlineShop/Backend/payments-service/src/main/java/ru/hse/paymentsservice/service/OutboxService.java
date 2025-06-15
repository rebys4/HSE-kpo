package ru.hse.paymentsservice.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hse.paymentsservice.entity.OutboxEvent;
import ru.hse.paymentsservice.repository.OutboxRepository;

import java.time.LocalDateTime;
import java.util.List;

@Service
@RequiredArgsConstructor
public class OutboxService {
    private final OutboxRepository outboxRepository;

    @Transactional
    public void saveEvent(String aggregateType, String aggregateId, String type, String payload) {
        OutboxEvent event = OutboxEvent.builder()
                .aggregateType(aggregateType)
                .aggregateId(aggregateId)
                .type(type)
                .payload(payload)
                .createdAt(LocalDateTime.now())
                .sent(false)
                .build();
        outboxRepository.save(event);
    }

    public List<OutboxEvent> findUnsent() {
        return outboxRepository.findAllBySentFalse();
    }

    @Transactional
    public void markAsSent(Long eventId) {
        outboxRepository.findById(eventId).ifPresent(event -> {
            event.setSent(true);
            outboxRepository.save(event);
        });
    }
}