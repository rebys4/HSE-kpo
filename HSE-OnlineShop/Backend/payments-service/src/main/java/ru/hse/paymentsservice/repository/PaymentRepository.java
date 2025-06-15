package ru.hse.paymentsservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.paymentsservice.entity.Payment;

public interface PaymentRepository extends JpaRepository<Payment, Long> {
    Payment findByOrderId(String orderId);
}
