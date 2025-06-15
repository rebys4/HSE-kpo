package ru.hse.ordersservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.ordersservice.entity.OutboxEvent;

import java.util.List;

public interface OutboxEventRepository extends JpaRepository<OutboxEvent, Long> {
    List<OutboxEvent> findAllByPublishedFalse();
}
