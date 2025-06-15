// entity/Payment.java
package ru.hse.paymentsservice.entity;

import jakarta.persistence.*;
import lombok.*;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Payment {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String orderId;
    private Double amount;

    @Enumerated(EnumType.STRING)
    private String status;

    private LocalDateTime createdAt = LocalDateTime.now();

    public enum Status {
        CREATED, SUCCESS, FAILED
    }
}