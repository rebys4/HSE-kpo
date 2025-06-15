package ru.hse.paymentsservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.paymentsservice.entity.InboxEvent;

import java.util.Optional;

public interface InboxEventRepository extends JpaRepository<InboxEvent, Long> {
    Optional<InboxEvent> findByEventId(String eventId);
}
