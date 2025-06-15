package ru.hse.paymentsservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.paymentsservice.entity.OutboxEvent;

import java.util.List;

public interface OutboxRepository extends JpaRepository<OutboxEvent, Long> {
    List<OutboxEvent> findAllBySentFalse();
}
