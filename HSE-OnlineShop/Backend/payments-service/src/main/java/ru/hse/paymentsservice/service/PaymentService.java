package ru.hse.paymentsservice.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hse.paymentsservice.dto.PaymentRequest;
import ru.hse.paymentsservice.entity.Payment;
import ru.hse.paymentsservice.entity.OutboxEvent;
import ru.hse.paymentsservice.repository.PaymentRepository;

import java.time.LocalDateTime;

@Service
@RequiredArgsConstructor
public class PaymentService {
    private final PaymentRepository paymentRepository;
    private final OutboxService outboxService;

    @Transactional
    public Payment processPayment(PaymentRequest request) {
        Payment payment = Payment.builder()
                .orderId(String.valueOf(request.getOrderId()))
                .amount(request.getAmount())
                .status(Payment.Status.SUCCESS)
                .createdAt(LocalDateTime.now())
                .build();
        paymentRepository.save(payment);

        // Outbox
        outboxService.saveEvent(
                "Payment",
                payment.getOrderId(),
                "PAYMENT_SUCCESS",
                "{\"orderId\":\"" + payment.getOrderId() + "\"}"
        );
        return payment;
    }
}