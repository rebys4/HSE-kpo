package ru.hse.ordersservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.ordersservice.entity.Order;

public interface OrderRepository extends JpaRepository<Order, Long> {
}
