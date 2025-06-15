package ru.hse.paymentsservice.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hse.paymentsservice.entity.InboxEvent;
import ru.hse.paymentsservice.repository.InboxEventRepository;

import java.time.LocalDateTime;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class InboxService {
    private final InboxEventRepository inboxEventRepository;

    @Transactional
    public boolean registerEventIfNotProcessed(String eventId, String aggregateType, String type, String payload) {
        Optional<InboxEvent> existing = inboxEventRepository.findByEventId(eventId);
        if (existing.isPresent()) return false;

        InboxEvent event = InboxEvent.builder()
                .eventId(eventId)
                .aggregateType(aggregateType)
                .type(type)
                .payload(payload)
                .receivedAt(LocalDateTime.now())
                .processed(true)
                .build();
        inboxEventRepository.save(event);
        return true;
    }
}